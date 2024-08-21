namespace Yggdrasil.Imports.Infrastructure.Handlers.Dispatchables;

using Microsoft.EntityFrameworkCore;

using Yggdrasil.Abstractions;
using Yggdrasil.Dispatch.Abstractions;
using Yggdrasil.Imports.Domain.Dispatchables;
using Yggdrasil.Imports.Domain.Entities;

/// <summary>
///   Queries for a CrsImportEntity based on the filename.
/// </summary>
public class GetCrsImportHandler : IYggdrasilDispatchableHandler<GetCrsImport, Result<CrsImportEntity, string>> {
  readonly DbContext context_;

  /// <summary>
  ///   GetCrsImportHandler constructor.
  /// </summary>
  /// <param name="context">The database connection.</param>
  public GetCrsImportHandler(DbContext context) {
    context_ = context;
  }

  /// <summary>
  ///   Handles the query logic to retrieve a CRS import record.
  /// </summary>
  /// <param name="dispatchable">The object containing the required information to query a CRS import record.</param>
  /// <param name="token">The cancellation token to use when cancellation of a task is required.</param>
  /// <returns>The result of the operation.</returns>
  public Result<CrsImportEntity, string> Handle(GetCrsImport dispatchable, CancellationToken token = default) {
    var import = context_.Set<CrsImportEntity>().FirstOrDefault(x => x.Filename == dispatchable.Filename);

    if (import == null) {
      return Result.Error($"Record not found for file: {dispatchable.Filename}");
    }

    return Result.Ok(import);
  }
}