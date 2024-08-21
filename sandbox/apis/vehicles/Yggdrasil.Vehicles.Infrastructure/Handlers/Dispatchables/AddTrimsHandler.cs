namespace Yggdrasil.Vehicles.Infrastructure.Handlers.Dispatchables;

using Microsoft.EntityFrameworkCore;

using Yggdrasil.Dispatch.Abstractions;
using Yggdrasil.Vehicles.Domain;
using Yggdrasil.Vehicles.Domain.Dispatchables;

public class AddTrimsHandler : IYggdrasilAsyncDispatchableHandler<AddTrims, bool> {
  readonly DbContext context_;

  public AddTrimsHandler(DbContext context) {
    context_ = context;
  }

  public async ValueTask<bool> Handle(AddTrims dispatchable, CancellationToken token = default) {
    context_.Set<TrimEntity>().AddRange(dispatchable.Trims);

    await context_.SaveChangesAsync(token);

    return true;
  }
}