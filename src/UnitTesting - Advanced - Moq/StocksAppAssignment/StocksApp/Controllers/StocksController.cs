using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace StocksApp.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IStocksService _stocksService;
        private readonly IFinnhubService _finnhubService;

        public StocksController(IStocksService stocksService, IFinnhubService finnhubService)
        {
            _stocksService = stocksService;
            _finnhubService = finnhubService;
        }

        [Route("[action]")]
        [Route("/")]
        [HttpGet]
        public async Task<IActionResult> Explore()
        {
            List<Dictionary<string, string>>? objects = await _finnhubService.GetStocks();
            
            return View(objects);   
        }
    }
}
