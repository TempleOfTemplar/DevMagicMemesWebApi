using DevMagicMemesWebApi.Common;
using DevMagicMemesWebApi.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DevMagicMemesWebApi.Services
{
    public class MemeService : EntityServiceBase<Meme, int>, IMemeService
    {
        private readonly ICurrentUser _currentUser;

        public MemeService(IUnitOfWork unitOfWork, ICurrentUser currentUser) : base(unitOfWork)
        {
            _currentUser = currentUser;
        }

        public async override Task<bool> UpdateAsync(
            Meme meme, CancellationToken cancellationToken = default)
        {
            var existingMeme = await _repository.GetQueryable()
                .Include(x => x.TagsMaps)
                .FirstOrDefaultAsync(
                x => x.Id == meme.Id, cancellationToken);

            if (existingMeme is null)
            {
                return false;
            }

            existingMeme.Image = meme.Image;
            existingMeme.Description = meme.Description;
            existingMeme.TagsMaps = meme.TagsMaps;

            var affected = await _unitOfWork.SaveChangeAsync(cancellationToken);

            return affected > 0;
        }

        public async virtual Task<IEnumerable<Meme>> GetListAsync(
            CancellationToken cancellationToken = default)
        {
            var result = await _repository.GetListAsync(cancellationToken);

            return result;
        }

        public async virtual Task<IEnumerable<Meme>> GetPageAsync(
            CancellationToken cancellationToken = default)
        {
            var result = await _repository.GetListAsync(cancellationToken);

            return result;
        }

        public async virtual Task<IEnumerable<Meme>> SearchByTagsTitlesAsync(
            Meme.WithTagsNamesFilter withTagsNamesFilter, CancellationToken cancellationToken = default)
        {
            var expression = withTagsNamesFilter.ToExpression(true);

            var result = await _repository.GetListAsync(expression, cancellationToken);

            return result;
        }

        public async virtual Task<IEnumerable<Meme>> SearchByTagsIdsAsync(
            Meme.WithTagsIdsFilter withTagsIdsFilter, CancellationToken cancellationToken = default)
        {
            var expression = withTagsIdsFilter.ToExpression(true);

            var result = await _repository.GetListAsync(expression, cancellationToken);

            return result;
        }

        public async virtual Task<IEnumerable<Meme>> GetListWithTagsAsync(
            Meme.GetListWithTags getListWithTags, CancellationToken cancellationToken = default)
        {
            var expression = getListWithTags.ToExpression(true);

            var result = await _repository.GetListAsync(expression, cancellationToken);

            return result;
        }

        public async virtual Task<PagedResult<Meme>> GetPageWithTagsAsync(
            PageParameter<Meme.GetListWithTags> parameter, CancellationToken cancellationToken = default)
        {
            var result = new PagedResult<Meme>();

            var expression = parameter.Filter.ToExpression(true);

            result.Data = await _repository.GetListAsync(expression, parameter, cancellationToken);

            result.Total = await _repository.CountAsync(expression, cancellationToken);

            return result;
        }
    }
}
