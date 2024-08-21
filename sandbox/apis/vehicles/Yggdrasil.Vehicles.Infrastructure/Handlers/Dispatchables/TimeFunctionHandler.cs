namespace Yggdrasil.Vehicles.Infrastructure.Handlers.Dispatchables;

using System.Diagnostics;
using System.Runtime.CompilerServices;

using Yggdrasil.Dispatch.Abstractions;
using Yggdrasil.Logging.Abstractions;

public class TimeFunctionHandler<TDispatchable, TResult> : IYggdrasilDispatchableHandler<TDispatchable, TResult> {
  readonly IYggdrasilDispatchableHandler<TDispatchable, TResult> handler_;
  readonly IYggdrasilLogger logger_ = YggdrasilLog.For<TimeFunctionHandler<TDispatchable, TResult>>();

  public TimeFunctionHandler(IYggdrasilDispatchableHandler<TDispatchable, TResult> handler) {
    handler_ = handler;
  }

  public TResult Handle(TDispatchable dispatchable, CancellationToken token = default) {
    var stopwatch = Stopwatch.StartNew();
    var result = handler_.Handle(dispatchable, token);

    stopwatch.Stop();

    var elapsed = stopwatch.Elapsed;

    logger_.Debug($"Handler: {handler_.GetType()} took {elapsed}ms to execute.");

    return result;
  }
}

public class TimeAsyncFunctionHandler<TDispatchable, TResult> : IYggdrasilAsyncDispatchableHandler<TDispatchable, TResult> {
  readonly IYggdrasilAsyncDispatchableHandler<TDispatchable, TResult> handler_;
  readonly IYggdrasilLogger logger_ = YggdrasilLog.For<TimeFunctionHandler<TDispatchable, TResult>>();

  public TimeAsyncFunctionHandler(IYggdrasilAsyncDispatchableHandler<TDispatchable, TResult> handler) {
    handler_ = handler;
  }

  public async ValueTask<TResult> Handle(TDispatchable dispatchable, CancellationToken token = default) {
    var stopwatch = Stopwatch.StartNew();
    var result = await handler_.Handle(dispatchable, token).ConfigureAwait(false);

    stopwatch.Stop();

    var elapsed = stopwatch.Elapsed;

    logger_.Debug($"Handler: {handler_.GetType()} took {elapsed}ms to execute.");

    return result;
  }
}

public class TimeStreamableFunctionHandler<TDispatchable, TResult> : IYggdrasilStreambleDispatchableHandler<TDispatchable, TResult> {
  readonly IYggdrasilStreambleDispatchableHandler<TDispatchable, TResult> handler_;
  readonly IYggdrasilLogger logger_ = YggdrasilLog.For<TimeFunctionHandler<TDispatchable, TResult>>();

  public TimeStreamableFunctionHandler(IYggdrasilStreambleDispatchableHandler<TDispatchable, TResult> handler) {
    handler_ = handler;
  }

  public async IAsyncEnumerable<TResult> Handle(TDispatchable dispatchable, [EnumeratorCancellation] CancellationToken token = default) {
    var stopwatch = Stopwatch.StartNew();
    var result = handler_.Handle(dispatchable, token);

    await foreach (var item in result.ConfigureAwait(false)) {
      yield return item;
    }

    stopwatch.Stop();

    var elapsed = stopwatch.Elapsed;

    logger_.Debug($"Handler: {handler_.GetType()} took {elapsed}ms to execute.");
  }
}