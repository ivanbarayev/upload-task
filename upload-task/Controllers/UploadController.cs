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
        private HTTPResponser RESPONSE;
        private Common cm = new Common();
        UploadModel mod = new UploadModel();

        public UploadController(ILogger<UploadController> logger)
        {
            _logger = logger;
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<HTTPResponser> Post()
        {
            long cnt = 1;


            if (cnt > 0)
            {
                END = mod.UploadInsertFS(Request);
                if (END == S.ERROR)
                {
                    RESPONSE = cm.Responser(S.ERROR, "Internal Error");
                }
                else
                {
                    RESPONSE = cm.Responser(S.SUCCESS, $"{END} Record Inserted Successfully in FileSystem");
                }
            }
            else
            {
                RESPONSE = cm.Responser(S.ERROR, "Bad Request");
            }

            return RESPONSE;
        }

        [HttpGet("{src:int}/{id:int}"), DisableRequestSizeLimit]
        public async Task<HTTPResponser> View([FromRoute] int src, int id)
        {
            return cm.Responser(1, "Başarılı GET w/param" + id + " src:" + src + " -- " + SettingsENV.destination_path.ToString());
        }

        [HttpGet, DisableRequestSizeLimit]
        public async Task<HTTPResponser> Get()
        {
            return cm.Responser(1, "Başarılı GET" + SettingsENV.destination_path.ToString());
        }


        [HttpDelete("{src:int}/{id}")]
        public async Task<HTTPResponser> Delete([FromRoute] int src, string id)
        {

            var END = mod.UploadDelete(id);
            if (END == -1)
            {
                RESPONSE = cm.Responser(S.ERROR, "Internal Error");
            }
            else if (END == 0)
            {
                RESPONSE = cm.Responser(S.NOTFOUND, "Record Not Found");
            }
            else
            {
                RESPONSE = cm.Responser(S.SUCCESS, $"{END} Record Deleted Successfully");
            }

            return RESPONSE;
        }


    }
}
