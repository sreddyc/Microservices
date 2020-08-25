using AutoMapper;
using IdentityModel;
using IdentityServer.Models;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer
{
    public class Config
    {
        private readonly IdentitySettings _identitySettings;
        private readonly IMapper _mapper;
        public Config(IdentitySettings identitySettings, IMapper mapper)
        {

            _identitySettings = identitySettings;
            _mapper = mapper;

        }
        public IEnumerable<ApiResource> GetApiResources()
        {
            // return new List<ApiResource>
            // {
            //     new ApiResource("resourceApi", "API Application")
            // };
             return _mapper.Map<IEnumerable<ApiResource>>(_identitySettings.ApiResources).ToList();
        }

        // scopes define the resources in your system
        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            // return new List<IdentityResource>
            // {
            //     new IdentityResources.OpenId(),
            //     new IdentityResources.Profile(),
            // };
             return _mapper.Map<IEnumerable<IdentityResource>>(_identitySettings.IdentityResources).ToList();
        }

        public IEnumerable<Client> GetClients()
        {
            return _mapper.Map<IEnumerable<Client>>(_identitySettings.Clients).ToList();
           
            // return new List<Client>
            // {
            //     new Client
            //     {
            //         ClientId = "clientApp",
 
            //         // no interactive user, use the clientid/secret for authentication
            //         AllowedGrantTypes = GrantTypes.ClientCredentials,
 
            //         // secret for authentication
            //         ClientSecrets =
            //         {
            //             new Secret("secret".Sha256())
            //         },
 
            //         // scopes that client has access to
            //         AllowedScopes = { "resourceApi" }
            //     },

            //     // OpenID Connect implicit flow client (MVC)
            //     new Client
            //     {
            //         ClientId = "mvc",
            //         ClientName = "MVC Client",
            //         AllowedGrantTypes = GrantTypes.Hybrid,

            //         ClientSecrets =
            //         {
            //             new Secret("secret".Sha256())
            //         },

            //         RedirectUris = { "http://localhost:5002/signin-oidc" },
            //         PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },
            //         AllowedScopes =
            //         {
            //             IdentityServerConstants.StandardScopes.OpenId,
            //             IdentityServerConstants.StandardScopes.Profile,
            //             "resourceApi"
            //         },
            //         AllowOfflineAccess = true
            //     }
            // };
        }

        public List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser{SubjectId = "818727", Username = "alice", Password = "alice",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
                    }
                },
                new TestUser{SubjectId = "88421113", Username = "bob", Password = "bob",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        new Claim("location", "somewhere")
                    }
                }
            };
        }
    }
}
