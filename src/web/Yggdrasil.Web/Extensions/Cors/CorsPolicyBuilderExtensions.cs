namespace Yggdrasil.Web.Extensions.Cors;

using Microsoft.AspNetCore.Cors.Infrastructure;

public static class CorsPolicyBuilderExtensions {
  public static void Default(this CorsPolicyBuilder builder) {
    builder.AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials()
           .SetIsOriginAllowed(_ => true)
           .WithExposedHeaders(
             "Grpc-Status",
             "Grpc-Message",
             "Grpc-Encoding",
             "Grpc-Accept-Encoding");
  }
}