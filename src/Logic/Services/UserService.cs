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
    public interface IUserService
    {
        UserDto GetById(int id);
        UserDto Add(UserDto user);
        UserDto Update(UserDto user);
        void Delete(int id);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private IMapper _mapper;

        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public UserDto GetById(int id)
        {
            var user = _context.Users.Find(id);

            return _mapper.Map<UserDto>(user);
        }

        public UserDto Add(UserDto user)
        {
            var entity = new User
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                ImageUrl = user.ImageUrl
            };

            _context.Users.Add(entity);
            _context.SaveChanges();

            return _mapper.Map<UserDto>(entity);
        }

        public UserDto Update(UserDto user)
        {
            var entity = _context.Users.Find(user.Id);

            if (entity == null)
            {
                throw new ServiceValidationException("User not found.");
            }

            // update email if it has changed
            if (!string.IsNullOrWhiteSpace(user.Email) && user.Email != entity.Email)
            {
                // throw error if the new email is already taken
                if (_context.Users.Any(x => x.Email == user.Email))
                {
                    throw new ServiceValidationException("Email " + user.Email + " is already taken.");
                }

                // Update the login as well if it exists
                if (entity.Logins.Any(x => x.Username == user.Email))
                {
                    entity.Logins.First(x => x.Username == user.Email).Username = user.Email;
                }

                entity.Email = user.Email;
            }

            // update email if it has changed
            if (!string.IsNullOrWhiteSpace(user.Phone) && user.Phone != entity.Phone)
            {
                // throw error if the new email is already taken
                if (_context.Users.Any(x => x.Phone == user.Phone))
                {
                    throw new ServiceValidationException("Phone " + user.Phone + " is already taken.");
                }

                // Update the login as well if it exists
                if (entity.Logins.Any(x => x.Username == user.Phone))
                {
                    entity.Logins.First(x => x.Username == user.Phone).Username = user.Phone;
                }

                entity.Phone = user.Phone;
            }

            entity.Name = user.Name;
            entity.Address = user.Address;
            entity.ImageUrl = user.ImageUrl;

            _context.Users.Update(entity);
            _context.SaveChanges();

            return _mapper.Map<UserDto>(entity);
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}