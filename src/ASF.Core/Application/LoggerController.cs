using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASF.Application
{
    public class LoggerController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "self")]
        public string Get()
        {
            return "测试成功";
        }
    }
}
