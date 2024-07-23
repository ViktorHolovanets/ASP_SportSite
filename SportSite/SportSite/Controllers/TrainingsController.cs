using Microsoft.AspNetCore.Mvc;
using SportSite.Models.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportSite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly Db _context;
        public TrainingsController(Db db)
        {
            _context = db;
        }
        [HttpGet]
        [Route("Index")]
        public ActionResult<IEnumerable<Services>> Index()
        {
            var services = _context.Services;
            return services;
        }
    }
}
