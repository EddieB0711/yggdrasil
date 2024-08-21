namespace Yggdrasil.Dispatch;

using Lamar;

using Yggdrasil.Dispatch.Abstractions;
using Yggdrasil.Logging.Abstractions;

abstract class DispatchableWrapper<TResult> {
  public abstract TResult Handle(IYggdrasilDispatchable<TResult> dispatchable, IServiceContext container);
}

abstract class AsyncDispatchableWrapper<TResult> {
  public abstract ValueTask<TResult> HandleAsync(IYggdrasilDispatchable<TResult> dispatchable, IServiceContext container, CancellationToken token);

  public abstract IAsyncEnumerable<TResult> HandleStream(IYggdrasilDispatchable<TResult> dispatchable, IServiceContext container, CancellationToken token);
}

class DispatchableWrapper<TDispatchable, TResult> : DispatchableWrapper<TResult> where TDispatchable : IYggdrasilDispatchable<TResult> {
  readonly IYggdrasilLogger _logger = YggdrasilLog.For<DispatchableWrapper<TDispatchable, TResult>>();

  public override TResult Handle(IYggdrasilDispatchable<TResult> dispatchable, IServiceContext container) {
    try {
      var handler = container.GetInstance<IYggdrasilDispatchableHandler<TDispatchable, TResult>>();
      var result = handler.Handle((TDispatchable)dispatchable);

      return result;
    } catch (OperationCanceledException) {
      throw;
    } catch (Exception e) {
      _logger.Error(e, "Error handling dispatchable.");

      throw;
    }
  }
}

class AsyncDispatchableWrapper<TDispatchable, TResult> : AsyncDispatchableWrapper<TResult> {
  public override ValueTask<TResult> HandleAsync(IYggdrasilDispatchable<TResult> dispatchable, IServiceContext container, CancellationToken token) {
    var handler = container.GetInstance<IYggdrasilAsyncDispatchableHandler<TDispatchable, TResult>>();
    var result = handler.Handle((TDispatchable)dispatchable, token);

    return result;
  }

  public override IAsyncEnumerable<TResult> HandleStream(IYggdrasilDispatchable<TResult> dispatchable, IServiceContext container, CancellationToken token) {
    var handler = container.GetInstance<IYggdrasilStreambleDispatchableHandler<TDispatchable, TResult>>();
    var result = handler.Handle((TDispatchable)dispatchable, token);

    return result;
  }
}