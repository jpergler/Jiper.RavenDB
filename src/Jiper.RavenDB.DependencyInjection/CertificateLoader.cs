using System.Security.Cryptography.X509Certificates;

namespace Jiper.RavenDB.DependencyInjection;

public static class CertificateLoader
{
    /// <summary>
    /// Returns true if the provided string is a Base64-encoded PKCS#12 certificate value,
    /// otherwise false (treated as a file path).
    /// Heuristic: no '.' present, length > 1000 chars, and decodes as Base64.
    /// </summary>
    public static bool IsCertificateValue(string? certificateOrPath)
    {
        if (string.IsNullOrWhiteSpace(certificateOrPath))
            return false;

        var s = certificateOrPath.Trim();

        // If it contains a dot, assume it's a file path (file extension)
        if (s.Contains('.'))
            return false;

        // Require a "way more than 1000 chars" length
        if (s.Length <= 1000)
            return false;

        // Validate that it is Base64
        try
        {
            _ = Convert.FromBase64String(s);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Loads an X509Certificate2 from a Base64-encoded PKCS#12 (PFX) value.
    /// </summary>
    public static System.Security.Cryptography.X509Certificates.X509Certificate2 LoadCertificateFromBase64(string base64, string? password)
    {
        var bytes = Convert.FromBase64String(base64.Trim());
        return new X509Certificate2(bytes, password);
    }

    /// <summary>
    /// Loads an X509Certificate2 from a PKCS#12 (PFX) file path.
    /// </summary>
    public static System.Security.Cryptography.X509Certificates.X509Certificate2 LoadCertificateFromFile(string path, string? password)
    {
        var bytes = System.IO.File.ReadAllBytes(path);
        return new X509Certificate2(bytes, password);
    }

    /// <summary>
    /// Loads an X509Certificate2 based on the input string being either a Base64 value or a file path.
    /// </summary>
    public static System.Security.Cryptography.X509Certificates.X509Certificate2 LoadCertificate(string certificateOrPath, string? password) =>
        IsCertificateValue(certificateOrPath)
            ? LoadCertificateFromBase64(certificateOrPath, password)
            : LoadCertificateFromFile(certificateOrPath, password);
}