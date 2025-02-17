using Microsoft.Extensions.ObjectPool;

namespace HotChocolate.Execution.Processing;

internal sealed class ResultListPool : DefaultObjectPool<ResultObjectBuffer<ResultList>>
{
    public ResultListPool(int maximumRetained)
        : base(new BufferPolicy(), maximumRetained)
    {
    }

    private sealed class BufferPolicy : IPooledObjectPolicy<ResultObjectBuffer<ResultList>>
    {
        private static readonly ResultMapPolicy _policy = new();

        public ResultObjectBuffer<ResultList> Create() => new(16, _policy);

        public bool Return(ResultObjectBuffer<ResultList> obj)
        {
            obj.Reset();
            return true;
        }
    }

    private sealed class ResultMapPolicy : IPooledObjectPolicy<ResultList>
    {
        public ResultList Create() => new();

        public bool Return(ResultList obj)
        {
            obj.Clear();
            return true;
        }
    }
}
