
using System.Collections.Generic;
using System.Threading.Tasks;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.SectionRepo
{
    public interface ISectionRepo
    {
        Task Create(Section section, Course course);
        Task Delete(Section section);
        Task Change(Section section);
        Task<Section> GetById (int id);
        Task<Section> GetDetail(int id);
        Task SaveChange();
        Task<IEnumerable<Section>> GetByCourseId(int courseId);
    }
}
