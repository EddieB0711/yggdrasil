namespace Yggdrasil.Web;

public static class MiddlewarePriority {
  public const int Default = 1;
  public const int Routing = 5;
  public const int Cors = 6;
  public const int DefaultRouting = 7;
  public const int Authentication = 8;
  public const int ControllerMaps = 9;
  public const int SignalR = 9;
  public const int Swagger = 10;
}