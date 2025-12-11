using Core.DTOs.Auth;
using Core.Entities;
using Core.Enums;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService
{
    private readonly UserManager<User> userManager;
    private readonly TokenService tokenService;

    private readonly UnitOfWork unitOfWork;

    public AuthService(UserManager<User> userManager, TokenService tokenService, UnitOfWork _unitOfWork)
    {
        this.userManager = userManager;
        this.tokenService = tokenService;
        unitOfWork = _unitOfWork;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        try
        {
            // Check if user already exists
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "User with this email already exists"
                };
            }
        
            // Create new user
            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,

               // Gives default role
                RoleId = (await unitOfWork.Repository<Role>().FindAsync(r => r.Name == RoleType.Customer.ToString())).FirstOrDefault()!.Id
            };

            IdentityResult result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"Registration failed: {errors}"
                };
            }

            // Assign default role (optional - uncomment if using roles)
            // await userManager.AddToRoleAsync(user, RoleType.Customer.ToString());

            // Get user roles and generate token
            var token = tokenService.GenerateToken(user, new List <string> { user.Role.Name! });

            return new AuthResponseDto
            {
                Success = true,
                Message = "Registration successful",
                Token = token,
                User = new UserAuthDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role.Name
                }
            };
        }
        catch (Exception ex)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = $"An error occurred during registration: {ex.Message}"
            };
        }
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        try
        {
            // Find user by email
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            // Check password
            var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);
            if (!passwordValid)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            // Get user roles and generate token
            // var roles = await userManager.GetRolesAsync(user);
            var role = await unitOfWork.Repository<Role>().GetByIdAsync(user.RoleId);
            var token = tokenService.GenerateToken(user, new List <string> { role?.Name! });

            return new AuthResponseDto
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                User = new UserAuthDto
                {
                    Id = user.Id,
                    Email = user.Email!,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role.Name
                }
            };
        }
        catch (Exception ex)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = $"An error occurred during login: {ex.Message}"
            };
        }
    }
}
