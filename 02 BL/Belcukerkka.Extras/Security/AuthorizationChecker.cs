using Belcukerkka.Models.Entities;
using Belcukerkka.Models.ViewModels;
using Belcukerkka.Repositories.Interfaces;
using Belcukerkka.Security;
using System.Linq;

namespace Belcukerkka.Services.Security
{
    public sealed class AuthorizationChecker
    {
        public AuthorizationChecker(IEntityRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        private readonly IEntityRepository<User> _userRepository;

        /// <summary>
        /// Checks whether Username and Password of specified LoginViewModel are valid.
        /// </summary>
        /// <param name="loginModel">Login View Model (Username, Password).</param>
        /// <returns>True, if existing user is found by specified username and its password is correct; otherwise, false.</returns>
        public bool CheckCredentials(LoginViewModel loginModel)
        {
            User user = _userRepository.GetAll()
                .Where(u => u.UserName == loginModel.LoginName)
                .FirstOrDefault();

            if (user == null)
                return false;

            var password = PasswordHandler.HashPassword(loginModel.Password);

            if (password != user.Password)
                return false;

            return true;
        }
    }
}
