using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using upload_task.Models;

namespace upload_task.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private Common cm = new Common();
        SettingsModel mod = new SettingsModel();
        private ObjectResult RESPONSE;

        public SettingsController(ILogger<SettingsController> logger)
        {
            _logger = logger;
        }

        [HttpGet, DisableRequestSizeLimit]
        public async Task<IActionResult> GetSettings()
        {
            try
            {
                var data = mod.SettingsRead();
                RESPONSE = StatusCode(200, cm.Responser(S.SUCCESS, "Success", data));
            }
            catch (Exception ex)
            {
                await cm.LogWriter(S.ERROR, "Controller.GetSettings", ex.Message);
                RESPONSE = StatusCode(500, cm.Responser(S.ERROR, $"Internal server error : {ex}"));
            }

            return RESPONSE;
        }

        [HttpPatch, DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateSetting([FromBody] Settings data)
        {
            try
            {
                var resp = mod.SettingsUpdate(data);
                RESPONSE = StatusCode(200, cm.Responser(S.SUCCESS, "Settings Updated Successfully", resp));
            }
            catch (Exception ex)
            {
                await cm.LogWriter(S.ERROR, "Controller.UpdateSetting", ex.Message);
                RESPONSE = StatusCode(500, cm.Responser(S.ERROR, $"Internal server error : {ex}"));
            }

            return RESPONSE;
        }
    }
}
