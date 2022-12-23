using Microsoft.AspNetCore.Mvc;
using CasestudyWebsite.Reports;

namespace CasestudyWebsite.Controllers
{
    public class ReportController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public ReportController(IWebHostEnvironment env)
        {
            _env = env;
        }
        [Route("api/helloreport")]
        [HttpGet]
        public IActionResult GetHelloReport()
        {
            HelloReport hello = new();
            hello.GenerateReport(_env.WebRootPath);
            return Ok(new { msg = "Report Generated" });
        }

        [Route("api/employeereport")]
        [HttpGet]
        public async Task<IActionResult> GetStudentReport()
        {
            EmployeeReport report = new();
            await report.GenerateReport(_env.WebRootPath);
            return Ok(new { msg = "Report Generated" });
        }

        [Route("api/callreport")]
        [HttpGet]
        public async Task<IActionResult> GetCallReport()
        {
            CallReport report = new();
            await report.GenerateReport(_env.WebRootPath);
            return Ok(new { msg = "Report Generated" });
        }
    }
}
