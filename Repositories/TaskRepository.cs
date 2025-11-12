using WorkNest.Config;
using WorkNest.Models.Task;
using System.Reflection;
using WorkNest.Config;

namespace WorkNest.Repositories
{
    public class TaskRepository : Repository<Models.Task.Task>, ITaskRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
