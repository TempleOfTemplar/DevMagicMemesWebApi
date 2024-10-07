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
    public class TagService : EntityServiceBase<Tag, int>, ITagService
    {
        private readonly ICurrentUser _currentUser;

        public TagService(IUnitOfWork unitOfWork, ICurrentUser currentUser) : base(unitOfWork)
        {
            _currentUser = currentUser;
        }

        public async override Task<bool> UpdateAsync(
            Tag tag, CancellationToken cancellationToken = default)
        {
            var existingTag = await _repository.GetQueryable()
                .Include(x => x.MemesMaps)
                .FirstOrDefaultAsync(
                x => x.Id == tag.Id, cancellationToken);

            if (existingTag is null)
            {
                return false;
            }

            existingTag.Title = tag.Title;
            existingTag.MemesMaps = tag.MemesMaps;

            var affected = await _unitOfWork.SaveChangeAsync(cancellationToken);

            return affected > 0;
        }

        public async Task<bool> AddMemesAsync(
            int tagId, int[] memeId, CancellationToken cancellationToken = default)
        {
            var data = await _repository.GetQueryable()
                .Include(x => x.MemesMaps)
                .FirstOrDefaultAsync(x => x.Id == tagId, cancellationToken);

            if (data is null)
            {
                return false;
            }

            foreach(var id in memeId)
            {
                if (data.MemesMaps.Any(x => x.TagId == tagId && x.MemeId == id))
                {
                    continue;
                }

                data.MemesMaps.Add(new Tag.MemesMap
                {
                    TagId = tagId,
                    MemeId = id
                });
            }

            var affected = await _unitOfWork.SaveChangeAsync(cancellationToken);

            return affected > 0;
        }

        public async Task<bool> RemoveMemesAsync(
            int tagId, int[] memeId, CancellationToken cancellationToken = default)
        {
            var data = await _repository.GetQueryable()
                .Include(x => x.MemesMaps)
                .FirstOrDefaultAsync(x => x.Id == tagId, cancellationToken);

            if (data is null)
            {
                return false;
            }

            foreach(var id in memeId)
            {
                if (!data.MemesMaps.Any(x => x.TagId == tagId && x.MemeId == id))
                {
                    continue;
                }

                data.MemesMaps.Remove(data.MemesMaps.First(x => x.TagId == tagId && x.MemeId == id));
            }

            var affected = await _unitOfWork.SaveChangeAsync(cancellationToken);

            return affected > 0;
        }
    }
}
