namespace Yggdrasil.Abstractions.Eventing;

public interface IYggdrasilEventAggregator : IDisposable {
  IObservable<TEvent> GetEvent<TEvent>();

  void Publish<TEvent>(TEvent e);
}