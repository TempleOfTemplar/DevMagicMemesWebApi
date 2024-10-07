using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMagicMemesWebApi.Contracts.Ollama
{
    public class ResponseKeywords
    {
        public string? imagePath { get; set; }
        public string[]? keywords { get; set; }
    }
}
