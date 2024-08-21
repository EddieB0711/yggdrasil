namespace Yggdrasil.Dispatch;

using System.Reflection;

using Lamar;

using Yggdrasil.Dispatch.Abstractions;

public class YggdrasilDispatch : IYggdrasilDispatch {
  readonly IServiceContext _container;

  public YggdrasilDispatch(IServiceContext container) {
    _container = container;
  }

  public TResult Dispatch<TResult>(IYggdrasilDispatchable<TResult> dispatchable, CancellationToken token = default) {
    var resultType = typeof(TResult);

    if (resultType.IsSubclassOf(typeof(ValueTask)) || resultType.IsSubclassOf(typeof(Task))) {
      var innerType = resultType.GetGenericArguments()[0];
      var asyncWrapperType = typeof(AsyncDispatchableWrapper<>).MakeGenericType(innerType);
      var handleMethod = asyncWrapperType.GetMethod("HandleAsync", BindingFlags.Public | BindingFlags.Instance);
      var wrapper = Activator.CreateInstance(asyncWrapperType);

      return (TResult)handleMethod.Invoke(wrapper, new object[] { dispatchable, _container, token });
    }

    if (resultType.IsGenericType || resultType.GetGenericTypeDefinition() == typeof(IAsyncEnumerable<>)) {
      var innerType = resultType.GetGenericArguments()[0];
      var asyncWrapperType = typeof(AsyncDispatchableWrapper<>).MakeGenericType(innerType);
      var handleMethod = asyncWrapperType.GetMethod("HandleStream", BindingFlags.Public | BindingFlags.Instance);
      var wrapper = Activator.CreateInstance(asyncWrapperType);

      return (TResult)handleMethod.Invoke(wrapper, new object[] { dispatchable, _container, token });
    }

    var handler = (DispatchableWrapper<TResult>)DispatchableCache.GetOrAdd<TResult>(dispatchable.GetType());

    return handler.Handle(dispatchable, _container);
  }
}