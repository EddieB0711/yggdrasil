namespace Yggdrasil.Dispatch;

using System.Collections.Concurrent;
using System.Reflection.Emit;

delegate object DispatchableHandlerConstructorDelegate();

static class DispatchableCache {
  static readonly ConcurrentDictionary<Type, object> Wrappers = new();

  public static object GetOrAdd<TResult>(Type dispatchableType) {
    return Wrappers.GetOrAdd(dispatchableType, _ => {
      var handlerType = typeof(DispatchableWrapper<,>).MakeGenericType(dispatchableType, typeof(TResult));
      var ctor = CreateWrapperCtor(handlerType);

      return ctor();
    });
  }

  static DispatchableHandlerConstructorDelegate CreateWrapperCtor(Type handlerType) {
    var ctor = handlerType.GetConstructor(Type.EmptyTypes);

    if (ctor == null) {
      throw new("Type does not have a default constructor.");
    }

    var method = new DynamicMethod($"{handlerType.Name}Ctor", handlerType, Type.EmptyTypes, typeof(YggdrasilDispatch).Module);
    var il = method.GetILGenerator();

    il.Emit(OpCodes.Newobj, ctor);
    il.Emit(OpCodes.Ret);

    return method.CreateDelegate<DispatchableHandlerConstructorDelegate>();
  }
}