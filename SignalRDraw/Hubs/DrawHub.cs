using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRDraw
{
    public class DrawHub : Hub
    {
        public async Task NewStroke(Point start, Point end)
        {
            await Clients.Others.SendAsync("newStroke", start, end);
        }

        public async Task ClearCanvas()
        {
            await Clients.Others.SendAsync("clearCanvas");
        }
    }
}