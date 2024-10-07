using DevMagicMemesWebApi.Common;
using DevMagicMemesWebApi.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DevMagicMemesWebApi.Contracts
{
    public interface ITagService : IEntityServiceBase<Tag, int>
    {
        /// <summary>
        /// Add service for "Tag.MemesMap" intermediate entity.
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="memeId"></param>
        /// <returns></returns>
        Task<bool> AddMemesAsync(int tagId, int[] memeId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete service for "Tag.MemesMap" intermediate entity
        /// </summary>
        /// <param name="tagId"></param>
        /// <param name="memeId"></param>
        /// <returns></returns>
        Task<bool> RemoveMemesAsync(
            int tagId, int[] memeId, CancellationToken cancellationToken = default);
    }
}
