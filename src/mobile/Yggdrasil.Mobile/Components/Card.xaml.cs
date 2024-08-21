namespace Yggdrasil.Mobile.Components;

using System.Windows.Input;

public partial class Card : ContentView {
  public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(Card));

  public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(Card));

  public Card() {
    InitializeComponent();
  }

  public ICommand Command {
    get => (ICommand)GetValue(CommandProperty);
    set => SetValue(CommandProperty, value);
  }

  public object CommandParameter {
    get => GetValue(CommandParameterProperty);
    set => SetValue(CommandParameterProperty, value);
  }

  void Card_OnTapped(object sender, TappedEventArgs e) {
    if (Command == null) {
      return;
    }

    if (Command.CanExecute(CommandParameter)) {
      Command.Execute(CommandParameter);
    }
  }
}