using System;
using System.Collections.Generic;

public interface IEventBinding<T> where T : IEvent {
    public Action<T> OnEvent { get; set; }
    public Action OnEventNoArgs { get; set; }
}
public class EventBinding<T> : IEventBinding<T> where T : IEvent {
    public Action<T> OnEvent = delegate { };
    public Action OnEventNoArgs = delegate { };

    Action<T> IEventBinding<T>.OnEvent { get => OnEvent; set => OnEvent = value; }
    Action IEventBinding<T>.OnEventNoArgs { get => OnEventNoArgs; set => OnEventNoArgs = value; }


    public EventBinding(Action<T> @event) {
        OnEvent = @event;
    }
    public EventBinding(Action @event) {
        OnEventNoArgs = @event;
    }
}
public static class EventBus<T> where T : IEvent {


    static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

    public static void Register(IEventBinding<T> binding) { bindings.Add(binding); }
    public static void Deregister(IEventBinding<T> binding) { bindings.Remove(binding); }

    public static void Raise(T @event) {
        foreach (var item in bindings) {
            item?.OnEvent.Invoke(@event);
            item?.OnEventNoArgs.Invoke();
        }
    }
}


public class TestHeroEvent : IEvent {

    public int TestValue;
    public float TestValue2;
}

public class TestEnemyEvent :IEvent {
    public int Points;
}
