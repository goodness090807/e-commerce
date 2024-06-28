using e_commerce.Common.Utils;
using e_commerce.data;
using e_commerce.service.Utils.TokenService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.service.Services.AuthorizationService
{
    public class AuthorizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public AuthorizationService(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public string Login(string account, string password)
        {
            // hash password
            password = HashHelper.HashPassword(password);

            var user = _context.Users.FirstOrDefault(x => x.Account == account && x.HashedPassword == password);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Generate token
            var token = _tokenService.GenerateJwtToken(user.Id.ToString());
            return token;
        }
    }
}
