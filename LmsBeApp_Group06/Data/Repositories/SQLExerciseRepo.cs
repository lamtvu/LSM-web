using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.ExerciseRepo
{
    public class SQLExerciseRepo : IExerciseRepo
    {
        private readonly LmsAppContext _context;
        public SQLExerciseRepo(LmsAppContext context)
        {
            this._context = context;
        }
        public Task Change(Exercise exercise)
        {
            //nothing
            throw new System.NotImplementedException();
        }

        public async Task Create(Exercise exercise)
        {
            if (exercise == null)
            {
                throw new ArgumentNullException(nameof(exercise));
            }
            await _context.Exercises.AddAsync(exercise);
        }

        public async Task Delete(Exercise exercise)
        {
            if (exercise == null)
            {
                throw new ArgumentNullException(nameof(exercise));
            }
            _context.Exercises.Remove(exercise);
        }

        public async Task<IEnumerable<Exercise>> GetByClassId(int id)
        {
            var exercises = _context.Exercises.Where(x => x.ClassId == id);
            return exercises;
        }

        public async Task<Exercise> GetById(int id)
        {
            return await _context.Exercises.FindAsync(id);
        }

        public async Task<IEnumerable<Exercise>> GetByUserId(int id)
        {
            var user = await _context.Users.Include(x => x.StudingClasses).FirstOrDefaultAsync(x => x.Id == id);
            var excercise = _context.Exercises.Where(x => user.StudingClasses.Any(c => c.Id == x.ClassId));
            return excercise;
        }

        public async Task<Exercise> GetDetail(int id)
        {
            return await _context.Exercises.Include(x=>x._Class).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }
}
