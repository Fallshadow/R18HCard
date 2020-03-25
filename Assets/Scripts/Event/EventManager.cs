using System;

namespace act.evt
{
    public class EventManager : Singleton<EventManager>
    {

        public static int CombineId(short hi, short lo)
        {
            return hi << 16 | (ushort)lo;
        }

        private EventHandler eventHandler = new EventHandler();

        public void Register(EventGroup groupId, short eventId, Action callback)
        {
            eventHandler.Register(CombineId((short)groupId, eventId), callback);
        }

        public void Unregister(EventGroup groupId, short eventId, Action callback)
        {
            eventHandler.Unregister(CombineId((short)groupId, eventId), callback);
        }

        public void Send(EventGroup groupId, short eventId)
        {
            eventHandler.Send(CombineId((short)groupId, eventId));
        }

        public void Register<T>(EventGroup groupId, short eventId, Action<T> callback)
        {
            eventHandler.Register(CombineId((short)groupId, eventId), callback);
        }

        public void Unregister<T>(EventGroup groupId, short eventId, Action<T> callback)
        {
            eventHandler.Unregister(CombineId((short)groupId, eventId), callback);
        }

        public void Send<T>(EventGroup groupId, short eventId, T arg1)
        {
            eventHandler.Send(CombineId((short)groupId, eventId), arg1);
        }

        public void Register<T1, T2>(EventGroup groupId, short eventId, Action<T1, T2> callback)
        {
            eventHandler.Register(CombineId((short)groupId, eventId), callback);
        }

        public void Unregister<T1, T2>(EventGroup groupId, short eventId, Action<T1, T2> callback)
        {
            eventHandler.Unregister(CombineId((short)groupId, eventId), callback);
        }

        public void Send<T1, T2>(EventGroup groupId, short eventId, T1 arg1, T2 agr2)
        {
            eventHandler.Send(CombineId((short)groupId, eventId), arg1, agr2);
        }

        public void Register<T1, T2, T3>(EventGroup groupId, short eventId, Action<T1, T2, T3> callback)
        {
            eventHandler.Register(CombineId((short)groupId, eventId), callback);
        }

        public void Register<T1, T2, T3, T4>(EventGroup groupId, short eventId, Action<T1, T2, T3, T4> callback)
        {
            eventHandler.Register(CombineId((short)groupId, eventId), callback);
        }

        public void Register<T1, T2, T3, T4, T5>(EventGroup groupId, short eventId, Action<T1, T2, T3, T4, T5> callback)
        {
            eventHandler.Register(CombineId((short)groupId, eventId), callback);
        }

        public void Unregister<T1, T2, T3>(EventGroup groupId, short eventId, Action<T1, T2, T3> callback)
        {
            eventHandler.Unregister(CombineId((short)groupId, eventId), callback);
        }

        public void Unregister<T1, T2, T3, T4>(EventGroup groupId, short eventId, Action<T1, T2, T3, T4> callback)
        {
            eventHandler.Unregister(CombineId((short)groupId, eventId), callback);
        }

        public void Unregister<T1, T2, T3, T4, T5>(EventGroup groupId, short eventId, Action<T1, T2, T3, T4, T5> callback)
        {
            eventHandler.Unregister(CombineId((short)groupId, eventId), callback);
        }

        public void Send<T1, T2, T3>(EventGroup groupId, short eventId, T1 arg1, T2 agr2, T3 agr3)
        {
            eventHandler.Send(CombineId((short)groupId, eventId), arg1, agr2, agr3);
        }

        public void Send<T1, T2, T3, T4>(EventGroup groupId, short eventId, T1 arg1, T2 agr2, T3 agr3, T4 arg4)
        {
            eventHandler.Send(CombineId((short)groupId, eventId), arg1, agr2, agr3, arg4);
        }

        public void Send<T1, T2, T3, T4, T5>(EventGroup groupId, short eventId, T1 arg1, T2 agr2, T3 agr3, T4 arg4, T5 arg5)
        {
            eventHandler.Send(CombineId((short)groupId, eventId), arg1, agr2, agr3, arg4, arg5);
        }
    }
}