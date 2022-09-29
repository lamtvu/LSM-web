using System.Collections.Generic;
using System.Threading.Tasks;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Data.Repositories.ExerciseRepo
{
    public interface IExerciseRepo
    {
        Task Create(Exercise exercise);
        Task Delete(Exercise exercise);
        Task Change(Exercise exercise);
        Task<Exercise> GetById(int id);
        Task<Exercise> GetDetail(int id);
        Task< IEnumerable<Exercise>> GetByClassId(int id); 
        Task< IEnumerable<Exercise>> GetByUserId(int id); 
        Task SaveChange();
    }
}
