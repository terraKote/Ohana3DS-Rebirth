namespace OhanaNext.Services
{
    public interface IEventService
    {
        void Subscribe<T>(Action<T> callback);
        void Unsubscribe<T>(Action<T> callback);
        void Trigger<T>(T evt);
        void Trigger(object evt);
    }
    
    public class EventService : IEventService
    {
        private readonly Dictionary<Type, List<Delegate>> _eventListeners = new();

        public void Subscribe<T>(Action<T> callback)
        {
            if (_eventListeners.TryGetValue(typeof(T), out var list))
            {
                list.Add(callback);
                return;
            }

            _eventListeners.Add(typeof(T), new List<Delegate> { callback });
        }

        public void Unsubscribe<T>(Action<T> callback)
        {
            if (!_eventListeners.TryGetValue(typeof(T), out var list))
                return;

            list.Remove(callback);
        }
        
        public void Trigger<T>(T evt)
        {
            if (!_eventListeners.TryGetValue(typeof(T), out var callbackList))
                return;

            var callbacks = callbackList.Cast<Action<T>>().ToArray();

            foreach (var action in callbacks)
            {
                action.Invoke(evt);
            }
        }
        
        public void Trigger(object evt)
        {
            if (evt == null)
                return;

            if (!_eventListeners.TryGetValue(evt.GetType(), out var callbackList))
                return;

            var callbacks = callbackList.ToArray();

            foreach (var callback in callbacks)
            {
                callback.DynamicInvoke(evt);
            }
        }
    }
}