using System.Collections.Generic;
using System.Threading.Tasks;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.ContentRepo
{
    public interface IContentRepo
    {
        Task Create(Section section, Content content);
        Task Delete(Content content);
        Task Change(Content content);
        Task SaveChange();
        Task<IEnumerable<Content>> GetBySectionId(int id);
        Task<Content> GetById(int id);
    }
}
