using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using upload_task.Models;
using Microsoft.AspNetCore.Http;

namespace upload_task.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]

    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;
        private int END = S.SUCCESS;
        private Common cm = new Common();
        UploadModel mod = new UploadModel();
        private ObjectResult RESPONSE;

        public UploadController(ILogger<UploadController> logger)
        {
            _logger = logger;
        }


        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> UploadInsert()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                int cnt = httpRequest.Form.Files.Count;

                if (cnt > 0)
                {
                    END = mod.UploadInsertFS(Request);
                    if (END == S.ERROR)
                    {
                        RESPONSE = StatusCode(500, cm.Responser(S.ERROR, "Internal Error"));
                    }
                    else
                    {
                        RESPONSE = StatusCode(200, cm.Responser(S.SUCCESS, $"{END} Record Inserted Successfully in FileSystem"));
                    }
                }
                else
                {
                    RESPONSE = StatusCode(400, cm.Responser(S.ERROR, "Bad Request"));
                }
            }
            catch (Exception ex)
            {
                await cm.LogWriter(S.ERROR, "Controller.UploadInsert", ex.Message);
                RESPONSE = StatusCode(500, cm.Responser(S.ERROR, "Internal Error"));
            }

            return RESPONSE;
        }


        [HttpGet("{id}"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadView([FromRoute] string id)
        {
            try
            {
                var resp = mod.UploadView(id).Result;
                RESPONSE = StatusCode(200, cm.Responser(S.SUCCESS, $"Successfully", resp));
            }
            catch (Exception ex)
            {
                await cm.LogWriter(S.ERROR, "Controller.UploadView", ex.Message);
                RESPONSE = StatusCode(500, cm.Responser(S.ERROR, "Internal Error"));
            }

            return RESPONSE;
        }


        [HttpGet, DisableRequestSizeLimit]
        public async Task<IActionResult> UploadList()
        {
            
            try
            {
                var page = int.Parse(HttpContext.Request.Query["page"]);
                var pageSize = int.Parse(HttpContext.Request.Query["pageSize"]);

                var resp = mod.UploadList(page, pageSize);
                RESPONSE = StatusCode(200, cm.Responser(S.SUCCESS, $"", resp.Result));
            }
            catch (Exception ex)
            {
                await cm.LogWriter(S.ERROR, "Controller.UploadList", ex.Message);
                RESPONSE = StatusCode(500, cm.Responser(S.ERROR, "Internal Error"));
            }

            return RESPONSE;
        }


        [HttpDelete("{src}/{id}")]
        public async Task<IActionResult> UploadDelete([FromRoute] string src, string id)
        {
            try
            {
                var END = await mod.UploadDelete(src, id);
                
                if (END == 0)
                {
                    RESPONSE = StatusCode(200, cm.Responser(S.NOTFOUND, "Record Not Found"));
                }
                else
                {
                    RESPONSE = StatusCode(200, cm.Responser(S.SUCCESS, $"{END} Record Deleted Successfully"));
                }
            }
            catch (Exception ex)
            {
                await cm.LogWriter(S.ERROR, "Controller.UploadDelete", ex.Message);
                RESPONSE = StatusCode(500, cm.Responser(S.ERROR, "Internal Error"));
            }

            return RESPONSE;
        }
    }
}
