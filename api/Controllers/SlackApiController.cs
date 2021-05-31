using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace lookaroond.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SlackApiController : ControllerBase
    {
        private ILogger<SlackApiController> _logger;

        public SlackApiController(ILogger<SlackApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
