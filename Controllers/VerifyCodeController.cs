using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Wee_XYYY.Extensions;
using SixLaborsCaptcha.Core;

namespace Wee_XYYY.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class VerifyCodeController : ControllerBase
{
    private readonly ILogger<VerifyCodeController> _logger;
    private ISixLaborsCaptchaModule _six;
    public VerifyCodeController(ILogger<VerifyCodeController> logger, ISixLaborsCaptchaModule six)
    {
        _logger = logger;
        _six = six;
    }

    [HttpGet("{key}")]
    public FileResult Get([BindRequired] string key)
    {
        // generate code
        Random random2 = new Random();
        int num = random2.Next(100000, 999999);
        string code = num.ToString();

        // save redis
        new RedisCache().Add("vc_" + key, code, 300);

        // create image
        var image = _six.Generate(code);

        return File(image, "Image/Png");
    }
}
