using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Profiles
{
    public class SubmissionProfile : Profile
    {
        public SubmissionProfile()
        {
            CreateMap<SubmissionExercise, SubmissionExerciseReadDto>();
            CreateMap<SubmissionExerciseChangeDto, SubmissionExercise>();
        }
    }
}
