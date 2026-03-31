using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly ICurrentUserService _currentUserService;

    public WalletController(IWalletService walletService, ICurrentUserService currentUserService)
    {
        _walletService = walletService;
        _currentUserService = currentUserService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WalletDto>> GetWalletById(Guid id)
    {
        var wallet = await _walletService.GetWalletByIdAsync(id);
        return Ok(wallet);
    }
}