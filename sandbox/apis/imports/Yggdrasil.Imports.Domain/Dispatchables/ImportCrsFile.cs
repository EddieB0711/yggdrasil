﻿namespace Yggdrasil.Imports.Domain.Dispatchables;

using Yggdrasil.Abstractions;
using Yggdrasil.Dispatch.Abstractions;
using Yggdrasil.Imports.Domain.Entities;

public record ImportCrsFile(string Filename) : IYggdrasilDispatchable<Result<CrsImportEntity, string>>;