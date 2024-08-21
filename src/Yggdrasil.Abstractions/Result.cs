namespace Yggdrasil.Abstractions;

public readonly struct Result<TOk, TError> {
  readonly TError? _errorValue;
  readonly bool _hasValue;
  readonly TOk? _okValue;

  Result(TOk value) {
    _okValue    = value;
    _errorValue = default;
    _hasValue   = true;
  }

  Result(TError value) {
    _errorValue = value;
    _okValue    = default;
    _hasValue   = false;
  }

  public static implicit operator Result<TOk, TError>(Result.ResultError<TError> error) {
    return new(error.Error);
  }

  public static implicit operator Result<TOk, TError>(Result.ResultOk<TOk> ok) {
    return new(ok.Ok);
  }

  public Task Match(Func<TOk, Task> ok, Func<TError, Task> error) {
    return _hasValue ? ok(_okValue) : error(_errorValue);
  }

  public void Match(Action<TOk> ok, Action<TError> error) {
    if (_hasValue) {
      ok(_okValue);
    } else {
      error(_errorValue);
    }
  }

  public T Match<T>(Func<TOk, T> ok, Func<TError, T> error) {
    return _hasValue ? ok(_okValue) : error(_errorValue);
  }
}

public static class Result {
  public static ResultError<T> Error<T>(T error) {
    return new(error);
  }

  public static ResultOk<T> Ok<T>(T ok) {
    return new(ok);
  }

  public record ResultError<T>(T Error);

  public record ResultOk<T>(T Ok);
}