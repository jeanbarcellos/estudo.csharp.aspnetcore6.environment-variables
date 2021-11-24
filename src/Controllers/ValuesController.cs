using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace APIEnvironment.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet]
    public object Get([FromServices]IConfiguration configuration)
    {
        return new
        {
            Local = Environment.MachineName,
            Mensagem = configuration["MENSAGEM"]
        };
    }
}
