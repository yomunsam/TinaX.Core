using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.Systems.Pipeline
{
    public class XPipelineContext<THandler>
    {
        private IPipelineHandler<THandler> _handler;
        public virtual IPipelineHandler<THandler> Handler => _handler;
        public XPipelineContext<THandler> Next { get; internal set; }
        public XPipelineContext<THandler> Prev { get; internal set; }

        public XPipelineContext(IPipelineHandler<THandler> handler)
        {
            _handler = handler;
            Next = null;
            Prev = null;
        }
    }
}
