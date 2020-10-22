using System;
using System.Collections.Generic;

public interface IZEvent
{

}

public class NonParams : IZEvent
{
    public Action actions;
    public NonParams(Action action)
    {
        actions += action;
    }
}
public class OneParams<T> : IZEvent
{
    public Action<T> actions;
    public OneParams(Action<T> action)
    {
        actions += action;
    }
}

public class TwoParams<T, H> : IZEvent
{
    public Action<T, H> actions;
    public TwoParams(Action<T, H> action)
    {
        actions += action;
    }
}

/// <summary>  </summary>
public class ZEvent : Singleton<ZEvent>
{
    private Dictionary<string, IZEvent> dicNameEvent = new Dictionary<string, IZEvent>();

    public void Bind(string name, Action act)
    {
        IZEvent iz = null;
        if (dicNameEvent.TryGetValue(name, out iz))
        {
            (dicNameEvent[name] as NonParams).actions += act;
        }
        else
        {
            dicNameEvent[name] = new NonParams(act);
        }
    }
    public void UnBind(string name, Action act)
    {
        IZEvent iz = null;
        if (dicNameEvent.TryGetValue(name, out iz))
        {
            (dicNameEvent[name] as NonParams).actions -= act;
        }
    }
    public void Fire(string name)
    {
        IZEvent iz = null;
        if (dicNameEvent.TryGetValue(name, out iz))
        {
            (dicNameEvent[name] as NonParams)?.actions?.Invoke();
        }
    }

    public void Bind<T>(string name, Action<T> act)
    {
        IZEvent iz = null;
        if (dicNameEvent.TryGetValue(name, out iz))
        {
            (dicNameEvent[name] as OneParams<T>).actions += act;
        }
        else
        {
            dicNameEvent[name] = new OneParams<T>(act);
        }

    }
    public void UnBind<T>(string name, Action<T> act)
    {
        IZEvent iz = null;
        if (dicNameEvent.TryGetValue(name, out iz))
        {
            (dicNameEvent[name] as OneParams<T>).actions -= act;
        }
    }
    public void Fire<T>(string name, T t)
    {
        IZEvent iz = null;
        if (dicNameEvent.TryGetValue(name, out iz))
        {
            (dicNameEvent[name] as OneParams<T>)?.actions?.Invoke(t);
        }
    }

    public void Bind<T, H>(string name, Action<T, H> act)
    {
        IZEvent iz = null;
        if (dicNameEvent.TryGetValue(name, out iz))
        {
            (dicNameEvent[name] as TwoParams<T, H>).actions += act;
        }
        else
        {
            dicNameEvent[name] = new TwoParams<T, H>(act);
        }
    }
    public void UnBind<T, H>(string name, Action<T, H> act)
    {
        IZEvent iz = null;
        if (dicNameEvent.TryGetValue(name, out iz))
        {
            (dicNameEvent[name] as TwoParams<T, H>).actions -= act;
        }
    }
    public void Fire<T, H>(string name, T t, H h)
    {
        IZEvent iz = null;
        if (dicNameEvent.TryGetValue(name, out iz))
        {
            (dicNameEvent[name] as TwoParams<T,H>)?.actions?.Invoke(t,h);
        }
    }

    public void Clear()
    {
        dicNameEvent.Clear();
    }

}
