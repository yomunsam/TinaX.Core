namespace TinaX.Systems.Pipeline
{
    public class XPipelineContext<THandler> where THandler : class
    {
        //private IPipelineHandler<THandler> _handler;
        //public virtual IPipelineHandler<THandler> PipelineHandler => _handler;

        private THandler _handler;
        public virtual THandler Handler => _handler;

        public XPipelineContext<THandler> Next { get; internal set; }
        public XPipelineContext<THandler> Prev { get; internal set; }

        

        public XPipelineContext(THandler handler)
        {
            _handler = handler;
            Next = null;
            Prev = null;
        }
    }
}
