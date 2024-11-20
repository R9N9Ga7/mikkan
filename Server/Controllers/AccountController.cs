using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Exceptions;
using Server.Interfaces.Services;
using Server.Models.Entities;
using Server.Models.Requests;
using Server.Models.Responses;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController
{
    public AccountController(IUserService userRepository, IMapper mapper)
    {
        _userService = userRepository;
        _mapper = mapper;
    }

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
            var userLoginDto = await _userService.Login(user);
            var userLoginResponse = _mapper.Map<UserLoginResponse>(userLoginDto);
            return Results.Json(userLoginResponse);
        }
        catch (UserNotFoundException _)
        {
            return Results.Unauthorized();
        }
        catch (UserInvalidPasswordException _)
        {
            return Results.Unauthorized();
        }
    }

    readonly IUserService _userService;
    readonly IMapper _mapper;
}
