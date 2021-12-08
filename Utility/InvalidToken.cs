using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace TodoApi.Utility
{
    public class InvalidTokenAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var Authorization = context.HttpContext.Request.Headers["request-key"];

            if (Authorization != "12345678910")
            {
                context.ModelState.AddModelError("Authorization", "Authorization failed!");
                
                //Add this code to Interrupt request, so request will not get into controller
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("After!");
        }
    }
}