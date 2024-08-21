namespace Yggdrasil.Dispatch.Abstractions;

/// <summary>
///   The interface used to handle dispatched commands/events.
/// </summary>
/// <typeparam name="TDispatchable">The command/event type.</typeparam>
/// <typeparam name="TResult">The result type of the operation.</typeparam>
public interface IYggdrasilDispatchableHandler<in TDispatchable, out TResult> {
  /// <summary>
  ///   The method used to perform the command/event operation.
  /// </summary>
  /// <param name="dispatchable">The command/event object.</param>
  /// <param name="token">The token used to cancel an operation.</param>
  /// <returns>The result of the operation.</returns>
  TResult Handle(TDispatchable dispatchable, CancellationToken token = default);
}

public interface IYggdrasilAsyncDispatchableHandler<in TDispatchable, TResult> {
  ValueTask<TResult> Handle(TDispatchable dispatchable, CancellationToken token = default);
}

public interface IYggdrasilStreambleDispatchableHandler<in TDispatchable, out TResult> {
  IAsyncEnumerable<TResult> Handle(TDispatchable dispatchable, CancellationToken token = default);
}