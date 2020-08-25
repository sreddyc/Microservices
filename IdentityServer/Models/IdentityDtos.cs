using System.Collections.Generic;


namespace IdentityServer.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject(myJsonResponse); 

    public class IdentityResourceDto
    {
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool Required { get; set; }

        public bool Emphasize { get; set; }

        public bool ShowInDiscoveryDocument { get; set; }

        public bool NonEditable { get; set; }

    }
    public class ApiResourceDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }

    }

    public class ClientDto
    {
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public string ProtocolType { get; set; }
        public ICollection<string> AllowedGrantTypes { get; set; }
        public bool RequireClientSecret { get; set; }
        public ICollection<string> ClientSecrets { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowRememberConsent { get; set; }
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public ICollection<string> RedirectUris { get; set; }
        public ICollection<string> PostLogoutRedirectUris { get; set; }
        public ICollection<string> AllowedScopes { get; set; }
        public bool RequirePkce { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public bool AllowOfflineAccess { get; set; }

    }

    public class IdentitySettings : IIdentitySettings
    {
        public List<IdentityResourceDto> IdentityResources { get; set; }
        public List<ApiResourceDto> ApiResources { get; set; }
        public List<ClientDto> Clients { get; set; }

    }

    public interface IIdentitySettings
    {
        List<IdentityResourceDto> IdentityResources { get; set; }
        List<ApiResourceDto> ApiResources { get; set; }
        List<ClientDto> Clients { get; set; }

    }


}