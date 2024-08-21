namespace Yggdrasil.Host.Eventing;

using Yggdrasil.Abstractions;
using Yggdrasil.Abstractions.Eventing;

public class YggdrasilSubscriptionManager : YggdrasilDisposable, IYggdrasilSubscriptionManager {
  readonly ICollection<IDisposable> m_Subscriptions = new List<IDisposable>();

  public void Add(IDisposable subscription) {
    m_Subscriptions.Add(subscription);
  }

  protected override ValueTask OnDisposeAsync() {
    foreach (var subscription in m_Subscriptions) {
      subscription.Dispose();
    }

    return new();
  }
}