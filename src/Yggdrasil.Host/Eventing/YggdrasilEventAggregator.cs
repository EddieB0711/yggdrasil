namespace Yggdrasil.Host.Eventing;

using System.Reactive.Linq;
using System.Reactive.Subjects;

using Yggdrasil.Abstractions;
using Yggdrasil.Abstractions.Eventing;

public class YggdrasilEventAggregator : YggdrasilDisposable, IYggdrasilEventAggregator {
  readonly Subject<object> m_Subject = new();

  public IObservable<TEvent> GetEvent<TEvent>() {
    return m_Subject.OfType<TEvent>().AsObservable();
  }

  public void Publish<TEvent>(TEvent e) {
    m_Subject.OnNext(e);
  }

  protected override ValueTask OnDisposeAsync() {
    m_Subject.Dispose();

    return new();
  }
}