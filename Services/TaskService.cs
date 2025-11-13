using WorkNest.Enums;
using WorkNest.Models.Dto;
using WorkNest.Models.User;
using WorkNest.Repositories;
using WorkNest.Utils;
using AutoMapper;
using System.Data;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using WorkNest.Repositories;
using WorkNest.Models.User.Dto;

namespace WorkNest.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepo, IMapper mapper, IUserRepository userRepo)
        {
            _taskRepo = taskRepo;
            _mapper = mapper;
            _userRepo = userRepo;
        }

        public async Task<TaskDTO> CreateTask(CreateTaskDTO dto)
        {
            var user = await _userRepo.GetOneAsync(u => u.Id == dto.AssignedUserId);
            if (user == null)
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Assigned user not found");
            }


            var task = _mapper.Map<Models.Task.Task>(dto);
            await _taskRepo.CreateOneAsync(task);

            var taskDto = _mapper.Map<TaskDTO>(task);
            taskDto.AssignedUser = _mapper.Map<UserWithoutPassDTO>(user);

            return taskDto;
        }

        public async Task DeleteTask(int taskId)
        {
            var task = await _taskRepo.GetOneAsync(t => t.Id == taskId);
            if (task == null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, "Task not found");
            }
            await _taskRepo.DeleteOneAsync(task);
        }

        public async Task<TaskDTO> MoveTask(int taskId, MoveTaskDTO dto, ClaimsPrincipal userClaims)
        {
            var task = await _taskRepo.GetOneAsync(t => t.Id == taskId);
            if (task == null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, "Task not found");
            }

            var userId = int.Parse(userClaims.FindFirstValue("id") ?? "0");
            var userRoles = userClaims.FindAll(ClaimTypes.Role).Select(r => r.Value);


            if (task.AssignedUserId != userId && !userRoles.Contains(ROLE.ADMIN))
            {
                throw new HttpResponseError(HttpStatusCode.Forbidden, "You are not authorized to move this task");
            }


            task.ColumnId = dto.TargetColumnId;
            await _taskRepo.UpdateOneAsync(task);

            var updatedTask = await _taskRepo.GetOneAsync(t => t.Id == taskId);
            var assignedUser = await _userRepo.GetOneAsync(u => u.Id == updatedTask.AssignedUserId);

            var taskDto = _mapper.Map<TaskDTO>(updatedTask);
            taskDto.AssignedUser = _mapper.Map<UserWithoutPassDTO>(assignedUser);

            return taskDto;
        }
    }
}