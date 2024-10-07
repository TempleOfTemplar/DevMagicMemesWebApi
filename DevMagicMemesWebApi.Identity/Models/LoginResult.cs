using System;
using System.Text.Json.Serialization;

namespace DevMagicMemesWebApi.Identity
{
    public class LoginResult
    {
        public string TokenType { get; set; } = "Bearer";

        public string AccessToken { get; set; } = String.Empty;

        public int ExpireIn { get; set; } = 3600;
    }
}
