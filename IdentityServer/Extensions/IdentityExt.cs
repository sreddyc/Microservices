using System.Collections.Generic;
using IdentityServer4.Models;

namespace IdentityServer.Extensions
{
    public static class ShaSecret
    {
        public static Secret GetShaSecret(this string secret)
        {
                 return new Secret(secret.Sha256());
        }

    }
}