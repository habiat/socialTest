using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Social.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Social.Service.User;

namespace Social.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region Fields
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        #endregion

        #region Ctor
        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }
        #endregion


        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [Route("search")]
        public IActionResult Search(string search = "", int page = 1)
        {
            var model = _userService.Search(search, page);
            return View(model);
        }
        [Route("profile/{id:guid}")]
        public IActionResult Profile(Guid id)
        {
            var model = _userService.UserGetByGuid(id, User.GetUserId());
            if (model == null)
                return RedirectToAction("Search");
            return View(model);
        }
        [HttpPost]
        [Route("requestFriend")]
        public IActionResult RequestFriend(string uId)
        {
            _userService.RequestFriendAdd(User.GetUserId(), new Guid(uId));
            return Json(new { Result = uId });
        }
        [HttpPost]
        [Route("cancelRequest")]
        public IActionResult CancelRequest(string uId)
        {
            _userService.RequestFriendRemove(User.GetUserId(), new Guid(uId));
            return Json(new { Result = uId });
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
