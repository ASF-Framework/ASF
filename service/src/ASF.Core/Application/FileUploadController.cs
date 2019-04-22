using ASF.Application.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ASF.Application
{
    [Route("[controller]/[action]")]
    public class FileUploadController : Controller
    {
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <param name="env">环境变量</param>
        /// <param name="dto">图片对象</param>
        /// <returns></returns>
        [HttpPost]
        public Result<string> Image([FromServices]IHostingEnvironment env, [FromForm] FileUploadImageRequestDto dto)
        {
            //验证请求数据合法性
            var result = dto.Valid();
            if (!result.Success)
                return Result<string>.ReFailure(result);

            var fileExtensions = Path.GetExtension(dto.Image.FileName).ToLowerInvariant();
            var fileName = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N") + fileExtensions;
            var filePath = Path.Combine(env.WebRootPath != null ? env.WebRootPath : "wwwroot", "upload" );

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            using (var stream = new FileStream(Path.Combine(filePath, fileName), FileMode.CreateNew))
            {
                dto.Image.CopyTo(stream);
            }
            return Result<string>.ReSuccess("upload/"+fileName);
        }
    }
}
