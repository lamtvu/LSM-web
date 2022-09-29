using System.Collections.Generic;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.SubmissionQuizRepo
{
    public interface ISubmissionQuizRepo
    {
        Task Create(SubmissionQuiz submissionQuiz);
        Task Delete(SubmissionQuiz submissionQuiz);
        Task<SubmissionQuiz> GetById(int id);
        Task<SubmissionQuiz> GetDetail(int id);
        Task<SubmissionQuiz> GetByUsernameAndQuizId(int userId, int quizId);
        Task<PageDataDto<SubmissionQuiz>> GetByQuizId(int quizId, int start, int limit, string searchValue);
        Task<IEnumerable<SubmissionQuiz>> GetOwnedSubmissions(int classId, int userId);
        Task SaveChange();
    }
}