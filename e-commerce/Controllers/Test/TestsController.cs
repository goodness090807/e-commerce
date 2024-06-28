using e_commerce.Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers.Test
{
    public class TestsController : BaseController
    {
        private readonly ILogger<TestsController> _logger;
        private readonly IHub _sentryHub;

        public TestsController(ILogger<TestsController> logger, IHub sentryHub)
        {
            _logger = logger;
            _sentryHub = sentryHub;
        }

        [HttpGet]
        public IActionResult Test()
        {
            _logger.LogInformation("送出Information");
            _sentryHub.AddBreadcrumb("送出麵包屑資料", category: "test", level: BreadcrumbLevel.Info);
            _sentryHub.CaptureMessage("抓資料");

            return Ok("送出Log訊號");
        }

        [HttpGet("key")]
        public IActionResult GetKey()
        {
            return Ok(KeyGenerator.GenerateRandomKey());
        }
    }
}
