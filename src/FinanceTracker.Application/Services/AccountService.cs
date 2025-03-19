using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinanceTracker.Application.Common;
using FinanceTracker.Domain.Dtos.Account;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace FinanceTracker.Application.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtOptions _jwtOptions;

    public AccountService(UserManager<User> userManager, JwtOptions jwtOptions)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions;
    }   
    
    public async Task<Result<string>> LoginAsync(LoginDto loginDto, CancellationToken ct)
    {
        var user = await _userManager.FindByNameAsync(loginDto.UserName);
        if (user is null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
        {
            return Result.Fail("Username or password is incorrect");
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.Email}")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(60),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return Result.Ok(tokenHandler.WriteToken(token));
    }

    public async Task<Result> RegisterAsync(RegisterDto registerDto, CancellationToken ct)
    {
        if (await _userManager.FindByNameAsync(registerDto.UserName) is not null)
        {
            return Result.Fail("Username is already taken");
        }
        if (await _userManager.FindByEmailAsync(registerDto.Email) is not null)
        {
            return Result.Fail("Email is already taken");
        }
        
        var user = new User
        {
            UserName = registerDto.UserName,
            Email = registerDto.Email,
        };

        if (!string.IsNullOrEmpty(registerDto.CurrencyCode))
        {
            user.CurrencyCode = registerDto.CurrencyCode;
        }

        var createUserResult = await _userManager.CreateAsync(user, registerDto.Password);
        if (!createUserResult.Succeeded)
        {
            var errors = createUserResult.Errors.Select(e => e.Description);
            return Result.Fail(errors);
        }
        
        return Result.Ok();
    }
}