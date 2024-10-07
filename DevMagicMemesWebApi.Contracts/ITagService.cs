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
        /// Get list data without filter.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Tag>> GetListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get paged data without filter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<PagedResult<Tag>> GetPageAsync(
            PageParameter parameter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get list data by filter.
        /// </summary>
        /// <param name="titleFilter"></param>
        /// <returns></returns>
        Task<IEnumerable<Tag>> SearchByTitleAsync(
            Tag.TitleFilter titleFilter, CancellationToken cancellationToken = default);

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
