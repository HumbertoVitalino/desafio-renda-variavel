using Core.Commons;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controller;

[ApiController]
[Route("api/v1/[controller]")]
public class AssetsController(
    IAssetRepository assetRepository
) : BaseController
{
    private readonly IAssetRepository _assetRepository = assetRepository;

    [HttpGet]
    [ProducesResponseType(typeof(Output), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetAllAssets(CancellationToken cancellationToken)
    {
        Output output = new();
        var assets = await _assetRepository.GetAllAssetsAsync(cancellationToken);
        output.AddResult(assets);
        return Ok(output);
    }

}
