﻿namespace Server.Models.Dtos;

public class UserLoginDto
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}
