using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Data.Repositories.InvitationRepos
{
    public interface IInvitationRepos
    {
        Task<IEnumerable<Invitation>> GetAllOfClass(int classid);
        Task<IEnumerable<Invitation>> GetAllOfClass(string searchQuery, int classid);
        Task<IEnumerable<Invitation>> GetAllOfStudent(int userid);
        Task<IEnumerable<Invitation>> GetAllOfStudent(string searchQuery, int userid);
        Task CreateInvitation(Invitation invitation);
        Task RemoveInvitation(Invitation invitation);
        Task RemoveAllInvitation(IEnumerable<Invitation> invitations);
        Task<Invitation> GetById(int id);
        Task SaveChange();
        Task<IEnumerable<Invitation>> GetByPageForClass(int classid, int start, int limit);
        Task<IEnumerable<Invitation>> GetByPageForClass(string searchQuery, int classid, int start, int limit);
        Task<IEnumerable<Invitation>> GetByPageForStudent(int userid, int start, int limit);
        Task<IEnumerable<Invitation>> GetByPageForStudent(string searchQuery, int userid, int start, int limit);
    }
}
