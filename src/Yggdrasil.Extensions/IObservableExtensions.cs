namespace Yggdrasil.Extensions;

using System.Reactive.Linq;

public static class IObservableExtensions {
  public static IDisposable SubscribeAsync<T>(this IObservable<T> observable, Func<T, Task> next) {
    return observable.Select(x => Observable.FromAsync(() => next(x))).Merge().Subscribe();
  }

  public static IDisposable SubscribeAsync<T>(this IObservable<T> observable, Func<T, ValueTask> next) {
    return observable.Select(x => Observable.FromAsync(async () => await next(x).ConfigureAwait(false))).Merge().Subscribe();
  }
}