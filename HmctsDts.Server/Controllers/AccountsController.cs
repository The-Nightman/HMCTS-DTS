using HmctsDts.Server.DTOs;
using HmctsDts.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HmctsDts.Server.Controllers;

public class AccountsController(IAccountsService accountsService, ILogger<AccountsController> logger)
    : BaseApiController
{
    [HttpPost("register-new-caseworker")]
    public async Task<ActionResult> RegisterNewCaseWorker(RegisterUserDto registerUserDto)
    {
        var status = await accountsService.RegisterNewCaseWorker(registerUserDto);

        if (!status)
        {
            return Conflict(new { message = "Registration failed. Please check you information and try again." });
        }

        return Created();
    }

    [HttpPost("login")]
    public async Task<ActionResult<StaffDataDto>> Login(LoginDto loginDto)
    {
        var staffData = await accountsService.Login(loginDto);

        if (staffData == null)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }

        return Ok(staffData);
    }
}