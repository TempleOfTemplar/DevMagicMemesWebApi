using System;

namespace DevMagicMemesWebApi.Identity
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string? Message { get; set; }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Data { get; set; } = default!;
    }
}
