namespace Yggdrasil.Imports.Domain.Dispatchables;

using Yggdrasil.Abstractions;
using Yggdrasil.Dispatch.Abstractions;
using Yggdrasil.Imports.Domain.Entities;

public record GetCrsImport(string Filename) : IYggdrasilDispatchable<Result<CrsImportEntity, string>>;