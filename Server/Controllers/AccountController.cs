using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Exceptions;
using Server.Interfaces.Services;
using Server.Models.Dtos;
using Server.Models.Entities;
using Server.Models.Requests;
using Server.Models.Responses;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IUserService userService, IMapper mapper)
{
    [HttpPost("create")]
    public async Task<IResult> Create(UserCreateRequest userCreateRequest)
    {
        try
        {
            var user = _mapper.Map<User>(userCreateRequest);
            await _userService.Create(user);

            return Results.Created();
        }
        catch (UserAlreadyExistsException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (UserRegistrationLimitException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IResult> Login(UserLoginRequest userLoginRequest)
    {
        try
        {
            var user = _mapper.Map<User>(userLoginRequest);
            var userTokensDto = await _userService.Login(user);
            var userTokensResponse = _mapper.Map<UserTokensResponse>(userTokensDto);
            return Results.Json(userTokensResponse);
        }
        catch (UserNotFoundException)
        {
            return Results.Unauthorized();
        }
        catch (UserInvalidPasswordException)
        {
            return Results.Unauthorized();
        }
    }

    [HttpPost("refresh")]
    public async Task<IResult> Refresh(UserTokensRequest userTokensRequest)
    {
        try
        {
            var userTokensDto = _mapper.Map<UserTokensDto>(userTokensRequest);
            var refreshedTokensDto = await _userService.RefreshTokens(userTokensDto);
            var userTokensResponse = _mapper.Map<UserTokensResponse>(refreshedTokensDto);
            return Results.Json(userTokensResponse);
        }
        catch (UserUnauthorizedException)
        {
            return Results.Unauthorized();
        }
    }

    readonly IUserService _userService = userService;
    readonly IMapper _mapper = mapper;
}
