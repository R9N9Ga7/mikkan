﻿namespace Server.Settings;

public class AccountSettings
{
    public int SaltSize { get; set; }
    public int KeySize { get; set; }
    public int Iterations { get; set; }
    public int UserRegistrationsLimit { get; set; }
    public string EncryptionKey { get; set; } = null!;
}
