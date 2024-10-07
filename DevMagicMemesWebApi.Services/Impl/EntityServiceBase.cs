using DevMagicMemesWebApi.Common;
using DevMagicMemesWebApi.Contracts;

namespace DevMagicMemesWebApi.Services
{
    public abstract class EntityServiceBase<TEntity, TKey>
        : IEntityServiceBase<TEntity, TKey>
        where TEntity : class
        where TKey : notnull
    {
        protected readonly IUnitOfWork _unitOfWork;
        public readonly IRepository<TEntity> _repository;

        public EntityServiceBase(IUnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;
            _repository = _unitOfWork.GetRepository<TEntity>();
        }

        public virtual async Task<bool> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _repository.AddAsync(entity, cancellationToken);

            var affected = await _unitOfWork.SaveChangeAsync(cancellationToken);

            return affected > 0;
        }

        public virtual async Task<bool> AddRangeAsync(
            IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _repository.AddRangeAsync(entities, cancellationToken);

            var affected = await _unitOfWork.SaveChangeAsync(cancellationToken);

            return affected > 0;
        }

        public virtual async Task<bool> DeleteAsync(TKey key, CancellationToken cancellationToken = default)
        {
            bool result;

            if (key is object[] keys)
            {
                result = await _repository.DeleteAsync(keys, cancellationToken);
            }
            else
            {
                result = await _repository.DeleteAsync(new object[] { key }, cancellationToken);
            }

            if (result)
            {
                var affected = await _unitOfWork.SaveChangeAsync(cancellationToken);

                return affected > 0;
            }

            return result;
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _repository.Update(entity);

            var affected = await _unitOfWork.SaveChangeAsync(cancellationToken);

            return affected > 0;
        }

        public virtual async Task<bool> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            _repository.UpdateRange(entities);

            var affected = await _unitOfWork.SaveChangeAsync(cancellationToken);

            return affected > 0;
        }

        public virtual Task<TEntity?> GetAsync(TKey key, CancellationToken cancellationToken = default)
        {
            if (key is object[] keys)
            {
                return _repository.FindAsync(keys, cancellationToken);
            }
            else
            {
                return _repository.FindAsync(new object[] { key }, cancellationToken);
            }
        }

        public virtual Task<IEnumerable<TEntity>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return _repository.GetListAsync(cancellationToken);
        }

        public virtual async Task<PagedResult<TEntity>> GetPageAsync(
            PageParameter parameter, CancellationToken cancellationToken = default)
        {
            var result = new PagedResult<TEntity>();

            result.Total = await _repository.CountAsync(cancellationToken);

            result.Data = await _repository.GetListAsync(parameter, cancellationToken);

            return result;
        }

        public virtual Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return _repository.CountAsync(cancellationToken);
        }
    }
}
