using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;

namespace Jiper.RavenDB.DependencyInjection;

public static class RavenDBSetup
{
    public static void AddRavenDB(this IServiceCollection services)
    {
        services.AddRavenDB(options => { });
    }

    public static void AddRavenDB(this IServiceCollection services, Action<RavenDBOptions> configureRavenOptions)
    {
        services.AddOptions();
        services.Configure(configureRavenOptions);
        services.ConfigureOptions<ConfigureRavenDBOptions>();

        services.AddRavenStore();
    }

    private static void AddRavenStore(this IServiceCollection services)
    {
        services.AddSingleton<IDocumentStore, DocumentStore>(x =>
        {
            var options = x.GetRequiredService<IOptions<RavenDBOptions>>().Value;

            X509Certificate2? certificate = null;

            if (!string.IsNullOrWhiteSpace(options.Certificate))
            {
                certificate = CertificateLoader.LoadCertificate(options.Certificate, options.CertificatePassword);
            }

            var store = new DocumentStore
            {
                Urls = options.Urls.ToArray(),
                Database = options.Database,
                Certificate = certificate
            };

            store.Initialize();

            return store;
        });
    }
}