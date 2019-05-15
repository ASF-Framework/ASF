using ASF.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ASF.Application
{
    [Route("[controller]/[action]")]
    public class FileUploadController : Controller
    {
        private readonly IHostingEnvironment env;
        public FileUploadController(IHostingEnvironment hostingEnvironment)
        {
            this.env = hostingEnvironment;
        }
        private string StorageRoot
        {
            get
            {
                string storageRoot = Path.Combine(env.WebRootPath != null ? env.WebRootPath : "wwwroot", "upload");
                if (!Directory.Exists(storageRoot))
                {
                    Directory.CreateDirectory(storageRoot);
                }
                return storageRoot;
            }
        }


        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpGet]
        public void Delete(string id)
        {
            var filename = id;
            var filePath = Path.Combine(this.StorageRoot, filename);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpGet]
        public IActionResult Download(string id)
        {
            var filename = id;
            var filePath = Path.Combine(this.StorageRoot, filename);

            var context = HttpContext;

            if (System.IO.File.Exists(filePath))
            {
                context.Response.Headers.Add("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                return File(filePath, "application/octet-stream");
            }
            else
            {
                context.Response.StatusCode = 404;
                return new JsonResult("");
            }

        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpPost]
        public async Task<JsonResult> UploadFiles()
        {
            var statuses = new List<ViewDataUploadFilesResult>();

            foreach (IFormFile file in Request.Form.Files.ToList())
            {

                var headers = Request.Headers;

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                {
                    await UploadWholeFile(Request, statuses);
                }
                else
                {
                    await UploadPartialFile(headers["X-File-Name"], Request, statuses);
                }
            }
            return Json(statuses);
        }



        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private async Task UploadPartialFile(string fileName, HttpRequest request, List<ViewDataUploadFilesResult> statuses)
        {
            if (request.Form.Files.Count != 1)
                throw new ValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Form.Files[0];
            var inputStream = file.OpenReadStream();
            var fullPath = Path.Combine(StorageRoot, Path.GetFileName(fileName));

            using (var fs = new FileStream(fullPath, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    await fs.WriteAsync(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            statuses.Add(new ViewDataUploadFilesResult(fileName, fullPath, file));
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private async Task UploadWholeFile(HttpRequest request, List<ViewDataUploadFilesResult> statuses)
        {
            for (int i = 0; i < request.Form.Files.Count; i++)
            {
                var file = request.Form.Files[i];
                var fileName = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName).ToLowerInvariant();
                var fullPath = Path.Combine(StorageRoot, Path.GetFileName(fileName));

                using (var stream = new FileStream(fullPath, FileMode.CreateNew))
                {
                    await file.CopyToAsync(stream);
                }

                statuses.Add(new ViewDataUploadFilesResult(fileName, fullPath, file));
            }
        }
        public class ViewDataUploadFilesResult
        {
            public ViewDataUploadFilesResult(string fileName, string filePath, IFormFile file)
            {
                this.url = "/FileUpload/Download/" + fileName;
                this.delete_url = "/FileUpload/Delete/" + fileName;
                this.thumbnail_url = @"data:image/png;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(filePath));
                this.name = fileName;
                this.size = file.Length;
                this.type = file.ContentType;
                this.delete_type = "GET";
            }
            /// <summary>
            /// 文件名称
            /// </summary>
            public string name { get; private set; }
            /// <summary>
            /// 文件大小 
            /// </summary>
            public long size { get; private set; }
            /// <summary>
            /// 文件类型
            /// </summary>
            public string type { get; private set; }
            /// <summary>
            /// 文件地址
            /// </summary>
            public string url { get; private set; }
            /// <summary>
            /// 删除文件地址
            /// </summary>
            public string delete_url { get; private set; }
            /// <summary>
            /// 缩略图地址
            /// </summary>
            public string thumbnail_url { get; private set; }
            /// <summary>
            /// 删除请求类型
            /// </summary>
            public string delete_type { get; private set; }
        }
    }
}
