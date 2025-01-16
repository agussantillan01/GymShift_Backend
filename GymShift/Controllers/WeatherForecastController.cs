using Microsoft.AspNetCore.Mvc;

namespace GymShift.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        [HttpGet]
        public IActionResult ObtenerDatos()
        {
            return Ok(new { mensaje = "Hola Mundo" });
        }
    }
}
