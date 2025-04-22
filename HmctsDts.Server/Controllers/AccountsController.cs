using HmctsDts.Server.DTOs;
using HmctsDts.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HmctsDts.Server.Controllers;

public class AccountsController(IAccountsService accountsService, ILogger<AccountsController> logger)
    : BaseApiController
{
    [HttpPost("register-new-caseworker")]
    public async Task<IActionResult> RegisterNewCaseWorker(RegisterUserDto registerUserDto)
    {
        var status = await accountsService.RegisterNewCaseWorker(registerUserDto);

        if (!status)
        {
            return Conflict(new { message = "Registration failed. Please check you information and try again." });
        }

        return Created();
    }
}