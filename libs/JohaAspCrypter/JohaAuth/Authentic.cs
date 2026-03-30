using JhCrypter.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JhCrypter;
namespace JohaAspCrypter.JohaAuth
{
    public static class Authentic
    {
        private static SymmetricSecurityKey _signKey
        {
            get
            {
                if (string.IsNullOrEmpty(CryptConfig.Option.SigningKey))
                {
                    throw new Exception("Signing key not found");
                }
                return new SymmetricSecurityKey(CryptConfig.Option.SigningKey.ToHash());
            }
        }
        private static SymmetricSecurityKey _ecnryptKey
        {
            get
            {
                if (string.IsNullOrEmpty(CryptConfig.Option.AuthEncryptingKey))
                {
                    throw new Exception("Encrypting key not found");
                }
                return new SymmetricSecurityKey(CryptConfig.Option.AuthEncryptingKey.ToHash());
            }
        }
        public static string GenerateToken(this List<Claim> claims, DateTime? expire = null)
        {
            expire = expire ?? DateTime.UtcNow.AddHours(1);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expire,
                SigningCredentials = new SigningCredentials(
                    _signKey,
                    SecurityAlgorithms.HmacSha256),
                EncryptingCredentials = new EncryptingCredentials(
                    _ecnryptKey,
                    SecurityAlgorithms.Aes256KW,        // key wrap algoritmi
                    SecurityAlgorithms.Aes128CbcHmacSha256) // content encryption
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
        public static IServiceCollection RegisterAuthService(this IServiceCollection service, JwtBearerEvents _event = null)
        {
            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                           .AddJwtBearer(options =>
                           {
                               options.TokenValidationParameters = new TokenValidationParameters
                               {
                                   ValidateIssuerSigningKey = true,
                                   IssuerSigningKey = _signKey,
                                   TokenDecryptionKey = _ecnryptKey,  // ✅ kerak
                                   ValidateIssuer = false,
                                   ValidateAudience = false,
                                   ClockSkew = TimeSpan.Zero
                               };
                               if (_event != null)
                                   options.Events = _event;
                           });
            service.AddAuthorization();
            return service;
        }
        public static WebApplication UseAuth(this WebApplication app)
        {
            app.UseAuthentication();  // 1-chi
            app.UseAuthorization();   // 2-chi
            return app;
        }
    }
}
