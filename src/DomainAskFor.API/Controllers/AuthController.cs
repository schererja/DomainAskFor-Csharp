using DomainAskFor.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DomainAskFor.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class AuthController : ControllerBase
  {
    // In-memory storage for demo purposes
    // In production, use a proper database
    private static List<UserModel> _users = new List<UserModel>();

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
      try
      {
        // Check if user already exists
        if (_users.Any(u => u.Email.ToLower() == request.Email.ToLower()))
        {
          return Ok(new AuthResponse
          {
            Success = false,
            ErrorMessage = "User with this email already exists"
          });
        }

        // Create new user
        var user = new UserModel
        {
          Id = _users.Count + 1,
          Email = request.Email,
          PasswordHash = HashPassword(request.Password),
          FirstName = request.FirstName,
          LastName = request.LastName,
          CreatedAt = DateTime.UtcNow
        };

        _users.Add(user);

        // Generate token (simplified - in production use JWT)
        var token = GenerateToken(user);

        return Ok(new AuthResponse
        {
          Success = true,
          Token = token,
          Email = user.Email
        });
      }
      catch (Exception ex)
      {
        return Ok(new AuthResponse
        {
          Success = false,
          ErrorMessage = ex.Message
        });
      }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
      try
      {
        var user = _users.FirstOrDefault(u => u.Email.ToLower() == request.Email.ToLower());
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
        {
          return Ok(new AuthResponse
          {
            Success = false,
            ErrorMessage = "Invalid email or password"
          });
        }

        user.LastLoginAt = DateTime.UtcNow;
        var token = GenerateToken(user);

        return Ok(new AuthResponse
        {
          Success = true,
          Token = token,
          Email = user.Email
        });
      }
      catch (Exception ex)
      {
        return Ok(new AuthResponse
        {
          Success = false,
          ErrorMessage = ex.Message
        });
      }
    }

    private string HashPassword(string password)
    {
      using (var sha256 = SHA256.Create())
      {
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
      }
    }

    private bool VerifyPassword(string password, string hash)
    {
      return HashPassword(password) == hash;
    }

    private string GenerateToken(UserModel user)
    {
      // Simplified token generation - in production use JWT
      return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user.Id}:{user.Email}:{DateTime.UtcNow.Ticks}"));
    }
  }
}
