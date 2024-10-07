using DevMagicMemesWebApi.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;

namespace DevMagicMemesWebApi.Contracts
{
    public partial class Meme
    {
        public class WithTagsNamesFilter
        {
            public ICollection<string>? TagsTitles { get; set; }

            public Expression<Func<Meme, bool>> ToExpression(bool defaultValue = false)
            {
                Expression<Func<Meme, bool>>? expression = null;

                if (this.TagsTitles is not null)
                {
                    expression = expression.And(x => x.Tags!.Any(x => this.TagsTitles!.Contains(x.Title!)));
                }

                expression ??= x => defaultValue;

                return expression;
            }

        }

        public class WithTagsIdsFilter
        {
            public ICollection<int>? TagsIds { get; set; }

            public Expression<Func<Meme, bool>> ToExpression(bool defaultValue = false)
            {
                Expression<Func<Meme, bool>>? expression = null;

                if (this.TagsIds is not null)
                {
                    expression = expression.And(x => x.Tags!.Any(x => this.TagsIds!.Contains(x.Id)));
                }

                expression ??= x => defaultValue;

                return expression;
            }

        }
    }
}
