using Application.Helpers;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using onion_architecture.Application.Common;
using onion_architecture.Application.Features.Auth;
using onion_architecture.Application.Features.Dto.UserDto;
using onion_architecture.Application.IService;
using onion_architecture.Application.Wrappers.Abstract;
using onion_architecture.Application.Wrappers.Concrete;
using onion_architecture.Domain.Entity;
using onion_architecture.Domain.Repositories;
using onion_architecture.Infrastructure.Exceptions;
using onion_architecture.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace onion_architecture.Application.Service
{
    public class AuthService : IAuthService, IRequest<IResponse>
    {
        private readonly JWTSettings _jwtSettings;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AuthService(IOptions<JWTSettings> jwtSettings, IUserRepository userRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _userRepository = userRepository;
        }
        public DataResponse<TokenDTO> Login(LoginDto dto)
        {
            try
            {
                var user = _userRepository.GetAll().Where(x => x.Email == dto.Email).SingleOrDefault();
                if (user == null)
                {
                    throw new ApiException(401, "Tài khoản không tồn tại");
                }
                var isPasswordValid = PasswordHelper.VerifyPassword(dto.PassWord, user.PassWord);
                if (!isPasswordValid)
                {
                    throw new ApiException(401, "Mật khẩu không chính xác");
                }
                else
                {
                    return new DataResponse<TokenDTO>(CreateToken(user), 200,"Success");
                }
                throw new ApiException(401, "Đăng nhập thất bại");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public TokenDTO CreateToken(User user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_jwtSettings.RefreshTokenExpiration);
            var securityKey = Encoding.ASCII.GetBytes(_jwtSettings.SecurityKey);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey),
                SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience[0],
                expires: accessTokenExpiration,
                 notBefore: DateTime.Now,
                 claims: GetClaims(user, _jwtSettings.Audience, user.Role),
                 signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDTO
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = (int)((DateTimeOffset)accessTokenExpiration).ToUnixTimeSeconds(),
                RefreshTokenExpiration = (int)((DateTimeOffset)refreshTokenExpiration).ToUnixTimeSeconds()
            };

            return tokenDto;
        }


        private IEnumerable<Claim> GetClaims(User user, List<string> audiences, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                 new Claim(ClaimTypes.Role,role)
            };
            claims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
            return claims;
        }

        private string CreateRefreshToken()
        {
            var numberByte = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numberByte);
            return Convert.ToBase64String(numberByte);
        }
    }
}
