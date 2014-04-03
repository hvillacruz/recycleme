using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using RecycleMeMVC.Models;
namespace RecycleMeMVC.Controllers
{
    [RoutePrefix("Profile")]
    public class ProfileController : Controller
    {
        //
        // GET: /Profile/
        [Route("Dashboard/{id?}")]
        public ActionResult Dashboard(string id)
        {

            ViewBag.UserId = id == null ? User.Identity.GetUserId() : id;
            return View();
        }


    }
}