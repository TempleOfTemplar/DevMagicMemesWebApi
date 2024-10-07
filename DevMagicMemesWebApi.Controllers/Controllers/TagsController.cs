using DevMagicMemesWebApi.Common;
using DevMagicMemesWebApi.Contracts;
using DevMagicMemesWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevMagicMemesWebApi.Controllers
{
    [ApiController]
    [EntityValidation]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _service;

        public TagsController(ITagService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get list data without filter.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<Tag>> GetListAsync()
        {
            return _service.GetListAsync(HttpContext.RequestAborted);
        }

        /// <summary>
        /// Get paged data without filter.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("GetPageByNoFilter")]
        public Task<PagedResult<Tag>> GetPageAsync([FromQuery] PageParameter page)
        {
            return _service.GetPageAsync(page, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Get list data by filter.
        /// </summary>
        /// <param name="titleFilter"></param>
        /// <returns></returns>
        [HttpGet("GetListByTitleFilter")]
        public Task<IEnumerable<Tag>> SearchByTitleAsync([FromQuery] Tag.TitleFilter titleFilter)
        {
            return _service.SearchByTitleAsync(titleFilter, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Add service for "Tag.MemesMap" intermediate entity.
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="memeId"></param>
        /// <returns></returns>
        [HttpPost("AddMemes/{tagId}")]
        public Task<bool> AddMemesAsync([FromRoute] int tagId, [FromQuery] int[] memeId)
        {
            return _service.AddMemesAsync(tagId, memeId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Delete service for "Tag.MemesMap" intermediate entity
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="memeId"></param>
        /// <returns></returns>
        [HttpDelete("RemoveMemes/{tagId}")]
        public Task<bool> RemoveMemesAsync([FromRoute] int tagId, [FromQuery] int[] memeId)
        {
            return _service.RemoveMemesAsync(tagId, memeId, HttpContext.RequestAborted);
        }
    }
}
