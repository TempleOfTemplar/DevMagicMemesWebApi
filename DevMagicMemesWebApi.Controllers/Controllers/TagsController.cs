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
        /// Get single data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<Tag?> GetAsync([FromRoute] int id)
        {
            return _service.GetAsync(id, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Add single data
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<bool> AddAsync([FromBody] Tag tag)
        {
            return _service.AddAsync(tag, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Add multiple data
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [HttpPost("AddRange")]
        public Task<bool> AddRangeAsync([FromBody] IEnumerable<Tag> tags)
        {
            return _service.AddRangeAsync(tags, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Update single data
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPut]
        public Task<bool> UpdateAsync([FromBody] Tag tag)
        {
            return _service.UpdateAsync(tag, HttpContext.RequestAborted);
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
