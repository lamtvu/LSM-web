using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.ClassRepo
{
    public class SQLClassRepo : IClassRepo
    {
        private readonly LmsAppContext _context;
        public SQLClassRepo(LmsAppContext context)
        {
            this._context = context;
        }

        public async Task AddStudent(User user, Class _class)
        {
            // nothing
        }

        public async Task<bool> CheckStuding(int classId, string username)
        {
            var _class = await _context.Classes.Include(x => x.students).FirstOrDefaultAsync(x => x.Id == classId);
            if (_class == null)
            {
                throw new ArgumentNullException(nameof(classId));
            }
            return _class.students.Any(x => x.Username == username);
        }

        public async Task Create(Class _class)
        {
            if (_class == null)
            {
                throw new ArgumentNullException(nameof(_class));
            }
            await _context.Classes.AddAsync(_class);
        }

        public async Task Delete(Class _class)
        {
            if (_class == null)
            {
                throw new ArgumentNullException(nameof(_class));
            }
            _context.StudentClasses.RemoveRange(_context.StudentClasses.Where(x => x.ClassId == _class.Id));
            _context.StudentClasses.RemoveRange(_context.StudentClasses.Where(x => x.ClassId == _class.Id));
            _context.Classes.Remove(_class);
        }

        public async Task<PageDataDto<Class>> Get(int start, int limit)
        {
            var _classes = _context.Classes.Include(c => c.Teacher).OrderBy(x => x.Id);
            return new PageDataDto<Class> { Data = _classes.OrderBy(x => x.Id).Skip(start).Take(limit) };
        }

        public async Task<Class> GetById(int id)
        {
            return await _context.Classes.Include(c => c.students)
                                         .Include(c => c.ClassAdmin)
                                         .OrderBy(c => c.Id)
                                         .AsSplitQuery()
                                         .FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task<int> GetCount()
        {
            return await _context.Classes.CountAsync();
        }

        public async Task<Class> GetDetail(int id)
        {
            var _class = await _context.Classes
            .Include(c => c.Courses)
            .Include(c => c.students)
            .OrderBy(c => c.Id)
            .AsSplitQuery().FirstOrDefaultAsync(c => c.Id == id);
            return _class;
        }

        public async Task<PageDataDto<Class>> GetOwnedClass(int start, int limit, string username, string searchValue)
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            List<Class> classes;
            if (String.IsNullOrWhiteSpace(searchValue))
            {
                classes = await _context.Classes.Include(x => x.Teacher)
                .Where(x => x.Teacher.Username == username).ToListAsync();
            }
            else
            {
                classes = await _context.Classes.Include(x => x.Teacher)
                .Where(x => x.Teacher.Username == username && (x.Name.Contains(searchValue) || x.Description.Contains(searchValue))).ToListAsync();
            }

            return new PageDataDto<Class>
            {
                Count = classes.ToList().Count,
                Data = classes.OrderBy(c => c.Id).Skip(start).Take(limit)
            };
        }

        public async Task<PageDataDto<User>> GetStudents(int id, int start, int limit)
        {
            var _class = _context.Classes.Include(c => c.students).FirstOrDefault(x => x.Id == id);
            return new PageDataDto<User> { Data = _class.students.OrderBy(x => x.Id).Skip(start).Take(limit), Count = _class.students.Count };
        }

        public async Task<PageDataDto<Class>> GetStudingClass(int start, int limit, string username)
        {
            var user = await _context.Users.Include(x => x.StudingClasses).ThenInclude(x => x.Teacher).FirstOrDefaultAsync(x => x.Username == username);
            var count = user.StudingClasses.ToList().Count;
            if (count == 0)
            {
                return new PageDataDto<Class> { Data = null, Count = 0 };
            }
            return new PageDataDto<Class> { Data = user.StudingClasses.OrderBy(x => x.Id).Skip(start).Take(limit), Count = count };
        }


        public Task RemoveStudent(User user, Class _class)
        {
            //nothing
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Class>> getAll()
        {
            var classes = await _context.Classes.Include(c => c.ClassAdmin).Include(c => c.Teacher).ToListAsync();

            return classes.ToList();

        }



        public async Task<IEnumerable<Class>> getAll(string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                return await getAll();
            }

            var collection = await getAll();

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                var search = searchValue.Trim();
                collection = collection.Where(a => a.Name.Contains(search));
            }

            return collection.ToList();
        }


        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<PageDataDto<Class>> GetAll(int start, int limit, string searchValue)
        {
            List<Class> _classes;
            if (String.IsNullOrWhiteSpace(searchValue))
            {
                _classes = await _context.Classes.Include(x => x.Teacher).ToListAsync();
            }
            else
            {
                searchValue = searchValue.ToLower();
                _classes = await _context.Classes.Include(x => x.Teacher).Where(x => x.Name.ToLower().Contains(searchValue)).ToListAsync();
            }

            return new PageDataDto<Class>
            {
                Data = _classes.OrderBy(x => x.Id).Skip(start).Take(limit),
                Count = _classes.Count
            };

        }
    }
}
