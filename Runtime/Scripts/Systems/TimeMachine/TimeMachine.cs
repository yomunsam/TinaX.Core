using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using TinaX.Systems;

namespace TinaX
{
    public class TimeMachine
    {
        public struct ActionOrder
        {
            public Action action;
            public int order;

            public ActionOrder(Action _action, int _order)
            {
                action = _action;
                order = _order;
            }
        }

        #region Static

        private static TimeMachine _instance;
        internal static TimeMachine Instance { get { if (_instance == null) { _instance = new TimeMachine(); } return _instance; } }

        public static ITimeTicket RegisterUpdate(Action updateAction, int order = 0)
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("[TinaX] Timemachine can only be used when playing");
                return default;
            }
            return TimeMachine.Instance.RegisterUpdateAction(updateAction, order);
        }
        public static void RemoveUpdate(Action updateAction)
        {
            if (!Application.isPlaying) return;
            TimeMachine.Instance.RemoveUpdateAction(updateAction);
        }
        public static void RemoveUpdate(int action_id)
        {
            if (!Application.isPlaying) return;
            TimeMachine.Instance.RemoveUpdateAction(action_id);
        }


        public static ITimeTicket RegisterLateUpdate(Action lateupdateAction, int order = 0)
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("[TinaX] Timemachine can only be used when playing");
                return default;
            }
            return TimeMachine.Instance.RegisterLateUpdateAction(lateupdateAction, order);
        }
        public static void RemoveLateUpdate(Action lateupdateAction)
        {
            if (!Application.isPlaying) return;
            TimeMachine.Instance.RemoveLateUpdateAction(lateupdateAction);
        }
        public static void RemoveLateUpdate(int action_id)
        {
            if (!Application.isPlaying) return;
            TimeMachine.Instance.RemoveLateUpdateAction(action_id);
        }

        public static ITimeTicket RegisterFixedUpdate(Action fixedupdateAction, int order = 0)
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("[TinaX] Timemachine can only be used when playing");
                return default;
            }
            return TimeMachine.Instance.RegisterFixedUpdateAction(fixedupdateAction, order);
        }
        public static void RemoveFixedUpdate(Action fixedupdateAction)
        {
            if (!Application.isPlaying) return;
            TimeMachine.Instance.RemoveFixedUpdateAction(fixedupdateAction);
        }
        public static void RemoveFixedUpdate(int action_id)
        {
            if (!Application.isPlaying) return;
            TimeMachine.Instance.RemoveFixedUpdateAction(action_id);
        }

        #endregion


        private readonly List<int> mListUpdateOrder = new List<int>();
        private readonly Dictionary<int, Dictionary<int, Action>> mDict_Update_Order_HashID_Action = new Dictionary<int, Dictionary<int, Action>>();
        private readonly Dictionary<int, ActionOrder> mDict_Update_HashID_Action = new Dictionary<int, ActionOrder>();

        private readonly List<int> mList_LateUpdateOrder = new List<int>();
        private readonly Dictionary<int, Dictionary<int, Action>> mDict_LateUpdate_Order_HashID_Action = new Dictionary<int, Dictionary<int, Action>>();
        private readonly Dictionary<int, ActionOrder> mDict_LateUpdate_HashID_Action = new Dictionary<int, ActionOrder>();


        private readonly List<int> mList_FixedUpdateOrder = new List<int>();
        private readonly Dictionary<int, Dictionary<int, Action>> mDict_FixedUpdate_Order_HashID_Action = new Dictionary<int, Dictionary<int, Action>>();
        private readonly Dictionary<int, ActionOrder> mDict_FixedUpdate_HashID_Action = new Dictionary<int, ActionOrder>();


        private IDisposable mUpdate_Disposable;
        private IDisposable mLateUpdate_Disposable;
        private IDisposable mFixedUpdate_Disposable;

        public ITimeTicket RegisterUpdateAction(Action updateAction, int order = 0)
        {
            lock (this)
            {
                if (updateAction == null) return default;
                int hashCode = updateAction.GetHashCode();
                if (mDict_Update_HashID_Action.TryGetValue(hashCode, out var action))
                {
                    return new TimeTicket(hashCode, TimeTicket.TicketType.Update);
                }
                else
                {
                    mDict_Update_HashID_Action.Add(hashCode, new ActionOrder(updateAction, order));

                    //dict order
                    if (!mDict_Update_Order_HashID_Action.ContainsKey(order))
                        mDict_Update_Order_HashID_Action.Add(order, new Dictionary<int, Action>());
                    if (!mDict_Update_Order_HashID_Action[order].ContainsKey(hashCode))
                        mDict_Update_Order_HashID_Action[order].Add(hashCode, updateAction);
                    else
                        mDict_Update_Order_HashID_Action[order][hashCode] = updateAction;

                    //order list 
                    if (!mListUpdateOrder.Contains(order))
                    {
                        mListUpdateOrder.Add(order);
                        //排序
                        mListUpdateOrder.Sort();
                    }

                    if(mDict_Update_HashID_Action.Count > 0 && mUpdate_Disposable == null)
                    {
                        mUpdate_Disposable = Observable.EveryUpdate()
                            .Subscribe(_ => OnUpdate());
                    }

                    return new TimeTicket(hashCode, TimeTicket.TicketType.Update);
                }
            }
            
        }

        public void RemoveUpdateAction(Action updateAction)
        {
            if (updateAction == null) return;
            this.RemoveUpdateAction(updateAction.GetHashCode());
        }

        public void RemoveUpdateAction(int actionId)
        {
            lock (this)
            {
                if (mDict_Update_HashID_Action.TryGetValue(actionId, out var ao))
                {
                    if(mDict_Update_Order_HashID_Action.TryGetValue(ao.order,out var id_action))
                    {
                        id_action.Remove(actionId);
                        if(id_action.Count == 0)
                        {
                            mListUpdateOrder.Remove(ao.order);
                        }
                    }
                    mDict_Update_HashID_Action.Remove(actionId);
                }
                if(mDict_Update_HashID_Action.Count == 0)
                {
                    if(mUpdate_Disposable != null)
                    {
                        mUpdate_Disposable.Dispose();
                        mUpdate_Disposable = null;
                    }
                }
            }
        }


        public ITimeTicket RegisterLateUpdateAction(Action lateupdateAction, int order = 0)
        {
            lock (this)
            {
                if (lateupdateAction == null) return default;
                int hashCode = lateupdateAction.GetHashCode();
                if (mDict_LateUpdate_HashID_Action.TryGetValue(hashCode, out var action))
                {
                    return new TimeTicket(hashCode, TimeTicket.TicketType.LateUpdate);
                }
                else
                {
                    mDict_LateUpdate_HashID_Action.Add(hashCode, new ActionOrder(lateupdateAction, order));

                    //dict order
                    if (!mDict_LateUpdate_Order_HashID_Action.ContainsKey(order))
                        mDict_LateUpdate_Order_HashID_Action.Add(order, new Dictionary<int, Action>());
                    if (!mDict_LateUpdate_Order_HashID_Action[order].ContainsKey(hashCode))
                        mDict_LateUpdate_Order_HashID_Action[order].Add(hashCode, lateupdateAction);
                    else
                        mDict_LateUpdate_Order_HashID_Action[order][hashCode] = lateupdateAction;

                    //order list 
                    if (!mList_LateUpdateOrder.Contains(order))
                    {
                        mList_LateUpdateOrder.Add(order);
                        //排序
                        mList_LateUpdateOrder.Sort();
                    }

                    if (mDict_LateUpdate_HashID_Action.Count > 0 && mLateUpdate_Disposable == null)
                    {
                        mLateUpdate_Disposable = Observable.EveryLateUpdate()
                            .Subscribe(_ => OnLateUpdate());
                    }

                    return new TimeTicket(hashCode, TimeTicket.TicketType.LateUpdate);
                }
            }

        }

        public void RemoveLateUpdateAction(Action lateupdateAction)
        {
            if (lateupdateAction == null) return;
            this.RemoveLateUpdateAction(lateupdateAction.GetHashCode());
        }

        public void RemoveLateUpdateAction(int actionId)
        {
            lock (this)
            {
                if (mDict_LateUpdate_HashID_Action.TryGetValue(actionId, out var ao))
                {
                    if (mDict_LateUpdate_Order_HashID_Action.TryGetValue(ao.order, out var id_action))
                    {
                        id_action.Remove(actionId);
                        if (id_action.Count == 0)
                        {
                            mList_LateUpdateOrder.Remove(ao.order);
                        }
                    }
                    mDict_LateUpdate_HashID_Action.Remove(actionId);
                }

                if (mDict_LateUpdate_HashID_Action.Count == 0)
                {
                    if (mLateUpdate_Disposable != null)
                    {
                        mLateUpdate_Disposable.Dispose();
                        mLateUpdate_Disposable = null;
                    }
                }
            }
        }


        public ITimeTicket RegisterFixedUpdateAction(Action fixedupdateAction, int order = 0)
        {
            lock (this)
            {
                if (fixedupdateAction == null) return default;
                int hashCode = fixedupdateAction.GetHashCode();
                if (mDict_FixedUpdate_HashID_Action.TryGetValue(hashCode, out var action))
                {
                    return new TimeTicket(hashCode, TimeTicket.TicketType.FixedUpdate);
                }
                else
                {
                    mDict_FixedUpdate_HashID_Action.Add(hashCode, new ActionOrder(fixedupdateAction, order));

                    //dict order
                    if (!mDict_FixedUpdate_Order_HashID_Action.ContainsKey(order))
                        mDict_FixedUpdate_Order_HashID_Action.Add(order, new Dictionary<int, Action>());
                    if (!mDict_FixedUpdate_Order_HashID_Action[order].ContainsKey(hashCode))
                        mDict_FixedUpdate_Order_HashID_Action[order].Add(hashCode, fixedupdateAction);
                    else
                        mDict_FixedUpdate_Order_HashID_Action[order][hashCode] = fixedupdateAction;

                    //order list 
                    if (!mList_FixedUpdateOrder.Contains(order))
                    {
                        mList_FixedUpdateOrder.Add(order);
                        //排序
                        mList_FixedUpdateOrder.Sort();
                    }

                    if (mDict_FixedUpdate_HashID_Action.Count > 0 && mFixedUpdate_Disposable == null)
                    {
                        mFixedUpdate_Disposable = Observable.EveryFixedUpdate()
                            .Subscribe(_ => OnFixedUpdate());
                    }
                    return new TimeTicket(hashCode, TimeTicket.TicketType.FixedUpdate);
                }
            }

        }

        public void RemoveFixedUpdateAction(Action fixedupdateAction)
        {
            if (fixedupdateAction == null) return;
            this.RemoveFixedUpdateAction(fixedupdateAction.GetHashCode());
        }

        public void RemoveFixedUpdateAction(int actionId)
        {
            lock (this)
            {
                if (mDict_FixedUpdate_HashID_Action.TryGetValue(actionId, out var ao))
                {
                    if (mDict_FixedUpdate_Order_HashID_Action.TryGetValue(ao.order, out var id_action))
                    {
                        id_action.Remove(actionId);
                        if (id_action.Count == 0)
                        {
                            mList_FixedUpdateOrder.Remove(ao.order);
                        }
                    }
                    mDict_FixedUpdate_HashID_Action.Remove(actionId);
                }
                if (mDict_FixedUpdate_HashID_Action.Count == 0)
                {
                    if (mFixedUpdate_Disposable != null)
                    {
                        mFixedUpdate_Disposable.Dispose();
                        mFixedUpdate_Disposable = null;
                    }
                }
            }
        }



        private void OnUpdate()
        {
            //foreach(var order in mListUpdateOrder)
            //{
            //    foreach (var item in mDict_Update_Order_HashID_Action[order])
            //        item.Value?.Invoke();
            //}

            for (var i = mListUpdateOrder.Count - 1; i >= 0; i--) 
            {
                var item = mDict_Update_Order_HashID_Action[mListUpdateOrder[i]].GetEnumerator();
                while (item.MoveNext())
                {
                    item.Current.Value?.Invoke();
                }
            }
        }

        private void OnLateUpdate()
        {
            //foreach (var order in mList_LateUpdateOrder)
            //{
            //    foreach (var item in mDict_LateUpdate_Order_HashID_Action[order])
            //        item.Value?.Invoke();
            //}
            for (var i = mList_LateUpdateOrder.Count - 1; i >= 0; i--)
            {
                var item = mDict_LateUpdate_Order_HashID_Action[mList_LateUpdateOrder[i]].GetEnumerator();
                while (item.MoveNext())
                {
                    item.Current.Value?.Invoke();
                }
            }
        }

        private void OnFixedUpdate()
        {
            //foreach (var order in mList_FixedUpdateOrder)
            //{
            //    foreach (var item in mDict_FixedUpdate_Order_HashID_Action[order])
            //        item.Value?.Invoke();
            //}
            for(var i = mList_FixedUpdateOrder.Count -1; i >=0; i--)
            {
                var item = mDict_FixedUpdate_Order_HashID_Action[mList_FixedUpdateOrder[i]].GetEnumerator();
                while (item.MoveNext())
                {
                    item.Current.Value?.Invoke();
                }
            }
        }

    }

}
