using System.Collections.Generic;
using System.Threading.Tasks;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.RequestTeacherRepo
{
    public interface IRequestTeacherRepo
    {
        Task<IEnumerable<RequestTeacher>> GetAll();
        Task<IEnumerable<RequestTeacher>> GetAll(string searchQuery);
        Task CreateTeacherRequest(RequestTeacher requestTeacher);
        Task RemoveTeacherRequest(RequestTeacher requestTeacher);
        Task RemoveTeacherAllRequest(IEnumerable<RequestTeacher> requestTeachers);
        Task<RequestTeacher> GetById(int id);
        Task SaveChange();
        Task<IEnumerable<RequestTeacher>> GetByPage(int start, int limit);
    }
}