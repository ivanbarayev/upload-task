using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using upload_task.Models;

namespace upload_task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class SettingsController : Controller
    {
        private static string collection_name = "settings";
        private readonly ILogger<SettingsController> _logger;
        private Common cm = new Common();
        SettingsModel mod = new SettingsModel();

        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }

        [HttpGet, DisableRequestSizeLimit]
        public IActionResult Get()
        {
            try
            {
                var data = mod.SettingsRead();
                return Ok(cm.Responser(1, "Success GET", data));
            }
            catch (Exception ex)
            {
                return StatusCode(500, cm.Responser(2, $"Internal server error : {ex}"));
            }
        }


        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Post([FromBody] Settings data)
        {
            try
            {
                cm.WriteToFile(JsonConvert.SerializeObject(data));
                return Ok(cm.Responser(1, "Success POST"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, cm.Responser(2, $"Internal server error : {ex}"));
            }
        }


        [HttpPut, DisableRequestSizeLimit]
        public IActionResult Put([FromBody] Settings data)
        {
            try
            {
                var leo = mod.SettingsUpdate(data);
                return Ok(cm.Responser(1, "Success PUT", leo));
            }
            catch (Exception ex)
            {
                return StatusCode(500, cm.Responser(2, $"Internal server error : {ex}"));
            }
        }

    }
}
