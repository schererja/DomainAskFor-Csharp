using System;
using System.ComponentModel.DataAnnotations;

namespace DomainAskFor.Models
{
  public class UserModel
  {
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }
  }

  public class LoginRequest
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
  }

  public class RegisterRequest
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
  }

  public class AuthResponse
  {
    public bool Success { get; set; }
    public string Token { get; set; }
    public string Email { get; set; }
    public string ErrorMessage { get; set; }
  }
}
