using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SignalRDraw.Controllers
{
    [Route("api/[controller]")]
    public class ResetController : Controller
    {
        private readonly IHubContext<DrawHub> context;

        public ResetController(IHubContext<DrawHub> context)
        {
            this.context = context;
        }

        [HttpPost]
        public Task Post()
        {
            DrawHub.ClearAll();
            return context.Clients.All.SendAsync("clearCanvas");
        }
    }
}