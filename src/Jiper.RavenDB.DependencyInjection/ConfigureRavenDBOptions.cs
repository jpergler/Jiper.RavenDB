using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Jiper.RavenDB.DependencyInjection;

public class ConfigureRavenDBOptions(IConfiguration configuration) : IConfigureOptions<RavenDBOptions>
{
    public void Configure(RavenDBOptions options)
    {
        configuration.GetSection(RavenDBOptions.SectionName).Bind(options);

        if (!options.Urls.Any())
        {
            throw new Exception("RavenDB urls are not configured");
        }

        if (string.IsNullOrWhiteSpace(options.Database))
        {
            throw new Exception("RavenDB database is not configured");
        }
    }
}