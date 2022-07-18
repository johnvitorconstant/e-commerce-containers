using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NSE.Identity.API.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {

        protected ICollection<string> Errors = new List<string>();

        protected IActionResult CustomResponse(object result = null)
        {
            if (OperationIsValid()) return Ok(result);

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                {"Messages", Errors.ToArray()},
            }));
        }


        protected IActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach(var error in errors)
            {
                AddProcessError(error.ErrorMessage);
            }
            return CustomResponse();
        }



        protected bool OperationIsValid()
        {
            return !Errors.Any();
        }
        protected void AddProcessError(string error)
        {
            Errors.Add(error);
        }
        protected void ClearProcessError(string error)
        {
            Errors.Clear();
        }
    }
}
