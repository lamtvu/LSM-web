using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Data.Repositories.AnnouncementRepos
{
    public class SQLAnnouncementRepos : IAnnouncementRepos
    {
        private readonly LmsAppContext _context;
        public SQLAnnouncementRepos(LmsAppContext context)
        {
            this._context = context;

        }
        public async Task CreateAnnouncement(Announcement announcement)
        {
            if (announcement == null)
            {
                throw new ArgumentNullException();
            }
            await _context.Announcements.AddAsync(announcement);
        }

        public async Task UpdateAnnouncement(Announcement announcement)
        {
            if (announcement == null)
            {
                throw new ArgumentNullException();
            }
            _context.Announcements.Update(announcement);
        }

        public async Task<IEnumerable<Announcement>> GetAllByProgram(int classid)
        {
            var announcements = await _context.Announcements.Include(c => c.Class)
                                                         .OrderByDescending(r => r.Id)
                                                         .Where(r => r.ClassId == classid&&r.Type=="Program")
                                                         .ToListAsync();
            
            return announcements.ToList();
        }

        public async Task<IEnumerable<Announcement>> GetAllByNotifyOfStudent(User user)
        {
            var announcements = await _context.Announcements.Include(c => c.Class)
                                                         .OrderByDescending(r => r.Id)
                                                         .Where(r => r.Class.students.Contains(user) && r.Type == "Notify")
                                                         .ToListAsync();

            return announcements.ToList();
        }

        public async Task<IEnumerable<Announcement>> GetAllByNotifyOfClass(int classid)
        {
            var announcements = await _context.Announcements.Include(c => c.Class)
                                                        .OrderByDescending(r => r.Id)
                                                         .Where(r => r.ClassId == classid && r.Type == "Notify")
                                                         .ToListAsync();

            return announcements.ToList();
        }

        public async Task<Announcement> GetById(int id)
        {
            return await _context.Announcements.Include(r => r.Class)
                                               .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Announcement>> GetByPageProgram(int classid, int start, int limit)
        {
            var announcements = _context.Announcements.Include(u => u.Class)
                                               .OrderByDescending(r => r.Id).Where(r => r.ClassId == classid && r.Type=="Program")
                                               .Skip(start).Take(limit);
            return announcements.ToList();
        }

        public async Task<IEnumerable<Announcement>> GetByPageNotifyOfStudent(User user, int start, int limit)
        {
            var announcements = _context.Announcements.Include(u => u.Class)
                                                  .OrderByDescending(r => r.Id).Where(r => r.Class.students.Contains(user)&&r.Type=="Notify")
                                                  .Skip(start).Take(limit);
            return announcements.ToList();
        }

        public async Task<IEnumerable<Announcement>> GetByPageNotifyOfClass(int classid, int start, int limit)
        {
            var announcements = _context.Announcements.Include(u => u.Class)
                                                  .OrderByDescending(r => r.Id).Where(r => r.ClassId==classid && r.Type == "Notify")
                                                  .Skip(start).Take(limit);
            return announcements.ToList();
        }

        public async Task RemoveAllAnnouncement(IEnumerable<Announcement> announcements)
        {
            _context.Announcements.RemoveRange(announcements);
        }

        public async Task RemoveAnnouncement(Announcement announcement)
        {
            _context.Announcements.Remove(announcement);
        }

        public async Task SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
