using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Profiles
{
    public class RequestStudentProfile:Profile
    {
        public RequestStudentProfile()
        {
            CreateMap<RequestStudent, RequestStudentReadDto>();
            CreateMap<RequestStudentReadDto, RequestStudent>();
        }
    }
}
