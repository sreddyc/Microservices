using System;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer.Extensions
{
    public static class Certificate
    {
        public static X509Certificate2 GetCertificate(this string certFile, string certKey)
        {
            try
            {

                // Create a collection object and populate it using the PFX file
                X509Certificate2Collection collection = new X509Certificate2Collection();
                collection.Import(certFile, certKey, X509KeyStorageFlags.PersistKeySet);


                if (collection.Count == 0)
                    throw new Exception($"Certificate for file {certFile} not found");

                return collection[0];



            }
            finally
            {


            }

        }
    }
}