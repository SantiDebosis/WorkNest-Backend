using WorkNest.Enums;
using WorkNest.Models.User;
using WorkNest.Repositories;
using WorkNest.Utils;
using AutoMapper;
using System.Net;
using WorkNest.Models.User.Dto;
using WorkNest.Models.User;
using WorkNest.Repositories;
using WorkNest.Services;

namespace WorkNest.Services
{
    public class UserServices
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly IEncoderServices _encoderServices;
        private readonly RoleServices _roleServices;

        public UserServices(IUserRepository repo, IMapper mapper, IEncoderServices encoderServices, RoleServices roleServices)
        {
            _repo = repo;
            _mapper = mapper;
            _encoderServices = encoderServices;
            _roleServices = roleServices;
        }

        async public Task<List<UserWithoutPassDTO>> GetAll()
        {
            var users = await _repo.GetAllAsync();
            return _mapper.Map<List<UserWithoutPassDTO>>(users);
        }

        async public Task<User> GetOneByEmailOrUsername(string? email, string? username)
        {
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(email))
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Email and Username are empty");
            }

            var user = await _repo.GetOneAsync(x => x.Email == email || x.UserName == username);
            return user;
        }

        async public Task<UserWithoutPassDTO> CreateOne(RegisterDTO register)
        {
            var user = _mapper.Map<User>(register);

            user.Password = _encoderServices.Encode(user.Password);

            var role = await _roleServices.GetOneByName(ROLE.USER);
            user.Roles = new() { role };

            await _repo.CreateOneAsync(user);

            return _mapper.Map<UserWithoutPassDTO>(user);
        }
    }
}
