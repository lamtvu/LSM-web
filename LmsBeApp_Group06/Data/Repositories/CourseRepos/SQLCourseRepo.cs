using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Models.Excel;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.CourseRepos
{
    public class SQLCourseRepo : ICourseRepo
    {
        private readonly LmsAppContext _context;
        public SQLCourseRepo(LmsAppContext context)
        {
            this._context = context;

        }
        public async Task Change(Course course)
        {
             if (course == null)
                throw new ArgumentNullException(nameof(course));
            _context.Courses.Update(course);
        }

        public async Task Create(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            await _context.Courses.AddAsync(course);
        }

        public async Task Delete(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            _context.Courses.Remove(course);
        }

        public async Task<PageDataDto<Course>> Get(int start, int limit)
        {
            var courses = await _context.Courses.ToListAsync();
            return new PageDataDto<Course> { Data = courses.OrderBy(x => x.Id).Skip(start).Take(limit), Count = courses.Count };
        }

        public async Task<PageDataDto<Course>> GetAll(string searchValue, int start, int limit)
        {
            List<Course> courses;
            if (!String.IsNullOrWhiteSpace(searchValue))
            {
                searchValue = searchValue.ToLower();
                courses = await _context.Courses.Include(x => x.Instructor)
                .Where(x => x.Instructor.Username.ToLower().Contains(searchValue) || x.Name.ToLower().Contains(searchValue)
                || x.Level.ToString().Contains(searchValue))
                .ToListAsync();
            }
            else
            {
                courses = await _context.Courses.Include(x => x.Instructor)
                .ToListAsync();
            }

            if (courses.Count == 0)
            {
                return new PageDataDto<Course> { Data = null, Count = 0 };
            }
            return new PageDataDto<Course> { Data = courses.OrderBy(x => x.Id).Skip(start).Take(limit), Count = courses.Count };
        }

        public async Task<List<Statistic>> GetStatistics()
        {
            List<Statistic> statistics = new List<Statistic>();
            var courses = await _context.Courses.Include(x => x.ClassesInUse).ToListAsync();

            if (courses.Count != 0)
            {
                foreach (var item in courses)
                {
                    var temp = new Statistic();
                    int? numClass = item.ClassesInUse.Count;
                    var classes = item.ClassesInUse;
                    int numStudent = 0;

                    foreach (var cls in classes)
                    {
                        var num = await _context.StudentClasses.Where(c => c.ClassId == cls.Id).ToListAsync();
                        numStudent += num.Count;
                    }

                    temp.numClass = (int)numClass;
                    temp.numStudent = numStudent;
                    temp.Id = item.Id;
                    temp.Name = item.Name;

                    statistics.Add(temp);
                }
            }
            return statistics;
        }

        public async Task<PageDataDto<Course>> GetByClassInUse(string searchValue, int classId, int start, int limit)
        {
            var _class = await _context.Classes.Include(x => x.Courses).ThenInclude(x => x.Instructor).FirstOrDefaultAsync(x => x.Id == classId);
            return new PageDataDto<Course> { Data = _class.Courses.OrderBy(x => x.Id).Skip(start).Take(limit), Count = _class.Courses.ToList().Count };
        }

        public Task<Course> GetById(int id)
        {
            return _context.Courses.Include(x => x.Instructor).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PageDataDto<Course>> GetByOwner(string nameSearch, string username, int start, int limit)
        {
            List<Course> courses;
            if (!String.IsNullOrWhiteSpace(nameSearch))
            {
                courses = await _context.Courses.Include(x => x.Instructor)
                .Where(x => x.Instructor.Username == username && x.Name.Contains(nameSearch))
                .ToListAsync();
            }
            else
            {
                courses = await _context.Courses.Include(x => x.Instructor)
                .Where(x => x.Instructor.Username == username)
                .ToListAsync();
            }

            if (courses.Count == 0)
            {
                return new PageDataDto<Course> { Data = null, Count = 0 };
            }
            return new PageDataDto<Course> { Data = courses.OrderBy(x => x.Id).Skip(start).Take(limit), Count = courses.Count };
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

         public async Task<PageDataDto<Course>> GetAll()
        {
            var courses = await _context.Courses.ToListAsync();
            var count = courses.ToList().Count;
            if (count == 0)
            {
                return new PageDataDto<Course> { Data = null, Count = 0 };
            }
            return new PageDataDto<Course> { Data = courses, Count = count };
        }

        public async Task<PageDataDto<Course>> GetAll(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetAll();
            }

            var search = searchQuery.Trim();
            var courses = await _context.Courses.Where(u=>u.Name.Contains(search)).ToListAsync();         
            var count = courses.Count;
            if (count == 0)
            {
                return new PageDataDto<Course> { Data = null, Count = 0 };
            }
            return new PageDataDto<Course> { Data = courses, Count = count };
        }

    }
}
