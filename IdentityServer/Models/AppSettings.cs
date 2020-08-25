namespace IdentityServer.Models
{
    public class AppSettings : IAppSettings
    {

        public string DatabaseConnection { get; set; }
        public string CertificateFile { get; set; }
        public string CertificateKey { get; set; }
    }

    public interface IAppSettings
    {
        string DatabaseConnection { get; set; }
        string CertificateFile { get; set; }

        string CertificateKey { get; set; }
    }
}