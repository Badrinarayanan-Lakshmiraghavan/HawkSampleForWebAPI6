using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;

namespace HawkAuthAspNet5.Controllers.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Hello, " + User.Identity.Name;
        }

        [HttpPost]
        public string Post([FromBody]string name)
        {
            return String.Format("Hello, {0}. Thanks for flying Hawk", name);
        }
    }
}