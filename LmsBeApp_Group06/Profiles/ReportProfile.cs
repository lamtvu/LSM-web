using AutoMapper;
using LmsBeApp_Group06.Dtos;
using LmsBeApp_Group06.Dtos.Report;
using LmsBeApp_Group06.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Profiles
{
    public class ReportProfile: Profile
    {
        public ReportProfile()
        {            
            CreateMap<Report, ReportReadDto>();
            CreateMap<ReportReadDto, Report>();
            CreateMap<Report, ReportCreateDto>();
            CreateMap<ReportCreateDto, Report>();

        }
    }
}
