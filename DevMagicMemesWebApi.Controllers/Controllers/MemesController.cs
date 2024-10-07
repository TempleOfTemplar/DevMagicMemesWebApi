using DevMagicMemesWebApi.Common;
using DevMagicMemesWebApi.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevMagicMemesWebApi.Controllers
{
    [ApiController]
    [EntityValidation]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class MemesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IMemeService _service;
        private readonly ITagService _tagService;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<Meme> _memeRepository;

        public MemesController(IFileService fileService, IMemeService service, ITagService tagService,
            IRepository<Tag> tagRepository, IRepository<Meme> memeRepository)
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
        /// Get list data without filter.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        public Task<IEnumerable<Meme>> GetListAsync()
        {
            return _service.GetListAsync(HttpContext.RequestAborted);
        }

        /// <summary>
        /// Get list data without filter.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPage")]
        public Task<IEnumerable<Meme>> GetPageAsync()
        {
            return _service.GetPageAsync(HttpContext.RequestAborted);
        }

        /// <summary>
        /// Get list data by filter.
        /// </summary>
        /// <param name="withTagsNamesFilter"></param>
        /// <returns></returns>
        [HttpGet("SearchByTagsTitles")]
        public Task<IEnumerable<Meme>> SearchByTagsTitlesAsync(
            [FromQuery] Meme.WithTagsNamesFilter withTagsNamesFilter)
        {
            return _service.SearchByTagsTitlesAsync(withTagsNamesFilter, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Get list data by filter.
        /// </summary>
        /// <param name="withTagsIdsFilter"></param>
        /// <returns></returns>
        [HttpGet("SearchByTagsIds")]
        public Task<IEnumerable<Meme>> SearchByTagsIdsAsync(
            [FromQuery] Meme.WithTagsIdsFilter withTagsIdsFilter)
        {
            return _service.SearchByTagsIdsAsync(withTagsIdsFilter, HttpContext.RequestAborted);
        }
        
        [HttpGet("RunTagging")]
        public async Task<bool> RunTagging()
        {
            var taggedImages = await _fileService.RunTagging();
            foreach (var taggedImage in taggedImages)
            {
                await _service.AddAsync(new Meme { Image = taggedImage.imagePath });
                var addedMeme = _memeRepository.GetQueryable().FirstOrDefault(x => x.Image == taggedImage.imagePath);
                if (addedMeme != null)
                {
                    foreach (var keyword in taggedImage.keywords)
                    {
                        var findedTag = _tagRepository.GetQueryable().FirstOrDefault(x => x.Title == keyword);
                        if (findedTag == null)
                        {
                            await _tagService.AddAsync(new Tag { Title = keyword });
                            var addedTag = _tagRepository.GetQueryable().FirstOrDefault(x => x.Title == keyword);
                            if (addedTag != null)
                            {
                                await _tagService.AddMemesAsync(addedTag.Id, [addedMeme.Id]);
                            }
                        }
                        else
                        {
                            await _tagService.AddMemesAsync(findedTag.Id, [addedMeme.Id]);
                        }
                    }
                }
            }

            return true;
        }
    }
}
