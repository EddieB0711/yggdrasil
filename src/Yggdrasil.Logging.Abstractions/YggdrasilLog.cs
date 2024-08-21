namespace Yggdrasil.Logging.Abstractions;

public static class YggdrasilLog {
  static Func<Type, IYggdrasilLogger>? s_LogFactory;

  public static void SetLogFactory(Func<Type, IYggdrasilLogger> logFactory) {
    s_LogFactory = logFactory;
  }

  public static IYggdrasilLogger For<T>() {
    return s_LogFactory == null ? null : s_LogFactory(typeof(T));
  }

  public static IYggdrasilLogger For(Type type) {
    return s_LogFactory == null ? null : s_LogFactory(type);
  }
}