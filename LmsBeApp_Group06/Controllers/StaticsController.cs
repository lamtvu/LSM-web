using AutoMapper;
using ClosedXML.Excel;
using LmsBeApp_Group06.Data.Repositories.CourseRepos;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace LmsBeApp_Group06.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StaticsController : ControllerBase
    {
        private readonly ICourseRepo _courseRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        public StaticsController(ICourseRepo courseRepo, IUserRepo userRepo, IMapper mapper)
        {
            this._mapper = mapper;
            this._userRepo = userRepo;
            this._courseRepo = courseRepo;

        }

        [HttpGet]
        public async Task<IActionResult> ExportExcel()
        {
            var courses = await _courseRepo.GetStatistics();

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Statistics");

            var currentRow = 3;
            worksheet.Range("A1:H1").Merge().Value= "STATISTICS";
            
            worksheet.Cell(currentRow, 2).Value = "COURSE ID";
            worksheet.Cell(currentRow, 3).Value = "COURSE NAME";
            worksheet.Cell(currentRow, 4).Value = "NUMBER OF CLASS";
            worksheet.Cell(currentRow, 5).Value = "NUMBER OF STUDENT";

            foreach(var item in courses)
            {
                currentRow++;
                worksheet.Cell(currentRow, 2).Value = item.Id;
                worksheet.Cell(currentRow, 3).Value = item.Name;
                worksheet.Cell(currentRow, 4).Value = item.numClass;
                worksheet.Cell(currentRow, 5).Value = item.numStudent;
            }

           
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Statistic.xlsx");

        }
    }
}
