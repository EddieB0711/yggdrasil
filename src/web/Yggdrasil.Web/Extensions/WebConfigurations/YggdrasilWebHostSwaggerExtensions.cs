namespace Yggdrasil;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Yggdrasil.Host.Abstractions;
using Yggdrasil.Web;

public static class YggdrasilWebHostSwaggerExtensions {
  public static T UseSwagger<T>(this T host, string title, string version) where T : IYggdrasilRunnableHost<T, WebApplicationBuilder, WebApplication> {
    host.ConfigureBuilder((builder, c) => {
                            builder.Services.AddEndpointsApiExplorer();
                            builder.Services.AddSwaggerGen(c => c.SwaggerDoc(version, new() { Title = title, Version = version }));
                          },
                          MiddlewarePriority.Swagger)
        .ConfigureHost((app, c) => {
          if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{title} {version}"));
          }
        }, MiddlewarePriority.Swagger);

    return host;
  }
}