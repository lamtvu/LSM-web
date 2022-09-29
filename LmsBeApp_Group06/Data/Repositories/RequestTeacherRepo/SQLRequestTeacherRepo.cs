using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.RequestTeacherRepo
{
    public class SQLRequestTeacherRepo : IRequestTeacherRepo
    {
        private readonly LmsAppContext _context;
        public SQLRequestTeacherRepo(LmsAppContext context)
        {
            this._context = context;

        }
        public async Task CreateTeacherRequest(RequestTeacher requestTeacher)
        {
            if (requestTeacher == null)
            {
                throw new ArgumentNullException();
            }
            await _context.RequestTeachers.AddAsync(requestTeacher);
        }

        public async Task<IEnumerable<RequestTeacher>> GetAll()
        {
            return _context.RequestTeachers.Select(t => new RequestTeacher
            {
                Id = t.Id,
                Description = t.Description,
                CompanyName = t.CompanyName,
                CreateDate = t.CreateDate,
                User = new User
                {
                    Username = t.User.Username,
                    Id = t.User.Id,
                    Email = t.User.Email
                }
            });
        }

        public async Task<IEnumerable<RequestTeacher>> GetAll(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetAll();
            }

            var collection = await GetAll();
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var search = searchQuery.Trim();
                collection = collection.Where(a => a.User.Username.Contains(search)
                      || a.User.FullName.Contains(search)
                      || a.User.Email.Contains(search));
            }

            return collection.ToList();
        }

        public async Task<RequestTeacher> GetById(int id)
        {
            return await _context.RequestTeachers.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<RequestTeacher>> GetByPage(int start, int limit)
        {
            return _context.RequestTeachers.OrderByDescending(r=>r.Id).Skip(start).Take(limit).Select(t => new RequestTeacher
            {
                Id = t.Id,
                Description = t.Description,
                CompanyName = t.CompanyName,
                CreateDate = t.CreateDate,
                User = new User
                {
                    Username = t.User.Username,
                    Id = t.User.Id,
                    Email = t.User.Email
                }
            });
        }

        public async Task RemoveTeacherRequest(RequestTeacher requestTeacher)
        {
            _context.RequestTeachers.Remove(requestTeacher);
        }
        public async Task RemoveTeacherAllRequest(IEnumerable<RequestTeacher> requestTeachers)
        {
            _context.RequestTeachers.RemoveRange(requestTeachers);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }
}