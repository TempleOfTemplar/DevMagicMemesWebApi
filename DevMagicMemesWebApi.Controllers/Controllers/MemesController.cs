using DevMagicMemesWebApi.Common;
using DevMagicMemesWebApi.Contracts;
using DevMagicMemesWebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace DevMagicMemesWebApi.Controllers
{
    [ApiController]
    [EntityValidation]
    [Route("api/[controller]")]
    public class MemesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IMemeService _service;
        private readonly ITagService _tagService;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<Meme> _memeRepository;

        public MemesController(IFileService fileService, IMemeService service, ITagService tagService, IRepository<Tag> tagRepository, IRepository<Meme> memeRepository)
        {
            _fileService = fileService;
            _service = service;
            _tagService = tagService;
            _tagRepository = tagRepository;
            _memeRepository = memeRepository;
        }

        /// <summary>
        /// Get single data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<Meme?> GetAsync([FromRoute] int id)
        {
            return _service.GetAsync(id, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Add single data
        /// </summary>
        /// <param name="meme"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<bool> AddAsync([FromBody] Meme meme)
        {
            return _service.AddAsync(meme, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Add multiple data
        /// </summary>
        /// <param name="memes"></param>
        /// <returns></returns>
        [HttpPost("AddRange")]
        public Task<bool> AddRangeAsync([FromBody] IEnumerable<Meme> memes)
        {
            return _service.AddRangeAsync(memes, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Update single data
        /// </summary>
        /// <param name="meme"></param>
        /// <returns></returns>
        [HttpPut]
        public Task<bool> UpdateAsync([FromBody] Meme meme)
        {
            return _service.UpdateAsync(meme, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Delete single data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task<bool> DeleteAsync([FromRoute] int id)
        {
            return _service.DeleteAsync(id, HttpContext.RequestAborted);
        }

        [HttpGet("RunTagging")]
        [AllowAnonymous]
        public async Task<bool> RunTagging()
        {
            var taggedImages = await _fileService.RunTagging();
            var memesToAdd = new List<Meme>();
            foreach (var taggedImage in taggedImages)
            {
                //Meme newMeme = new Meme { Image = taggedImage.imagePath };
                await _service.AddAsync(new Meme { Image = taggedImage.imagePath });
                var addedMeme = _memeRepository.GetQueryable().FirstOrDefault(x => x.Image == taggedImage.imagePath);
                if(addedMeme != null)
                {
                foreach (var keyword in taggedImage.keywords)
                {
                    var findedTag = _tagRepository.GetQueryable().FirstOrDefault(x => x.Title == keyword);
                    if (findedTag == null)
                    {
                        //await _tagRepository.AddAsync(new Tag { Title = keyword });
                        await _tagService.AddAsync(new Tag { Title = keyword });
                        var addedTag = _tagRepository.GetQueryable().FirstOrDefault(x => x.Title == keyword);
                        if(addedTag != null)
                        {
                            await _tagService.AddMemesAsync(addedTag.Id, [addedMeme.Id]);
                            //addedMeme.Tags.Add(addedTag)
                            //newMeme.TagsMaps.Add(addedTag);
                        }
                    } else {
                            await _tagService.AddMemesAsync(findedTag.Id, [addedMeme.Id]);
                            //addedMeme.Tags.Add(findedTag);
                        }

                    //var addedTag = await _tagService.AddAsync(new Tag { Title = keyword });
                    //newMeme.Tags.Add(addedTag);
                }

                }
                //memesToAdd.Add(new Meme { Image = taggedImage.imagePath });

            }


            //_service.AddRangeAsync(memes, HttpContext.RequestAborted);

            return true;
            //string[] filePaths = Directory.GetFiles(Path.Combine(this.Environment.WebRootPath, "Files/"));

            //return _service.GetAsync(id, HttpContext.RequestAborted);
        }
    }
}
