using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ASF.Application.DTO
{
    public class FileUploadImageRequestDto : IDto
    {
        [Required]
        public IFormFile Image { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,gif,png", ErrorMessage = "图片格式错误")]
        public string FileName
        {
            get
            {
                return this.Image.FileName;
            }
        }
    }
}
