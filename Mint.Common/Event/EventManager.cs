using System.Reflection;

namespace Mint.Common.Event;

public class EventManager
{
    struct EventData
    {
        public object Invoker;
        public MethodBase Method;
        public ParameterInfo[] Parameters;
    }

    private static readonly Dictionary<Type, List<EventData>> _registeredMethods = new();

    public static void Call(IEvent _event)
    {
        foreach (List<EventData> methods in _registeredMethods.Values)
        {
            foreach (EventData method in methods)
            {
                foreach (ParameterInfo parameter in method.Parameters)
                {
                    Type? parameterType = parameter.ParameterType;
                    if (parameterType != null)
                    {
                        if (_event.GetType().IsAssignableFrom(parameterType))
                        {
                            method.Method.Invoke(method.Invoker, new object[] { _event });
                        }
                    }
                }
            }
        }
    }

    public static void RegisterListener(object obj)
    {        
        foreach (MethodBase method in obj.GetType().GetMethods())
        {
            Attribute? eventTarget = method.GetCustomAttribute(typeof(EventTarget));
            if (eventTarget != null)
            {
                ParameterInfo[] parameterInfo = method.GetParameters();
                foreach (ParameterInfo parameter in parameterInfo)
                {
                    Type? parameterType = parameter.ParameterType;
                    if (parameterType != null)
                    {
                        if (typeof(IEvent).IsAssignableFrom(parameterType))
                        {
                            if (!_registeredMethods.TryGetValue(obj.GetType(), out _))
                            {
                                List<EventData> data = new();
                                _registeredMethods.Add(obj.GetType(), data);
                            }
                            _registeredMethods[obj.GetType()].Add(new EventData { Invoker = obj, Method = method, Parameters = method.GetParameters() });

                            break;
                        }
                    }
                }
            }
        }
    }

    public static void UnregisterListener(Type type)
    {
        _registeredMethods.Remove(type);
    }
}
