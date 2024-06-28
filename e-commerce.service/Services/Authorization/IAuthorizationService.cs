﻿using e_commerce.service.Services.Authorization.ViewModels;

namespace e_commerce.service.Services.Authorization
{
    public interface IAuthorizationService : IBaseService
    {
        Task<bool> CheckEmailAsync(string email);
        Task<AuthViewModel> Login(string email, string password);
        Task<AuthViewModel> Register(string email, string password, string name);
    }
}
