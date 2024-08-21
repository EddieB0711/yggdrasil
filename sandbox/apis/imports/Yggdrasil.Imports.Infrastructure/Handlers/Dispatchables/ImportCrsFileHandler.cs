namespace Yggdrasil.Imports.Infrastructure.Handlers.Dispatchables;

using Microsoft.EntityFrameworkCore;

using Yggdrasil.Abstractions;
using Yggdrasil.Dispatch.Abstractions;
using Yggdrasil.Imports.Domain.Dispatchables;
using Yggdrasil.Imports.Domain.Entities;

/// <summary>
///   The handler that delegates importation of a CRS file.
/// </summary>
public class ImportCrsFileHandler : IYggdrasilDispatchableHandler<ImportCrsFile, Result<CrsImportEntity, string>> {
  readonly DbContext context_;

  /// <summary>
  ///   ImportCrsFileHandler constructor.
  /// </summary>
  /// <param name="context">The database connection.</param>
  public ImportCrsFileHandler(DbContext context) {
    context_ = context;
  }

  /// <summary>
  ///   Handles the delegation of importing a CRS file.
  /// </summary>
  /// <param name="dispatchable">The object containing the required information to import a CRS file.</param>
  /// <param name="token">The cancellation token to use when cancellation of a task is required.</param>
  /// <returns>The result of the operation.</returns>
  public Result<CrsImportEntity, string> Handle(ImportCrsFile dispatchable, CancellationToken token = default) {
    var importEntity = new CrsImportEntity {
      Filename     = dispatchable.Filename,
      ImportStatus = ImportStatus.Imported,
      ImportDate   = DateTime.UtcNow,
    };

    context_.Set<CrsImportEntity>().Add(importEntity);
    context_.SaveChanges();

    return Result.Ok(importEntity);
  }
}