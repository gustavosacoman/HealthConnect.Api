namespace HealthConnect.Application.Services;

using AutoMapper;
using HealthConnect.Application.Dtos.Auth;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Models;
using HealthConnect.Domain.Models.Roles;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/// <summary>
/// Provides handling of auth business rules for retrieval, creation, update, and deletion.
/// </summary>
public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IConfiguration configuration,
    IRoleRepository roleRepository)
    : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IConfiguration _configuration = configuration;
    private readonly IRoleRepository _roleRepository = roleRepository;

    /// <inheritdoc/>
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        if (request is null)
        {
            throw new NullReferenceException("Login request cannot be null.");
        }

        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            throw new ArgumentException("Email and password must be provided.");
        }

        var user = await _userRepository.GetUserByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.HashedPassword, user.Salt);

        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var userRoleNames = user.UserRoles.Select(ur => ur.Role.Name);

        Guid? profileId = null;

        if (userRoleNames.Any(name => name.Equals("doctor", StringComparison.OrdinalIgnoreCase)))
        {
            if (user.Doctor is null)
            {
                throw new InvalidOperationException($"User {user.Email} has 'doctor' role but no associated doctor profile found.");
            }

            profileId = user.Doctor.Id;
        }
        else if (user.Client is not null)
        {
            profileId = user.Client.Id;
        }

        var token = GenerateToken(user, user.UserRoles.Select(ur => ur.Role), profileId);

        return new LoginResponseDto
        {
            Token = token,
        };

    }

    private string GenerateToken(User user, IEnumerable<Role> roles, Guid? profileId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var keyString = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT key is not configured.");

        var key = Encoding.UTF8.GetBytes(keyString);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        if (profileId != Guid.Empty)
        {
            claims.Add(new Claim("profileId", profileId.ToString()));
        }

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),

            Expires = DateTime.UtcNow.AddHours(1),

            Issuer = _configuration["Jwt:Issuer"],

            Audience = _configuration["Jwt:Audience"],

            SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
