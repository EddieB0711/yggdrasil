namespace Yggdrasil.Web.Configurations;

using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;

public class SignalRConfigurations {
  public Action<IEndpointRouteBuilder> EndpointBuilder { get; set; }
  public Action<HubOptions> HubOptions { get; set; }
  public Action<ISignalRServerBuilder> ServerBuilder { get; set; }
}