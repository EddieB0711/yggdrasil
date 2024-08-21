namespace Yggdrasil.Abstractions;

public abstract class YggdrasilDisposable : IAsyncDisposable, IDisposable {
  bool _disposed;

  public ValueTask DisposeAsync() {
    if (_disposed) {
      throw new ObjectDisposedException(GetType().FullName);
    }

    return DisposeAsync(true);
  }

  public void Dispose() {
    if (_disposed) {
      throw new ObjectDisposedException(GetType().FullName);
    }

    DisposeAsync(true).ConfigureAwait(false).GetAwaiter().GetResult();
  }

  protected virtual ValueTask OnDisposeAsync() {
    return new();
  }

  async ValueTask DisposeAsync(bool disposing) {
    if (!disposing || _disposed) {
      return;
    }

    await OnDisposeAsync().ConfigureAwait(false);

    _disposed = true;

    GC.SuppressFinalize(this);
  }
}