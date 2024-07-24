using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LabSession5.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HelloController : ControllerBase
{
    private readonly IStringLocalizer<HelloController> _localizer;

    public HelloController(IStringLocalizer<HelloController> localizer)
    {
        _localizer = localizer;
    }

    [HttpGet]
    public IActionResult Get([FromQuery] string culture)
    {
        var supportedCultures = new[] { "en", "fr", "de" };

        if (Array.Exists(supportedCultures, c => c.Equals(culture, StringComparison.OrdinalIgnoreCase)))
        {
            var cultureInfo = new CultureInfo(culture);
            CultureInfo.CurrentCulture = cultureInfo;
            CultureInfo.CurrentUICulture = cultureInfo;
        }
        else
        {
            return BadRequest("Unsupported culture");
        }

        var localizedString = _localizer["Hello"].Value;
        return Ok(localizedString);
    }
}