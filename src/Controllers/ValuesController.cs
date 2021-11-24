using System;
using Demo.Models;
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
        var local = Environment.MachineName;

        // Acesando como map
        var message = configuration["MENSAGEM"];       

        var data = configuration.GetSection("Nivel01");
        var data2 = data.GetSection("Nivel01:Default").Value;

        var appSettings = configuration.GetSection(AppSettingsConfiguration.Key).Get<AppSettingsConfiguration>();

        var weblogSettings = configuration.GetSection(WeblogConfiguration.Key).Get<WeblogConfiguration>();

        return new
        {
            Local = local,

            Mensagem1 = message,

            Level1 = configuration["Nivel01:Default"],

            Data1 = data,
            Data2 = data2,

            Default = appSettings,

            Weblog = weblogSettings.ApplicationName,            
        };
    }
}
