﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Social.Core.Paging;

namespace Social.Web.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            result.LinkTemplate = Url.Action(RouteData.Values["action"].ToString(), new { page = "{0}" });

            return View("Default", result);
        }
    }
}
