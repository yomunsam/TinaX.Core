/*
 * 参考来源：https://www.cnblogs.com/HelloMyWorld/p/5526914.html
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;

namespace TinaX.Utils
{
    public class TimeoutTask
    {
        public static async Task<AsyncCompletedEventArgs> Start(Action action, CancellationToken token)
        {
            return await TimeoutTask.Start(action, token, Timeout.Infinite);
        }

        public static async Task<AsyncCompletedEventArgs> Start(Action action, int timeout)
        {
            return await TimeoutTask.Start(action, CancellationToken.None, timeout);
        }

        public static async Task<AsyncCompletedEventArgs> Start(Action action, TimeSpan timeout)
        {
            return await TimeoutTask.Start(action, CancellationToken.None, (int)timeout.TotalMilliseconds);
        }

        public static async Task<AsyncCompletedEventArgs> Start(Action action, CancellationToken token, int timeout = Timeout.Infinite)
        {
            var task = new TimeoutTask(action, token, timeout);
            return await task.Run();
        }

        private Action mAction;
        private CancellationToken mToken;
        private event AsyncCompletedEventHandler mAsyncCompletedEvent;
        private TaskCompletionSource<AsyncCompletedEventArgs> mTaskCompletionSource;

        //protected TimeoutTask(Action action, TimeSpan timeout) : this(action, CancellationToken.None, (int)timeout.TotalMilliseconds) { }
        //protected TimeoutTask(Action action, CancellationToken token) : this(action, token, Timeout.Infinite) { }
        //protected TimeoutTask(Action action, int timeout) : this(action, CancellationToken.None, timeout) { }
        protected TimeoutTask(Action action, CancellationToken token, int timeout = Timeout.Infinite)
        {
            mAction = action;
            mTaskCompletionSource = new TaskCompletionSource<AsyncCompletedEventArgs>();
            if (timeout != Timeout.Infinite)
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                cts.CancelAfter(timeout);
                mToken = cts.Token;
            }
            else
                mToken = token;
        }

        private async Task<AsyncCompletedEventArgs> Run()
        {
            mAsyncCompletedEvent += this.AsyncCompletedEventHandler;
            try
            {
                using(mToken.Register(()=> mTaskCompletionSource.TrySetCanceled()))
                {
                    ExecuteAction();
                    return await mTaskCompletionSource.Task.ConfigureAwait(false);
                }
            }
            finally
            {
                mAsyncCompletedEvent -= this.AsyncCompletedEventHandler;
            }

        }

        /// <summary>
        /// 异步完成事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AsyncCompletedEventHandler(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                mTaskCompletionSource.TrySetCanceled();
            }
            else if (e.Error != null)
            {
                mTaskCompletionSource.TrySetException(e.Error);
            }
            else
            {
                mTaskCompletionSource.TrySetResult(e);
            }
        }

        /// <summary>
        /// 执行Action
        /// </summary>
        private void ExecuteAction()
        {
            Task.Factory.StartNew(() =>
            {
                mAction.Invoke();
                this.OnAsyncCompleteEvent(null);
            });
        }

        /// <summary>
        /// 触发异步完成事件
        /// </summary>
        /// <param name="userState"></param>
        private void OnAsyncCompleteEvent(object userState)
        {
            if (mAsyncCompletedEvent != null)
                mAsyncCompletedEvent(this, new AsyncCompletedEventArgs(error: null, cancelled: false, userState: userState));
        }

    }

    public class TimeoutTask<T>
    {
        
        private Func<T> mFunc;
        private CancellationToken mToken;
        private event AsyncCompletedEventHandler mAsyncCompletedEvent;
        private TaskCompletionSource<AsyncCompletedEventArgs> _tcs;


        
        public static async Task<T> Start(Func<T> func, CancellationToken token, int timeout = Timeout.Infinite)
        {
            var task = new TimeoutTask<T>(func, token, timeout);
            return await task.Run();
        }

        public static async Task<T> Start(Func<T> func, int timeout)
        {
            return await TimeoutTask<T>.Start(func, CancellationToken.None, timeout);
        }
        public static async Task<T> Start(Func<T> func, TimeSpan timeout)
        {
            return await TimeoutTask<T>.Start(func, CancellationToken.None, (int)timeout.TotalMilliseconds);
        }

        public static async Task<T> Start(Func<T> func, CancellationToken token)
        {
            return await TimeoutTask<T>.Start(func, token, Timeout.Infinite);
        }
        
        //protected TimeoutTask(Func<T> func, CancellationToken token) : this(func, token, Timeout.Infinite) { }
        //protected TimeoutTask(Func<T> func, int timeout = Timeout.Infinite) : this(func, CancellationToken.None, timeout) { }
        //protected TimeoutTask(Func<T> func, TimeSpan timeout) : this(func, CancellationToken.None, (int)timeout.TotalMilliseconds) { }

        protected TimeoutTask(Func<T> func, CancellationToken token, int timeout = Timeout.Infinite)
        {
            mFunc = func;

            _tcs = new TaskCompletionSource<AsyncCompletedEventArgs>();

            if (timeout != Timeout.Infinite)
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
                cts.CancelAfter(timeout);
                mToken = cts.Token;
            }
            else
            {
                mToken = token;
            }
        }

        #region 私有方法
        /// <summary>
        /// 运行Task
        /// </summary>
        /// <returns></returns>
        private async Task<T> Run()
        {
            mAsyncCompletedEvent += AsyncCompletedEventHandler;

            try
            {
                using (mToken.Register(() => _tcs.TrySetCanceled()))
                {
                    ExecuteFunc();
                    var args = await _tcs.Task.ConfigureAwait(false);
                    return (T)args.UserState;
                }

            }
            finally
            {
                mAsyncCompletedEvent -= AsyncCompletedEventHandler;
            }

        }

        /// <summary>
        /// 执行
        /// </summary>
        private void ExecuteFunc()
        {
            ThreadPool.QueueUserWorkItem(s =>
            {
                var result = mFunc.Invoke();

                OnAsyncCompleteEvent(result);
            });
        }

        /// <summary>
        /// 异步完成事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AsyncCompletedEventHandler(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _tcs.TrySetCanceled();
            }
            else if (e.Error != null)
            {
                _tcs.TrySetException(e.Error);
            }
            else
            {
                _tcs.TrySetResult(e);
            }
        }

        /// <summary>
        /// 触发异步完成事件
        /// </summary>
        /// <param name="userState"></param>
        private void OnAsyncCompleteEvent(object userState)
        {
            if (mAsyncCompletedEvent != null)
            {
                mAsyncCompletedEvent(this, new AsyncCompletedEventArgs(error: null, cancelled: false, userState: userState));
            }
        }
        #endregion
    }

}
