using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Data.Repositories.CourseRepos
{
    public interface IRequestStudentRepos
    {
        Task<IEnumerable<RequestStudent>> GetAll(int classid);
        Task<IEnumerable<RequestStudent>> GetAll(string searchQuery, int classid);
        Task CreateStudentRequest(RequestStudent requestStudent);
        Task RemoveStudentRequest(RequestStudent requestStudent);
        Task RemoveStudentAllRequest(IEnumerable<RequestStudent> requestStudents);
        Task<RequestStudent> GetById(int id);
        Task SaveChange();
        Task<PageDataDto<RequestStudent>> GetByPage(int classid, int start, int limit);
        Task<PageDataDto<RequestStudent>> GetByPage(string searchQuery, int classid, int start, int limit);
        Task<bool> CheckIfRequested(int classid, string username);
        Task<int> GetByClassId(int classid, string username);
    }
}
