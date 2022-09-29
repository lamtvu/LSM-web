using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Data.Repositories.CourseRepos
{
    public class SQLRequestStudentRepos : IRequestStudentRepos
    {
        private readonly LmsAppContext _context;
        public SQLRequestStudentRepos(LmsAppContext context)
        {
            this._context = context;

        }
        public async Task CreateStudentRequest(RequestStudent requestStudent)
        {
            if (requestStudent == null)
            {
                throw new ArgumentNullException();
            }
            await _context.RequestStudents.AddAsync(requestStudent);
        }

        public async Task<IEnumerable<RequestStudent>> GetAll(int classid)
        {
            var requests = await _context.RequestStudents.Include(c => c.Class).Include(c => c.Sender)
                                                            .OrderByDescending(r => r.Id)
                                                          .Where(r => r.ClassId == classid).ToListAsync();
            return requests.ToList();
        }

        public async Task<IEnumerable<RequestStudent>> GetAll(string searchQuery, int classid)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetAll(classid);
            }

            var collection = await GetAll(classid);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var search = searchQuery.Trim();
                collection = collection.Where(a => a.Sender.Username.Contains(search)
                      || a.Sender.FullName.Contains(search)
                      || a.Sender.Email.Contains(search));
            }

            return collection.ToList();
        }

        public async Task<RequestStudent> GetById(int id)
        {
            return await _context.RequestStudents.Include(r => r.Class)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

         public async Task<int> GetByClassId(int classid, string username)
        {
            var result=await  _context.RequestStudents.Include(r => r.Class).FirstOrDefaultAsync(r => r.ClassId==classid && r.Sender.Username==username);
            return result.Id;
        }

        public async Task<PageDataDto<RequestStudent>> GetByPage(int classid, int start, int limit)
        {
            var requests = _context.RequestStudents.Include(x => x.Sender)
                                                    .OrderByDescending(r => r.Id)
                                                    .Where(r => r.ClassId == classid);
            return new PageDataDto<RequestStudent> { Data = requests.OrderByDescending(r => r.Id).Skip(start).Take(limit), Count = requests.ToList().Count };
        }

        public async Task<PageDataDto<RequestStudent>> GetByPage(string searchQuery, int classid, int start, int limit)
        {
            if (String.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetByPage(classid, start, limit);
            }
            var search = searchQuery.Trim();
            var requests = _context.RequestStudents.Include(x => x.Sender)
                                                    .Where(r => r.ClassId == classid
                                                    || r.Sender.Username.Contains(search)
                                                    || r.Sender.FullName.Contains(search)
                                                    || r.Sender.Email.Contains(search));
            return new PageDataDto<RequestStudent> { Data = requests.OrderByDescending(r => r.Id).Skip(start).Take(limit), Count = requests.ToList().Count };
        }

        public async Task RemoveStudentRequest(RequestStudent requestStudent)
        {
            _context.RequestStudents.Remove(requestStudent);
        }
        public async Task RemoveStudentAllRequest(IEnumerable<RequestStudent> requestStudents)
        {
            _context.RequestStudents.RemoveRange(requestStudents);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfRequested(int classid, string username)
        {
            var result= await _context.RequestStudents.FirstOrDefaultAsync(c =>c.ClassId==classid&& c.Sender.Username==username);
            if(result==null)
            {
                return false;
            }
            return true;
        }
    }
}

