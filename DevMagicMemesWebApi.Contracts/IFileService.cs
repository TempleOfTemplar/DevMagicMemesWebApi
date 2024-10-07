using DevMagicMemesWebApi.Contracts.Ollama;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevMagicMemesWebApi.Contracts
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions);
        void DeleteFile(string fileNameWithExtension);
        Task<List<ResponseKeywords>> RunTagging();
    }
}
