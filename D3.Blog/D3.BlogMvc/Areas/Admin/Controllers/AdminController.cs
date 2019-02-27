using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace D3.BlogMvc.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,Admin")]
    [Area("Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About(int? id)
        {
            return new JsonResult("this is admin abour");
        }

    }
}