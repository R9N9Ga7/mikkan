using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController
{
    [HttpGet]
    public IResult Get()
    {
        return Results.Json("Hello World!");
    }
}
