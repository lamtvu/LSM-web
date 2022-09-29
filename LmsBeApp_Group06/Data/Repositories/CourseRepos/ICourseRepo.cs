using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using LmsBeApp_Group06.Models.Excel;

namespace LmsBeApp_Group06.Data.Repositories.CourseRepos
{
    public interface ICourseRepo
    {
        Task Create(Course course);
        Task Delete(Course course);
        Task Change(Course course);
        Task<PageDataDto<Course>> GetAll(string searchValue, int start, int limit);
        Task<Course> GetById(int id);
        Task<PageDataDto<Course>> GetByOwner(string searchValue, string username, int start, int limit);
        Task<PageDataDto<Course>> GetByClassInUse(string valueSearch, int classId, int start, int limit);
        Task<List<Statistic>> GetStatistics();
        Task SaveChange();
        Task<PageDataDto<Course>> GetAll(string searchQuery);
         Task<PageDataDto<Course>> GetAll();
    }
}
