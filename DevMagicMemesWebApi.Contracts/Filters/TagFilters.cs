using DevMagicMemesWebApi.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq.Expressions;

namespace DevMagicMemesWebApi.Contracts
{
    public partial class Tag
    {
        public class TitleFilter
        {
            public string? TagTitle { get; set; }

            public Expression<Func<Tag, bool>> ToExpression(bool defaultValue = false)
            {
                Expression<Func<Tag, bool>>? expression = null;

                if (!String.IsNullOrEmpty(this.TagTitle))
                {
                    expression = expression.And(x => x.Title!.Contains(this.TagTitle));
                }

                expression ??= x => defaultValue;

                return expression;
            }

        }
    }
}
