using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RecycleMeMVC.Models;
namespace RecycleMeMVC.Controllers
{
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/
        public ActionResult Dashboard()
        {
            ViewBag.UserId = User.Identity.GetUserId();
            return View();
        }
    }
}