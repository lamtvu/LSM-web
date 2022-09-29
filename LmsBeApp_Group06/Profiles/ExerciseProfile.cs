using System;
using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;

namespace LmsBeApp_Group06.Profiles
{
    public class ExerciseProfile : Profile
    {
        public ExerciseProfile()
        {
            CreateMap<ExerciseCreateDto,Exercise>();
            CreateMap<ExerciseChangeDto,Exercise>();
            CreateMap<Exercise,ExerciseReadDto>()
            .ForMember(x=>x.DueDate, opt=>opt.MapFrom<CreateDateResolve>());
        }
        public class CreateDateResolve : IValueResolver<Exercise, ExerciseReadDto, DateTime>
        {
            public DateTime Resolve(Exercise source, ExerciseReadDto destination, DateTime destMember, ResolutionContext context)
            {
                
                return source.CreateDate.AddHours(7);
            }
        }
    }
}
