using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace User.Identity
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("gateway_api","gateway api")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "android",
                    ClientName = "Guoyunx User Api",
                    AllowedGrantTypes = new List<string>
                    {
                        "SmsAuthCode"
                    },
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("pwd123".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        "gateway_api"
                    },
                }
            };
        }
    }
}