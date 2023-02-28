using API.BusinessLogic.Dto;
using API.BusinessLogic.Interfaces;
using API.DataAccess.Interfaces.Repositories;
using API.Domain;
using API.Interface.UnitOfWork;

namespace API.BusinessLogic;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<GetUserDto>> GetUsersAsync()
    {
        var users = await _unitOfWork.GetRepository<IUserRepository>().GetUsersAsync();

        return users.Select(u => new GetUserDto
        {
            Id = u.Id,
            Email = u.Email,
            Firstname = u.Firstname,
            Lastname = u.Lastname,
            Picture = u.Picture
        });
    }

    public async Task<GetUserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = new User
        {
            Firstname = createUserDto.Firstname,
            Lastname = createUserDto.Lastname,
            Email = createUserDto.Email,
            Password = _passwordHasher.HashPassword(createUserDto.Password),
            Picture = createUserDto.Picture
        };

        await _unitOfWork.GetRepository<IUserRepository>().CreateUserAsync(user);

        await _unitOfWork.SaveChangesAsync();

        await _unitOfWork.GetRepository<IRoleRepository>().AffectRoleAsync(user.Id);

        await _unitOfWork.SaveChangesAsync();

        return new GetUserDto
        {
            Email = user.Email,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Picture = user.Picture,
            Id = user.Id
        };
    }

    public async Task DeleteUserAsync(int id)
    {
        _unitOfWork.GetRepository<IUserRepository>()
            .DeleteUserAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<GetUserDto> GetUserAsync(int id)
    {
        var u = await _unitOfWork.GetRepository<IUserRepository>()
            .GetUserAsync(id);

        return new GetUserDto
        {
            Id = u.Id,
            Email = u.Email,
            Firstname = u.Firstname,
            Lastname = u.Lastname,
            Picture = u.Picture
        };
    }

    public async Task<ModifyUserDto> ModifyUserAsync(ModifyUserDto modifyUserDto)
    {
        var user = new User {
            Id = modifyUserDto.Id,
            Email = modifyUserDto.Email,
            Firstname = modifyUserDto.Firstname,
            Lastname = modifyUserDto.Lastname,
            Picture = modifyUserDto.Picture
        };

        var u = await _unitOfWork.GetRepository<IUserRepository>().ModifyUserAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return modifyUserDto;
    }
}

