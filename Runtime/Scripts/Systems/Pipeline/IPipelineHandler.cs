using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.Systems.Pipeline
{
    public interface IPipelineHandler
    {
    }

    public interface IPipelineHandler<THandler> : IPipelineHandler
    {
        THandler Handler { get; }
    }
}
