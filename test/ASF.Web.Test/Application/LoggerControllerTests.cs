using ASF.Application.DTO;
using ASF.Core.Test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.ComponentModel.DataAnnotations;
namespace ASF.Core.Test.Application
{
   public class LoggerControllerTests: WebHostTest
    {
        [Fact]
        public void Test()
        {
            LoggerDeleteRequestDto dto = new LoggerDeleteRequestDto();
            //验证请求数据合法性
            var result = dto.Valid();
        }
    }
}
