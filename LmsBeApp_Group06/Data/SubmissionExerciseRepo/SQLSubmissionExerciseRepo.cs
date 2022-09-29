using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using Microsoft.EntityFrameworkCore;

namespace LmsBeApp_Group06.Data.Repositories.SubmisstionExerciseRepo
{
    public class SQLSubmissionExerciseRepo : ISubmissionExerciseRepo
    {
        private readonly LmsAppContext _context;
        public SQLSubmissionExerciseRepo(LmsAppContext context)
        {
            this._context = context;
        }
        public Task Change(SubmissionExercise submission)
        {
            //nothing
            throw new System.NotImplementedException();
        }

        public async Task Create(SubmissionExercise submission)
        {
            if (submission == null)
            {
                throw new ArgumentNullException(nameof(submission));
            }
            await _context.AddAsync(submission);
        }

        public async Task Delete(SubmissionExercise submission)
        {
            if (submission == null)
            {
                throw new ArgumentNullException(nameof(submission));
            }
            _context.Remove(submission);
        }

        public async Task<PageDataDto<SubmissionExercise>> GetByExerciseId(int id, int start, int limit, string searchValue)
        {
            List<SubmissionExercise> submissions;
            if (!String.IsNullOrWhiteSpace(searchValue))
            {
                submissions = await _context.SubmissionExercises.Include(x => x.Student)
                .Where(x => x.ExerciseId == id && x.Student.Username.Contains(searchValue))
                .ToListAsync();
            }
            else
            {
                submissions = await _context.SubmissionExercises.Where(x => x.ExerciseId == id)
                .Include(x => x.Student).OrderBy(x => x.Id).ToListAsync();
            }
            return new PageDataDto<SubmissionExercise> { Count = submissions.Count, Data = submissions.OrderBy(x => x.Id).Skip(start).Take(limit) };
        }

        public async Task<SubmissionExercise> GetByExerciseIdAndUserID(int userId, int exerciseId)
        {
            var submission = await _context.SubmissionExercises.FirstOrDefaultAsync(x => x.ExerciseId == exerciseId && x.StudentId == userId);
            return submission;
        }

        public async Task<SubmissionExercise> GetById(int id)
        {
            return await _context.SubmissionExercises.FindAsync(id);
        }

        public async Task<SubmissionExercise> GetDetail(int id)
        {
            return await _context.SubmissionExercises
            .Include(x => x.Exercise)
            .Include(x => x.Student)
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<SubmissionExercise>> GetOwnedInClass(int classId, int userId)
        {
            var submissions = _context.SubmissionExercises.Include(x => x.Exercise)
            .Where(x => x.StudentId == userId && x.Exercise.ClassId == classId);
            return submissions;
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }
    }
}
