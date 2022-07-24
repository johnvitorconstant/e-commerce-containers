using ECC.Client.API.Application.Commands;
using ECC.Core.Mediator;
using ECC.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECC.Client.API.Controllers;

public class ClientsController : MainController
{

    private readonly IMediatorHandler _mediatorHandler;

    public ClientsController(IMediatorHandler mediatorHandler)
    {
        _mediatorHandler = mediatorHandler;
    }

    [AllowAnonymous]
    [HttpGet("api/clients/")]
    public async Task<IActionResult>  Index()
    {
        var result =  await _mediatorHandler.SendCommand((new RegisterClientCommand(Guid.NewGuid(), "John", "john1@gmail.com",
            "30849593077")));

   

        return CustomResponse(result);
    }

}