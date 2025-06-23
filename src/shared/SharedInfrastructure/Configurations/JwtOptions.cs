namespace SharedInfrastructure.Configurations;

public class JwtOptions
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Key { get; set; } = null!;
    public int ClockSkewSeconds { get; set; } = 0;
}
