using WorkNest.Models.Board;
using WorkNest.Models.Column;
using WorkNest.Models.Dto;
using WorkNest.Models.Task;
using WorkNest.Models.User;
using WorkNest.Models.User.Dto;
using AutoMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection;

namespace WorkNest.Config
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<bool?, bool>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<string?, string>().ConvertUsing((src, dest) => src ?? dest);

            CreateMap<RegisterDTO, User>();
            CreateMap<User, UserWithoutPassDTO>().ForMember(
                dest => dest.Roles,
                opt => opt.MapFrom(e => e.Roles.Select(x => x.Name).ToList())
            );

            CreateMap<Board, BoardDTO>();
            CreateMap<Board, BoardDetailsDTO>();
            CreateMap<CreateBoardDTO, Board>();

            CreateMap<Column, ColumnDTO>();

            CreateMap<Models.Task.Task, TaskDTO>();
            CreateMap<CreateTaskDTO, Models.Task.Task>();
            CreateMap<UpdateTaskDTO, Models.Task.Task>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
