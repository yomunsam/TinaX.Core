using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.Systems.Pipeline
{
    public class XPipeline<THandler>
    {
        private XPipelineContext<THandler> _head;
        private XPipelineContext<THandler> _last;

        public XPipelineContext<THandler> First => _head;

        public XPipeline()
        {
        }

        

        public XPipelineContext<THandler> AddFirst(IPipelineHandler<THandler> handler)
        {
            var context = new XPipelineContext<THandler>(handler);
            var _origin_first = _head;
            context.Next = _origin_first;
            _head = context;
            if(_origin_first != null)
            {
                _origin_first.Prev = _head;
                if (_last == null)
                    _last = _origin_first;
            }
            else
            {
                if (_last == null)
                    _last = context;
            }
            return context;
        }

        public XPipelineContext<THandler> AddLast(IPipelineHandler<THandler> handler) 
        {
            var context = new XPipelineContext<THandler>(handler);
            var origin_prev = _last;
            _last = context;

            if (origin_prev != null)
            {
                origin_prev.Next = context;
                context.Prev = origin_prev;
            }
            if (_head == null)
            {
                _head = origin_prev != null ? origin_prev : _last;
            }
            return context;
        }

        /// <summary>
        /// Start Pipeline
        /// </summary>
        /// <param name="callback"></param>
        public void Start(Func<IPipelineHandler<THandler>, bool> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (_head != null)
                DoPipeline(_head, callback);
        }

        /// <summary>
        /// Start Pipeline (Reverse
        /// </summary>
        /// <param name="callback"></param>
        public void StartReverse(Func<IPipelineHandler<THandler>, bool> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (_last != null)
                DoPipelineReverse(_last, callback);
        }

        private void DoPipeline(XPipelineContext<THandler> context, Func<IPipelineHandler<THandler>, bool> callback)
        {
            if (context != null && context.Handler != null)
            {
                bool doNext = callback(context.Handler);
                if (doNext && context.Next != null && context.Next != context)
                {
                    DoPipeline(context.Next, callback);
                }
            }
        }

        /// <summary>
        /// 倒序  (TENET o((>ω< ))o
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        private void DoPipelineReverse(XPipelineContext<THandler> context, Func<IPipelineHandler<THandler>, bool> callback)
        {
            if (context != null && context.Handler != null)
            {
                bool doNext = callback(context.Handler);
                if (doNext && context.Prev != null && context.Prev != context)
                {
                    DoPipelineReverse(context.Prev, callback);
                }
            }
        }
    }
}
