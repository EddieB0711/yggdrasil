namespace Yggdrasil.Vehicles.Domain.Dispatchables;

using Yggdrasil.Dispatch.Abstractions;

public record AddTrims(IEnumerable<TrimEntity> Trims) : IYggdrasilDispatchable<ValueTask<bool>>;