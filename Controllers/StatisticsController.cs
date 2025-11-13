using WorkNest.Enums;
using WorkNest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WorkNest.Services;

namespace WorkNest.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    [Authorize(Roles = ROLE.ADMIN)]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsService _statsService;

        public StatisticsController(StatisticsService statsService)
        {
            _statsService = statsService;
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult> GetDashboardStats()
        {
            var stats = await _statsService.GetDashboardStats();
            return Ok(stats);
        }
    }
}