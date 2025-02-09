﻿using AutoMapper;
using BigTech.Application.Resources;
using BigTech.Domain.Dto.Role;
using BigTech.Domain.Dto.UserRole;
using BigTech.Domain.Entity;
using BigTech.Domain.Enum;
using BigTech.Domain.Interfaces.Repositories;
using BigTech.Domain.Interfaces.Services;
using BigTech.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace BigTech.Application.Services;
public class RoleService : IRoleService
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<Role> _roleRepository;
    private readonly IBaseRepository<UserRole> _userRoleRepository;
    private readonly IMapper _mapper;

    public RoleService(IBaseRepository<User> userRepository,
        IBaseRepository<Role> roleRepository, IMapper mapper, IBaseRepository<UserRole> userRoleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
        _userRoleRepository = userRoleRepository;
    }

    public async Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto dto)
    {
        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == dto.Name);
        if (role != null)
        {
            return new BaseResult<RoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleAlreadyExists,
                ErrorCode = (int)ErrorCodes.RoleAlreadyExists
            };
        }

        role = new Role()
        {
            Name = dto.Name
        };
        await _roleRepository.CreateAsync(role);
        return new BaseResult<RoleDto>()
        {
            Data = _mapper.Map<RoleDto>(role)
        };
    }

    public async Task<BaseResult<RoleDto>> DeleteRoleAsync(long id)
    {
        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
        if (role == null)
        {
            return new BaseResult<RoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }

        await _roleRepository.RemoveAsync(role);
        return new BaseResult<RoleDto>()
        {
            Data = _mapper.Map<RoleDto>(role)
        };
    }

    public async Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto dto)
    {
        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);
        if (role == null)
        {
            return new BaseResult<RoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }

        role.Name = dto.Name;
        var updatedRole = _roleRepository.UpdateAsync(role);

        return new BaseResult<RoleDto>()
        {
            Data = _mapper.Map<RoleDto>(updatedRole)
        };
    }

    public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto)
    {
        var user = await _userRepository.GetAll()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Login == dto.Login);

        if (user == null || user.Roles == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound
            };
        }

        var roles = user.Roles.Select(x => x.Name).ToArray();
        if (roles != null && roles.All(x => x != dto.RoleName))
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == dto.RoleName);
            if (role == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound
                };
            }

            UserRole userRole = new UserRole()
            {
                RoleId = role.Id,
                UserId = user.Id
            };
            await _userRoleRepository.CreateAsync(userRole);

            return new BaseResult<UserRoleDto>()
            {
                Data = new UserRoleDto()
                {
                    Login = user.Login,
                    RoleName = role.Name
                }
            };
        }

        return new BaseResult<UserRoleDto>()
        {
            ErrorMessage = ErrorMessage.UserAlreadyExistsThisRole,
            ErrorCode = (int)ErrorCodes.UserAlreadyExistsThisRole
        };
    }

    public async Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(DeleteUserRoleDto dto)
    {
        var user = await _userRepository.GetAll()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Login == dto.Login);

        if (user == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound
            };
        }

        var role = user.Roles.FirstOrDefault(x => x.Id == dto.RoleId);
        if (role == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }

        var userRole = await _userRoleRepository.GetAll()
            .Where(x => x.RoleId == role.Id)
            .FirstOrDefaultAsync(x => x.UserId == user.Id);
        await _userRoleRepository.RemoveAsync(userRole);
        //_userRoleRepository.Remove(userRole);
        //await _userRoleRepository.SaveChangesAsync();

        return new BaseResult<UserRoleDto>()
        {
            Data = new UserRoleDto()
            {
                Login = user.Login,
                RoleName = role.Name
            }
        };
    }

    public async Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto dto)
    {
        throw new NotImplementedException();
    }
}
