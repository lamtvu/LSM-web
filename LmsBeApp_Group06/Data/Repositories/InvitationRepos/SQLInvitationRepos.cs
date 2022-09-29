using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Data.Repositories.InvitationRepos
{
    public class SQLInvitationRepos : IInvitationRepos
    {
        private readonly LmsAppContext _context;
        public SQLInvitationRepos(LmsAppContext context)
        {
            this._context = context;

        }
        public async Task CreateInvitation(Invitation invitation)
        {
            if (invitation == null)
            {
                throw new ArgumentNullException();
            }
            await _context.Invitations.AddAsync(invitation);
        }

        public async Task<IEnumerable<Invitation>> GetAllOfClass(int classid)
        {
            var invitations = await _context.Invitations.Include(c => c.Class).Include(c => c.Receiver)
                                                         .Where(r => r.ClassId == classid).ToListAsync();
            return invitations.ToList();
        }

        public async Task<IEnumerable<Invitation>> GetAllOfClass(string searchQuery, int classid)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetAllOfClass(classid);
            }

            var collection = await GetAllOfClass(classid);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var search = searchQuery.Trim();
                collection = collection.Where(a => a.Receiver.Username.Contains(search)
                      || a.Receiver.FullName.Contains(search)
                      || a.Receiver.Email.Contains(search));
            }

            return collection.ToList();
        }

        public async Task<IEnumerable<Invitation>> GetAllOfStudent(int userid)
        {
            var invitations = await _context.Invitations.Include(c => c.Class).Include(c => c.Receiver)
                                                         .Where(r => r.ReceiverId == userid).ToListAsync();
            return invitations.ToList();
        }

        public async Task<IEnumerable<Invitation>> GetAllOfStudent(string searchQuery, int userid)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetAllOfStudent(userid);
            }

            var collection = await GetAllOfStudent(userid);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var search = searchQuery.Trim();
                collection = collection.Where(a => a.Receiver.Username.Contains(search)
                      || a.Receiver.FullName.Contains(search)
                      || a.Receiver.Email.Contains(search));
            }

            return collection.ToList();
        }

        public async Task<Invitation> GetById(int id)
        {
            return await _context.Invitations.Include(r => r.Class).Include(r=>r.Receiver)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Invitation>> GetByPageForClass(int classid, int start, int limit)
        {
            var invitations = _context.Invitations.Include(u => u.Class)
                                               .OrderByDescending(r => r.Id).Where(r => r.ClassId == classid)
                                               .Skip(start).Take(limit);
            return invitations.ToList();
        }

        public async Task<IEnumerable<Invitation>> GetByPageForStudent(int userid, int start, int limit)
        {
            var invitations = _context.Invitations.Include(u => u.Class)
                                                  .OrderByDescending(r => r.Id).Where(r=>r.ReceiverId==userid)
                                                  .Skip(start).Take(limit);
            return invitations.ToList();
        }

        public async Task<IEnumerable<Invitation>> GetByPageForStudent(string searchQuery, int userid, int start, int limit)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetByPageForStudent(userid, start, limit);
            }

            var collection = await GetByPageForStudent(userid, start, limit);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var search = searchQuery.Trim();
                collection = collection.Where(a => a.Class.Name.Contains(search));
            }

            return collection.ToList();
        }

        public async Task<IEnumerable<Invitation>> GetByPageForClass(string searchQuery, int classid, int start, int limit)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return await GetByPageForClass(classid, start, limit);
            }

            var collection = await GetByPageForClass(classid, start, limit);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var search = searchQuery.Trim();
                collection = collection.Where(a => a.Receiver.Username.Contains(search)
                      || a.Receiver.FullName.Contains(search)
                      || a.Receiver.Email.Contains(search));
            }

            return collection.ToList();
        }

        public async Task RemoveAllInvitation(IEnumerable<Invitation> invitations)
        {
            _context.Invitations.RemoveRange(invitations);
        }

        public async Task RemoveInvitation(Invitation invitation)
        {
            _context.Invitations.Remove(invitation);
        }

        public async Task SaveChange()
        {
            _context.SaveChanges();
        }
    }
}
