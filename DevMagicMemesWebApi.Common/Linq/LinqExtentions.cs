using System.Linq.Expressions;

namespace DevMagicMemesWebApi.Common
{
    public static class LinqExtentions
    {
        public static Expression<Predicate<TModel>> And<TModel>(
            this Expression<Predicate<TModel>>? expression, Expression<Predicate<TModel>> expression1)
        {
            if (expression is not null)
            {
                var newParameter = Expression.Parameter(typeof(TModel));

                var newExpression = Expression.AndAlso(
                    Expression.Invoke(expression, newParameter),
                    Expression.Invoke(expression1, newParameter));

                return Expression.Lambda<Predicate<TModel>>(newExpression, newParameter);
            }
            else
            {
                return expression1;
            }
        }

        public static Expression<Predicate<TModel>> Or<TModel>(
            this Expression<Predicate<TModel>>? expression, Expression<Predicate<TModel>> expression1)
        {
            if (expression is not null)
            {
                var newParameter = Expression.Parameter(typeof(TModel));

                var newExpression = Expression.OrElse(
                    Expression.Invoke(expression, newParameter),
                    Expression.Invoke(expression1, newParameter));

                return Expression.Lambda<Predicate<TModel>>(newExpression, newParameter);
            }
            else
            {
                return expression1;
            }
        }

        public static Expression<Func<TModel, bool>> And<TModel>(
            this Expression<Func<TModel, bool>>? expression, Expression<Func<TModel, bool>> expression1)
        {
            if (expression is not null)
            {
                var newParameter = Expression.Parameter(typeof(TModel));

                var newExpression = Expression.AndAlso(
                    Expression.Invoke(expression, newParameter),
                    Expression.Invoke(expression1, newParameter));

                return Expression.Lambda<Func<TModel, bool>>(newExpression, newParameter);
            }
            else
            {
                return expression1;
            }
        }

        public static Expression<Func<TModel, bool>> Or<TModel>(
            this Expression<Func<TModel, bool>>? expression, Expression<Func<TModel, bool>> expression1)
        {
            if (expression is not null)
            {
                var newParameter = Expression.Parameter(typeof(TModel));

                var newExpression = Expression.OrElse(
                    Expression.Invoke(expression, newParameter),
                    Expression.Invoke(expression1, newParameter));

                return Expression.Lambda<Func<TModel, bool>>(newExpression, newParameter);
            }
            else
            {
                return expression1;
            }
        }
    }
}
