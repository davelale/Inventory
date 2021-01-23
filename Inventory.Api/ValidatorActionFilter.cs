using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Inventory.Api
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using System.Web.Http.ModelBinding;

    namespace MyApi.Filters
    {
        public class ValidatorActionFilter : Microsoft.AspNetCore.Mvc.Filters.IActionFilter
        {
            public void OnActionExecuting(ActionExecutingContext filterContext)
            {
                if (!filterContext.ModelState.IsValid)
                {
                    filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
                }
            }

            public void OnActionExecuted(ActionExecutedContext filterContext)
            {

            }
        }
    }
}
