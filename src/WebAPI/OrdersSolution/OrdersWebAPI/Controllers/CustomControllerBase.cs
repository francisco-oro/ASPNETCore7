using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersWebAPI.DbContext;

namespace OrdersWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomControllerBase : ControllerBase
    {
        protected readonly ApplicationDbContext _context;

        public CustomControllerBase(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
