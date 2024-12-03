using DialogFlowAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UWorldSupportBot.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {
        private readonly DialogFlowAPIService _dialogFlowAPIService;

        public BotController(DialogFlowAPIService dialogFlowAPIService)
        {
            _dialogFlowAPIService = dialogFlowAPIService;

        }
        [HttpPost]
        public async Task<string> Get([FromBody] RequestModel model)
        {
            return await _dialogFlowAPIService.GetResultAsync("adsfsfsd", model.Query);
        }
    }

    public class RequestModel
    {
        public string Query { get; set; }
    }
}
