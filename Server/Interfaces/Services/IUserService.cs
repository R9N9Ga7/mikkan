﻿using Server.Models.Dtos;
using Server.Models.Entities;

namespace Server.Interfaces.Services;

public interface IUserService
{
    public Task<User> Create(User user);
    public Task<UserTokensDto> Login(User user);
    public Task<UserTokensDto> RefreshTokens(UserTokensDto userTokensDto);
}
