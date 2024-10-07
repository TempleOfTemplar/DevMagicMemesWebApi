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
    }
}
