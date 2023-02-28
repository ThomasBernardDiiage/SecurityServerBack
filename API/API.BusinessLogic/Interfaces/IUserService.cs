using API.BusinessLogic.Dto;

namespace API.BusinessLogic.Interfaces;

public interface IUserService
{
    Task<IEnumerable<GetUserDto>> GetUsersAsync();
    Task<GetUserDto> GetUserAsync(int id);
    Task<GetUserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task DeleteUserAsync(int id);
    Task<ModifyUserDto> ModifyUserAsync(ModifyUserDto modifyUserDto);
}

