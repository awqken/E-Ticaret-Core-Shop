using CoreShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CoreShop.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Target of <c>UseExceptionHandler</c>: shown for unhandled exceptions.
        /// The exception itself is already logged by the framework's diagnostics middleware.
        /// </summary>
        [Route("Error")]
        public IActionResult Index()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                _logger.LogError("Unhandled exception while processing {Path}", exceptionFeature.Path);
            }

            Response.StatusCode = StatusCodes.Status500InternalServerError;

            return View("Index", new ErrorPageVM
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Title = "Bir şeyler ters gitti",
                Message = "Beklenmedik bir hata oluştu. Lütfen daha sonra tekrar deneyin.",
                IconCssClass = "fa-solid fa-triangle-exclamation"
            });
        }

        /// <summary>
        /// Target of <c>UseStatusCodePagesWithReExecute</c>: shown for error status
        /// codes that produced no body (404, 403, 405, ...).
        /// </summary>
        [Route("Error/{statusCode:int}")]
        public IActionResult HandleStatusCode(int statusCode)
        {
            var reExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (reExecuteFeature != null)
            {
                var originalUrl = reExecuteFeature.OriginalPath + reExecuteFeature.OriginalQueryString;
                _logger.LogWarning("Status code {StatusCode} for {OriginalUrl}", statusCode, originalUrl);
            }

            Response.StatusCode = statusCode;

            var model = statusCode switch
            {
                StatusCodes.Status404NotFound => new ErrorPageVM
                {
                    StatusCode = statusCode,
                    Title = "Sayfa bulunamadı",
                    Message = "Aradığınız sayfa taşınmış veya hiç var olmamış olabilir.",
                    IconCssClass = "fa-solid fa-magnifying-glass"
                },
                StatusCodes.Status403Forbidden => new ErrorPageVM
                {
                    StatusCode = statusCode,
                    Title = "Erişim engellendi",
                    Message = "Bu sayfayı görüntülemek için yetkiniz bulunmuyor.",
                    IconCssClass = "fa-solid fa-lock"
                },
                _ => new ErrorPageVM
                {
                    StatusCode = statusCode,
                    Title = "İstek işlenemedi",
                    Message = "İsteğiniz gerçekleştirilemedi. Lütfen tekrar deneyin.",
                    IconCssClass = "fa-solid fa-circle-exclamation"
                }
            };

            return View("Index", model);
        }
    }
}
