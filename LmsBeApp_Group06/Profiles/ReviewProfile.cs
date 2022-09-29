using AutoMapper;
using LmsBeApp_Group06.Dtos.Reivew;
using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Profiles
{
    public class ReviewProfile:Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewReadDto>();
            CreateMap<ReviewReadDto, Review>();
            CreateMap<Review, ReviewCreateDto>();
            CreateMap<ReviewCreateDto, Review>();
            CreateMap<Review, ReviewEditDto>();
            CreateMap<ReviewEditDto, Review>();
        }
    }
}
