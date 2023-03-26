using System;
using System.Collections.Generic;

namespace Unity.FPS.Game
{
    public class GameEvent
    {
    }



    // A simple Event System that can be used for remote systems communication
    public static class EventManager
    {


        static readonly Dictionary<Type, Action<GameEvent>> s_Events = new Dictionary<Type, Action<GameEvent>>();

        static readonly Dictionary<Delegate, Action<GameEvent>> s_EventLookups =
            new Dictionary<Delegate, Action<GameEvent>>();

        public static void AddListener<T>(Action<T> evt) where T : GameEvent
        {
            if (!s_EventLookups.ContainsKey(evt))
            {
                Action<GameEvent> newAction = (e) => evt((T)e);
                s_EventLookups[evt] = newAction;

                if (s_Events.TryGetValue(typeof(T), out Action<GameEvent> internalAction))
                    s_Events[typeof(T)] = internalAction += newAction;
                else
                    s_Events[typeof(T)] = newAction;
            }
        }

        public static void RemoveListener<T>(Action<T> evt) where T : GameEvent
        {
            if (s_EventLookups.TryGetValue(evt, out var action))
            {
                if (s_Events.TryGetValue(typeof(T), out var tempAction))
                {
                    tempAction -= action;
                    if (tempAction == null)
                        s_Events.Remove(typeof(T));
                    else
                        s_Events[typeof(T)] = tempAction;
                }

                s_EventLookups.Remove(evt);
            }
        }

        public static void Broadcast(GameEvent evt)
        {
            if (s_Events.TryGetValue(evt.GetType(), out var action))
                action.Invoke(evt);
        }

        public static void Clear()
        {
            s_Events.Clear();
            s_EventLookups.Clear();
        }
    }

    public delegate R ReturnableAction<R, V>(V v);

    public class GameEventReturnData
    {
    }

    public static class ReturnableEventManager
    {


        static readonly Dictionary<Type, ReturnableAction<GameEventReturnData, GameEvent>> s_Events = new Dictionary<Type, ReturnableAction<GameEventReturnData, GameEvent>>();

        static readonly Dictionary<Delegate, ReturnableAction<GameEventReturnData, GameEvent>> s_EventLookups =
            new Dictionary<Delegate, ReturnableAction<GameEventReturnData, GameEvent>>();

        public static void AddListener<R, V>(ReturnableAction<R, V> evt) where R : GameEventReturnData
        where V : GameEvent
        {
            if (!s_EventLookups.ContainsKey(evt))
            {
                ReturnableAction<GameEventReturnData, GameEvent> newAction = (e) => evt((V)e);
                s_EventLookups[evt] = newAction;

                if (s_Events.TryGetValue(typeof(V), out ReturnableAction<GameEventReturnData, GameEvent> internalAction))
                    s_Events[typeof(V)] = internalAction += newAction;
                else
                    s_Events[typeof(V)] = newAction;
            }
        }

        public static void RemoveListener<R, V>(ReturnableAction<R, V> evt) where R : GameEventReturnData
        where V : GameEvent
        {
            if (s_EventLookups.TryGetValue(evt, out var action))
            {
                if (s_Events.TryGetValue(typeof(V), out var tempAction))
                {
                    tempAction -= action;
                    if (tempAction == null)
                        s_Events.Remove(typeof(V));
                    else
                        s_Events[typeof(V)] = tempAction;
                }

                s_EventLookups.Remove(evt);
            }
        }

        public static R Broadcast<R>(GameEvent evt) where R : GameEventReturnData
        {
            if (s_Events.TryGetValue(evt.GetType(), out var action))
                return (R)action.Invoke(evt);

            return null;
        }

        public static void Clear()
        {
            s_Events.Clear();
            s_EventLookups.Clear();
        }
    }
}