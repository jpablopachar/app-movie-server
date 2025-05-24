using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using app_movie_server.Data;
using app_movie_server.Models;
using app_movie_server.Models.Dtos;
using app_movie_server.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace app_movie_server.Repositories
{
    /// <summary>
    /// Repositorio responsable de gestionar las operaciones relacionadas con los usuarios en la base de datos.
    /// Proporciona métodos para obtener, autenticar y registrar usuarios, así como verificar la unicidad de los mismos.
    /// </summary>
    /// <remarks>
    /// Esta clase implementa la interfaz <see cref="IUserRepository"/> y utiliza Entity Framework Core, Identity y AutoMapper
    /// para la gestión de usuarios y roles.
    /// </remarks>
    public class UserRepository(AppDbContext context, IConfiguration configuration, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper) : IUserRepository
    {
        private readonly AppDbContext _context = context;
        private readonly string _secretKey = configuration.GetValue<string>("ApiSettings:SecretKey")!;
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IMapper _mapper = mapper;

        public AppUser GetUser(string userId)
        {
            return _context.AppUsers.FirstOrDefault(au => au.Id == userId)!;
        }

        public ICollection<AppUser> GetUsers()
        {
            return [.. _context.AppUsers.OrderBy(au => au.UserName)];
        }

        public bool IsUserUnique(string userId)
        {
            var user = _context.AppUsers.FirstOrDefault(au => au.UserName == userId);

            return user == null;
        }

        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            var user = _context.AppUsers.FirstOrDefault(au => au.UserName!.ToLower() == loginDto.UserName!.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user!, loginDto.Password!);

            if (user == null || !isValid)
            {
                return new LoginResponseDto
                {
                    Token = "",
                    User = null,
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var handlerToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, user.UserName!.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault()!),
                ]),

                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handlerToken.CreateToken(tokenDescriptor);

            LoginResponseDto loginResponseDto = new()
            {
                Token = handlerToken.WriteToken(token),
                User = _mapper.Map<UserDataDto>(user),
            };

            return loginResponseDto;
        }

        public async Task<UserDataDto> Register(RegisterDto registerDto)
        {
            AppUser user = new()
            {
                UserName = registerDto.UserName,
                Email = registerDto.UserName,
                NormalizedEmail = registerDto.UserName!.ToUpper(),
                Name = registerDto.Name,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password!);

            if (result.Succeeded)
            {
                if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("Registered"));
                }

                await _userManager.AddToRoleAsync(user, "Admin");

                var returnedUser = _context.AppUsers.FirstOrDefault(au => au.UserName == registerDto.UserName);

                return _mapper.Map<UserDataDto>(returnedUser);
            }

            return new UserDataDto();
        }
    }
}