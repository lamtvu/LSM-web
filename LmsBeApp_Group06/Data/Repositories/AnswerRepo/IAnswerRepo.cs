using System.Collections.Generic;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.AnswerRepo
{
    public interface IAnswerRepo
    {
        Task Create(Answer answer);
        Task CreateRange(Answer[] answers);
        Task Delete(Answer answer);
        Task DeleteRange(Answer[] answer);
        Task Edit(Answer answer);
        Task SaveChage();
        Task<Answer> GetById(int id);
        Task<Answer> GetDetail(int id);
        Task<IEnumerable<Answer>> GetByQuestionId(int questionId);
    }
}