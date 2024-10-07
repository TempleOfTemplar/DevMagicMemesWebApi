using System;

namespace DevMagicMemesWebApi.Identity
{
    public class JwtTokenOptions
    {
        public string Issuer { get; set; } = String.Empty;

        public string Audience { get; set; } = String.Empty;

        public string SigningKey { get; set; } = String.Empty;
    }
}
