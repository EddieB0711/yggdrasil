namespace Yggdrasil.Abstractions.Eventing;

public interface IYggdrasilSubscriptionManager : IDisposable {
  void Add(IDisposable subscription);
}