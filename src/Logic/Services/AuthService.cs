using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SaaSApi.Common;
using SaaSApi.Data;
using SaaSApi.Data.Entities;
using SaaSApi.Logic.Framework;
using SaaSApi.Logic.Models;

namespace SaaSApi.Logic.Services
{
    public interface IAuthService
    {
        UserDto Authenticate(LoginRequest request);
        UserDto Register(RegisterRequest request);
        void UpdateLogin(LoginRequest request);
    }

    public class AuthService : IAuthService
    {
        private DataContext _context;
        private IMapper _mapper;

        public AuthService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public UserDto Authenticate(LoginRequest request)
        {
            var validationError = "Username or password is incorrect.";

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                throw new ServiceValidationException(validationError);
            }

            var login = _context.Logins.SingleOrDefault(x => x.Username == request.Username.ToLower());

            // check if username exists
            if (login == null)
            {
                throw new ServiceValidationException(validationError);
            }

            // check if password is correct
            if (!VerifyPasswordHash(request.Password, login.PasswordHash, login.PasswordSalt))
            {
                throw new ServiceValidationException(validationError);
            }

            // authentication successful
            var user = _context.Users.SingleOrDefault(x => x.Id == login.UserId);

            return _mapper.Map<UserDto>(user);
        }

        public UserDto Register(RegisterRequest request)
        {
            var registrationType = RegistrationType.Unknown;

            var isEmail = request.Username.Contains('@') &&
                request.Username.IndexOf('.', request.Username.IndexOf('@')) > 0;

            if (isEmail)
            {
                registrationType = RegistrationType.Email;
            }
            else if (int.TryParse(request.Username, out var phone))
            {
                registrationType = RegistrationType.Phone;
            }

            // validation
            if (registrationType == RegistrationType.Unknown)
            {
                throw new ServiceValidationException("Username is invalid");
            }

            if (string.IsNullOrEmpty(request.Password))
            {
                throw new ServiceValidationException("Password is invalid");
            }

            if (_context.Logins.Any(x => x.Username == request.Username.ToLower()))
            {
                throw new ServiceValidationException($"{registrationType} {request.Username.ToLower()} is already taken");
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

            var entity = new User
            {
                Name = request.Name,
                Logins = new List<Login>()
            };

            if (registrationType == RegistrationType.Email)
            {
                entity.Email = request.Username.ToLower();
            }
            else if (registrationType == RegistrationType.Phone)
            {
                entity.Phone = request.Username.ToLower();
            }

            entity.Logins.Add(new Login
            {
                Username = request.Username.ToLower(),
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,

            });

            _context.Users.Add(entity);
            _context.SaveChanges();

            return _mapper.Map<UserDto>(entity);
        }

        public void UpdateLogin(LoginRequest request)
        {
            var entity = _context.Logins.SingleOrDefault(x => x.Username == request.Username.ToLower());

            if (entity == null)
            {
                throw new ServiceValidationException("Login not found");
            }

            // update password if provided
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

                entity.PasswordHash = passwordHash;
                entity.PasswordSalt = passwordSalt;
            }

            _context.Logins.Update(entity);
            _context.SaveChanges();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}