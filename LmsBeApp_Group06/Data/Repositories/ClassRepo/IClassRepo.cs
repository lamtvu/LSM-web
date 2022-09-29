using System.Collections.Generic;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.ClassRepo
{
    public interface IClassRepo
    {
        Task<IEnumerable<Class>>getAll(string searchValue);
        Task<IEnumerable<Class>>getAll();
        Task Create(Class _class);
        Task<PageDataDto<Class>> GetStudingClass(int start, int limit, string username);
        Task<PageDataDto<Class>> GetOwnedClass(int start, int limit, string username, string searchValue);
        Task<PageDataDto<Class>> Get(int start, int limit);
        Task<int> GetCount();
        Task<Class> GetDetail(int id);
        Task<Class> GetById(int id);
        Task<PageDataDto<Class>> GetAll(int start, int limit, string searchValue);
        Task<PageDataDto<User>> GetStudents(int id,int start, int limit);
        Task AddStudent(User user,Class _class);
        Task RemoveStudent(User user,Class _class);
        Task<bool> CheckStuding(int classId, string username);
        Task Delete(Class _class);
        Task SaveChange();
    }
}
