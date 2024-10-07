using DevMagicMemesWebApi.Common;
using DevMagicMemesWebApi.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DevMagicMemesWebApi.Contracts
{
    public interface IMemeService : IEntityServiceBase<Meme, int>
    {
        /// <summary>
        /// Get list data without filter.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Meme>> GetListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get list data without filter.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Meme>> GetPageAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Get list data by filter.
        /// </summary>
        /// <param name="withTagsNamesFilter"></param>
        /// <returns></returns>
        Task<IEnumerable<Meme>> SearchByTagsTitlesAsync(
            Meme.WithTagsNamesFilter withTagsNamesFilter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get list data by filter.
        /// </summary>
        /// <param name="withTagsIdsFilter"></param>
        /// <returns></returns>
        Task<IEnumerable<Meme>> SearchByTagsIdsAsync(
            Meme.WithTagsIdsFilter withTagsIdsFilter, CancellationToken cancellationToken = default);
    }
}
