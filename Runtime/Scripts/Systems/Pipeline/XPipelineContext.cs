#nullable enable
namespace TinaX.Systems.Pipeline
{
    public class XPipelineContext<THandler> where THandler : class
    {

        private readonly THandler m_Handler;

        public XPipelineContext(THandler handler)
        {
            m_Handler = handler;
            Next = null;
            Prev = null;
        }

        public virtual THandler Handler => m_Handler;

        public XPipelineContext<THandler>? Next { get; internal set; }
        public XPipelineContext<THandler>? Prev { get; internal set; }

    }
}
