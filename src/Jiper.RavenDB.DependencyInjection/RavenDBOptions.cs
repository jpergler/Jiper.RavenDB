namespace Jiper.RavenDB.DependencyInjection;

public class RavenDBOptions
{
    public const string SectionName = "RavenDB";

    public ICollection<string> Urls { get; set; } = [];

    public required string Database { get; set; }

    public string? Certificate { get; set; }

    public string? CertificatePassword { get; set; }
}