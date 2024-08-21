namespace Yggdrasil.Vehicles.Api.Controllers;

using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Yggdrasil.Dispatch.Abstractions;
using Yggdrasil.Vehicles.Domain;
using Yggdrasil.Vehicles.Domain.Dispatchables;

using Vehicles = global::Vehicles;

public class VehiclesService : Vehicles.VehiclesBase {
  readonly IYggdrasilDispatch _dispatch;

  public VehiclesService(IYggdrasilDispatch dispatch) {
    _dispatch = dispatch;
  }

  public override async Task<Empty> AddTrims(AddTrimsRequest request, ServerCallContext context) {
    var trims = request.Trims.Select(x => new TrimEntity {
      MakeId            = x.MakeId,
      MakeName          = x.MakeName,
      ModelId           = x.ModelId,
      ModelName         = x.ModelName,
      ModelYear         = (short)x.ModelYear,
      TrimId            = x.TrimId,
      TrimName          = x.TrimName,
      ProductTypeCode   = x.ProductTypeCode,
      TrimDisplayName   = x.TrimDisplayName,
      TrimPhotoFileHash = x.TrimPhotoFileHash.ToByteArray(),
      TrimPhotoFilePath = x.TrimPhotoFilePath,
      TrimMSRP          = (decimal)x.TrimMsrp,
    }).ToList();

    await _dispatch.Dispatch(new AddTrims(trims));

    return new();
  }

  public override Task<Empty> AddVinPatterns(AddVinPatternsRequest request, ServerCallContext context) {
    return base.AddVinPatterns(request, context);
  }
}