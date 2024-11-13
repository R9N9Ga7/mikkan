using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Exceptions;
using Server.Interfaces.Repositories;
using Server.Models.Entities;
using Server.Models.Requests;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController
{
    public AccountController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpPost("create")]
    public async Task<IResult> Create(UserCreateRequest userCreateRequest)
    {
        try
        {
            var user = _mapper.Map<User>(userCreateRequest);
            await _userRepository.Create(user);

            return Results.Created();
        } catch (UserAlreadyExistsException ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    readonly IUserRepository _userRepository;
    readonly IMapper _mapper;
}
