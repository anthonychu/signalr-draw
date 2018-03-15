using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRDraw
{
    public class DrawHub : Hub
    {
        public async Task NewStroke(Point start, Point end, string color)
        {
            await Clients.Others.SendAsync("newStroke", start, end, color);
        }

        public async Task ClearCanvas()
        {
            await Clients.Others.SendAsync("clearCanvas");
        }
    }
}