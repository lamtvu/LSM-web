using System.Collections.Generic;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.SubmisstionExerciseRepo
{
    public interface ISubmissionExerciseRepo
    {
        Task Create(SubmissionExercise submission);
        Task Delete(SubmissionExercise submission);
        Task Change(SubmissionExercise submission);
        Task<SubmissionExercise> GetById(int id);
        Task<SubmissionExercise> GetDetail(int id);
        Task<PageDataDto<SubmissionExercise>> GetByExerciseId(int id, int start, int limit, string searchValue);
        Task<SubmissionExercise> GetByExerciseIdAndUserID(int userId, int exerciseId);
        Task<IEnumerable<SubmissionExercise>> GetOwnedInClass(int classId, int userId);
        Task SaveChange();
    }
}
