using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRDraw
{
    public class DrawHub : Hub
    {
        private static List<Stroke> strokes = new List<Stroke>();

        public async Task NewStrokes(IEnumerable<Stroke> newStrokes)
        {
            lock(strokes)
            {
                strokes.AddRange(newStrokes);
            }
            var tasks = newStrokes.Select(
                s => Clients.Others.SendAsync("newStroke", s.Start, s.End, s.Color));
            await Task.WhenAll(tasks);
        }

        public async Task ClearCanvas()
        {
            var task = Clients.Others.SendAsync("clearCanvas");
            lock(strokes)
            {
                strokes.Clear();
            }
            await task;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("clearCanvas");
            var tasks = strokes.Select(s => Clients.Caller.SendAsync("newStroke", s.Start, s.End, s.Color));
            await Task.WhenAll(tasks);
        }
    }
}