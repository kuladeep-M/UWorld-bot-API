using DialogFlowAPI;
using Microsoft.AspNetCore.Mvc;

namespace UWorldSupportBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {
        private readonly DialogFlowAPIService _dialogFlowAPIService;

        public BotController(DialogFlowAPIService dialogFlowAPIService)
        {
            _dialogFlowAPIService = dialogFlowAPIService;

        }
        [HttpGet]
        public async Task<string> Get()
        {
            return await _dialogFlowAPIService.GetResultAsync("adsfsfsd", "course ?");
        }
    }
}
