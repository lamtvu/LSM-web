using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.UserRepos
{
    public class SQLUserRepo : IUserRepo
    {
        private readonly LmsAppContext _context;
        private readonly IMapper _mapper;
        public SQLUserRepo(LmsAppContext context, IMapper mapper)
        {
            this._mapper = mapper;
            this._context = context;

        }
        public async Task Change(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            _context.Users.Update(user);
        }

        public async Task CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            await _context.Users.AddAsync(user);
        }

        public async Task Delete(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            _context.Users.Remove(user);
        }

        public async Task<PageDataDto<User>> GetAll()
        {
            var users = await _context.Users.Include(u => u.Role).Where(u=>u.RoleId!=1).ToListAsync();
            var count = users.ToList().Count;
            if (count == 0)
            {
                return new PageDataDto<User> { Data = null, Count = 0 };
            }
            return new PageDataDto<User> { Data = users, Count = count };
        }

        public async Task<PageDataDto<User>> GetAll(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetAll();
            }

            var search = searchQuery.Trim();
            var users = await _context.Users.Include(u => u.Role).Where(u=>u.RoleId!=1
                                                                    && (u.Username.Contains(search)
                                                                    || u.Role.RoleName.Contains(search))).ToListAsync();         
            var count = users.Count;
            if (count == 0)
            {
                return new PageDataDto<User> { Data = null, Count = 0 };
            }
            return new PageDataDto<User> { Data = users, Count = count };
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.Include(u => u.Role).Include(u => u.StudingClasses)
            .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<PageDataDto<User>> GetByPage(int start, int limit, string searchValue)
        {
            List<User> users;
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                users = await _context.Users.Include(u => u.Role).ToListAsync();
            }
            else
            {
                users = await _context.Users.Include(u => u.Role)
                .Where(x => x.Username.Contains(searchValue) || x.Email.Contains(searchValue))
                .ToListAsync();
            }
            return new PageDataDto<User> { Data = users.OrderBy(user => user.Id).Skip(start).Take(limit), Count = users.Count };
        }

        public Task<User> GetByUsername(string username)
        {
            return _context.Users.Include(u => u.Role).Include(u => u.StudingClasses)
            .FirstOrDefaultAsync(u => u.Username == username);
        }

        public Task<User> GetDetail(int id)
        {
            return _context.Users.Include(u => u.Role)
            .Include(u => u.Information)
            .FirstOrDefaultAsync(u => u.Id == id);
        }
        public Task<User> GetDetail(string username)
        {
            return _context.Users.Include(u => u.Role)
            .Include(u => u.Information)
            .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }

}