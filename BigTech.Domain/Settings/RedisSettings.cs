namespace BigTech.Domain.Settings;
public class RedisSettings
{
    public const string DefaultSection = "Redis";

    public required string Url { get; set; }

    public required string InstanceName { get; set; }
}