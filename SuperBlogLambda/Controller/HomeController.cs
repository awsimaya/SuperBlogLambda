using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SuperBlogLambda.Controller
{
    public class HomeController: Controller
    {
        public ActionResult Index([FromQuery]string lang)
        {
            return View("Index", GetPosts(lang ?? "EN").Result);
        }
    }
}
