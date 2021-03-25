﻿using System;
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

        

        public XPipelineContext<THandler> AddFirst(THandler handler)
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

        public XPipelineContext<THandler> AddLast(THandler handler) 
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


        #region Sync Start

        /// <summary>
        /// Start Pipeline
        /// </summary>
        /// <param name="callback"></param>
        public void Start(Func<THandler, bool> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (_head != null)
                DoPipeline(_head, callback);
        }

        private void DoPipeline(XPipelineContext<THandler> context, Func<THandler, bool> callback)
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

        #endregion

        #region Sync Start with next param
        public void Start(Func<THandler, THandler, bool> callback) //第二个THandler参数是指代上下文中的下一个
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (_head != null)
                DoPipeline(_head, callback);
        }

        private void DoPipeline(XPipelineContext<THandler> context, Func<THandler, THandler, bool> callback)
        {
            if (context != null && context.Handler != null)
            {
                bool doNext = callback(context.Handler, context.Next == null ? default : context.Next.Handler);
                if (doNext && context.Next != null && context.Next != context)
                {
                    DoPipeline(context.Next, callback);
                }
            }
        }
        #endregion

        #region Async Start

        public Task StartAsync(Func<THandler, Task<bool>> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (_head != null)
                return DoPipelineAsync(_head, callback);
            else
                return Task.CompletedTask;
        }

        private async Task DoPipelineAsync(XPipelineContext<THandler> context, Func<THandler, Task<bool>> callback)
        {
            if (context != null && context.Handler != null)
            {
                bool doNext = await callback(context.Handler);
                if (doNext && context.Next != null && context.Next != context)
                {
                    await DoPipelineAsync(context.Next, callback);
                }
            }
        }

        #endregion

        #region Async Start with nedxt param
        public Task StartAsync(Func<THandler, THandler, Task<bool>> callback) //第二个THandler参数是指代上下文中的下一个
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (_head != null)
                return DoPipelineAsync(_head, callback);
            else
                return Task.CompletedTask;
        }

        private async Task DoPipelineAsync(XPipelineContext<THandler> context, Func<THandler, THandler, Task<bool>> callback)
        {
            if (context != null && context.Handler != null)
            {
                bool doNext = await callback(context.Handler, context.Next == null ? default : context.Next.Handler);
                if (doNext && context.Next != null && context.Next != context)
                {
                    await DoPipelineAsync(context.Next, callback);
                }
            }
        }
        #endregion




        /// <summary>
        /// Start Pipeline (Reverse
        /// </summary>
        /// <param name="callback"></param>
        public void StartReverse(Func<THandler, bool> callback)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (_last != null)
                DoPipelineReverse(_last, callback);
        }


        /// <summary>
        /// Start Pipeline (Reverse
        /// </summary>
        /// <param name="callback"></param>
        public void StartReverse(Func<THandler, THandler, bool> callback) //第二个THandler参数是指代上下文中的上一个
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            if (_last != null)
                DoPipelineReverse(_last, callback);
        }

        


        

        

        

        /// <summary>
        /// 倒序  (TENET o((>ω< ))o
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        private void DoPipelineReverse(XPipelineContext<THandler> context, Func<THandler, bool> callback)
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

        /// <summary>
        /// 倒序  (TENET o((>ω< ))o
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callback"></param>
        private void DoPipelineReverse(XPipelineContext<THandler> context, Func<THandler, THandler, bool> callback)
        {
            if (context != null && context.Handler != null)
            {
                bool doNext = callback(context.Handler,
                    context.Prev == null ? default : context.Prev.Handler);
                if (doNext && context.Prev != null && context.Prev != context)
                {
                    DoPipelineReverse(context.Prev, callback);
                }
            }
        }
    }
}
