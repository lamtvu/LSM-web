using System.Collections.Generic;
using System.Threading.Tasks;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.QuizRepo
{
    public interface IQuizRepo
    {
        Task Create(Quiz quiz);
        Task Delete(Quiz quiz);
        Task<Quiz> GetById(int id);
        Task<IEnumerable<Quiz>> GetByClassId(int classId);
        Task Remove(Quiz quiz);
        Task SaveChange();
    }
}
