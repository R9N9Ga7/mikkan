using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Exceptions;
using Server.Interfaces.Services;
using Server.Models.Entities;
using Server.Models.Requests;

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
        } catch (UserAlreadyExistsException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    readonly IUserService _userService;
    readonly IMapper _mapper;
}
