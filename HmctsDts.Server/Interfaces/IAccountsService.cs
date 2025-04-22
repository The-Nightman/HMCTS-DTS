using HmctsDts.Server.DTOs;

namespace HmctsDts.Server.Interfaces;

public interface IAccountsService
{
    Task<bool> RegisterNewCaseWorker(RegisterUserDto registerUserDto);
}