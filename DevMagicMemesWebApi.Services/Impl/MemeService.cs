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
    }
}
