using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Data.Repositories.AnnouncementRepos
{
    public interface IAnnouncementRepos
    {
        Task<IEnumerable<Announcement>> GetAllByProgram(int classid);
        Task<IEnumerable<Announcement>> GetAllByNotifyOfClass(int classid);
        Task<IEnumerable<Announcement>> GetAllByNotifyOfStudent(User user);
        Task CreateAnnouncement(Announcement announcement);
        Task UpdateAnnouncement(Announcement announcement);
        Task RemoveAnnouncement(Announcement announcement);
        Task RemoveAllAnnouncement(IEnumerable<Announcement> invitations);
        Task<Announcement> GetById(int id);
        Task SaveChange();
        Task<IEnumerable<Announcement>> GetByPageProgram(int classid, int start, int limit);
        Task<IEnumerable<Announcement>> GetByPageNotifyOfStudent(User user, int start, int limit);
        Task<IEnumerable<Announcement>> GetByPageNotifyOfClass(int classid, int start, int limit);

    }
}
