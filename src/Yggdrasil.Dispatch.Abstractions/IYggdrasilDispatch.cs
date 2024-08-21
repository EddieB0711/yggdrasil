namespace Yggdrasil.Dispatch.Abstractions;

/// <summary>
///   Interface for dispatching commands and events.
/// </summary>
public interface IYggdrasilDispatch {
  /// <summary>
  ///   The function used to dispatch commands and events.
  /// </summary>
  /// <param name="dispatchable">The command/event being dispatched.</param>
  /// <param name="token">The token used to cancel an operation.</param>
  /// <typeparam name="TResult">The result type of the operation.</typeparam>
  /// <returns>The result of the operation.</returns>
  TResult Dispatch<TResult>(IYggdrasilDispatchable<TResult> dispatchable, CancellationToken token = default);
}