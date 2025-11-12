using WorkNest.Enums;
using WorkNest.Repositories;
using WorkNest.Utils;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WorkNest.Config;
using WorkNest.Repositories;

namespace WorkNest.Services
{
    public class StatisticsService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IUserRepository _userRepo;
        private readonly ApplicationDbContext _db; 

        public StatisticsService(ITaskRepository taskRepo, IUserRepository userRepo, ApplicationDbContext db)
        {
            _taskRepo = taskRepo;
            _userRepo = userRepo;
            _db = db;
        }

        public class StatsDTO
        {
            public int TotalTasks { get; set; }
            public int TasksDone { get; set; }
            public int TasksInProgress { get; set; }
            public int TasksOverdue { get; set; }
            public object TasksPerUser { get; set; } = null!;
        }

        public async Task<StatsDTO> GetDashboardStats()
        {
            var tasksQuery = _db.Tasks.Include(t => t.Column); 

            int totalTasks = await tasksQuery.CountAsync();

            int tasksDone = await tasksQuery.CountAsync(t => t.Column.Name == "Done");
            int tasksInProgress = await tasksQuery.CountAsync(t => t.Column.Name == "In Progress");

            int tasksOverdue = await tasksQuery.CountAsync(
                t => t.DueDate < DateTime.UtcNow && t.Column.Name != "Done"
            );

            var tasksPerUser = await _db.Users
                .Select(u => new {
                    UserName = u.UserName,
                    Email = u.Email,
                    ActiveTasks = _db.Tasks.Count(t =>
                        t.AssignedUserId == u.Id &&
                        t.Column.Name != "Done"
                    )
                })
                .Where(u => u.ActiveTasks > 0) 
                .ToListAsync();

            return new StatsDTO
            {
                TotalTasks = totalTasks,
                TasksDone = tasksDone,
                TasksInProgress = tasksInProgress,
                TasksOverdue = tasksOverdue,
                TasksPerUser = tasksPerUser
            };
        }
    }
}
