using System.Collections.Generic;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.QuestionRepo
{
    public interface IQuestionRepo
    {
        Task Create(Question question);
        Task Delete(Question question);
        Task DeleteRange(Question[] questions);
        Task<Question> GetById(int id);
        Task<Question> GetDetail(int id);
        Task<PageDataDto<Question>> GetByQuizId(int quiz, int start, int limit);
        Task<IEnumerable<Question>> GetByQuizId(int quizid);
        Task SaveChange();
    }
}
