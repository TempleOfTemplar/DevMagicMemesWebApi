using DevMagicMemesWebApi.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using OllamaSharp;
using OllamaSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using DevMagicMemesWebApi.Contracts.Ollama;

namespace DevMagicMemesWebApi.Services;


public class FileService(IWebHostEnvironment environment) : IFileService
{
    public async Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions)
    {
        if (imageFile == null)
        {
            throw new ArgumentNullException(nameof(imageFile));
        }

        var contentPath = environment.ContentRootPath;
        var path = Path.Combine(contentPath, "Uploads");
        // path = "c://projects/ImageManipulation.Ap/uploads" ,not exactly, but something like that

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // Check the allowed extenstions
        var ext = Path.GetExtension(imageFile.FileName);
        if (!allowedFileExtensions.Contains(ext))
        {
            throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
        }

        // generate a unique filename
        var fileName = $"{Guid.NewGuid().ToString()}{ext}";
        var fileNameWithPath = Path.Combine(path, fileName);
        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        return fileName;
    }
    public async Task<List<ResponseKeywords>> RunTagging()
    {

        var contentPath = environment.ContentRootPath;
        var path = Path.Combine(contentPath, "Uploads");
        // path = "c://projects/ImageManipulation.Ap/uploads" ,not exactly, but something like that

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        var uri = new Uri("http://localhost:11434");
        var ollama = new OllamaApiClient(uri);
        ollama.SelectedModel = "llava";
        //var prompt = "extract meme tags";
        //var prompt = "describe meme";
        //var prompt = "generate keywords, format your response in JSON";
        var prompt = "generate keywords, provide them in a single list format separated by comma.";
        string[] filePaths = Directory.GetFiles(path);
        //var base64ImageImages = new List<string>();
        List<ResponseKeywords> responseKeywords = new List<ResponseKeywords>();
        foreach (string filePath in filePaths)
        {
            var fileName = Path.GetFileName(filePath);
            var imageData = await File.ReadAllBytesAsync(filePath);
            string base64ImageRepresentation = Convert.ToBase64String(imageData);
            //base64ImageImages.Add(base64ImageRepresentation);
            var request = new GenerateRequest { Images = [base64ImageRepresentation], Prompt = prompt, Model = "llava" };
            var result = await ollama.Generate(request).StreamToEnd();
            var responce = result.Response;
            string[] pieces = responce.Split(new string[] { "," },
                                  StringSplitOptions.TrimEntries);
            ResponseKeywords keywords = new ResponseKeywords { imagePath = filePath, imageBase64 = base64ImageRepresentation, keywords = pieces};
            responseKeywords.Add(keywords);
            //ResponseKeywords keywords = JsonSerializer.Deserialize<ResponseKeywords>(responce);
            //if (keywords != null)
            //{
            //keywords.imagePath = filePath;
            //responseKeywords.Add(keywords);
            //}
        }
        return responseKeywords;
    }


    public void DeleteFile(string fileNameWithExtension)
    {
        if (string.IsNullOrEmpty(fileNameWithExtension))
        {
            throw new ArgumentNullException(nameof(fileNameWithExtension));
        }
        var contentPath = environment.ContentRootPath;
        var path = Path.Combine(contentPath, $"Uploads", fileNameWithExtension);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Invalid file path");
        }
        File.Delete(path);
    }

}