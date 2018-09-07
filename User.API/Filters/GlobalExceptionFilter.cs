using Microsoft.AspNetCore.Mvc.Filters;

namespace User.API.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}