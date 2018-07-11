using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRDraw
{
    public class DrawHub : Hub
    {
        private static ConcurrentBag<Stroke> strokes = new ConcurrentBag<Stroke>();
        public async Task NewStroke(Point start, Point end, string color)
        {
            var task = Clients.Others.SendAsync("newStroke", start, end, color);
            strokes.Add(new Stroke
            {
                Start = start,
                End = end,
                Color = color
            });
            await task;
        }

        public async Task NewStrokes(IEnumerable<Stroke> newStrokes)
        {
            foreach (var s in newStrokes)
            {
                strokes.Add(s);
            }
            var tasks = newStrokes.Select(
                s => Clients.Others.SendAsync("newStroke", s.Start, s.End, s.Color));
            await Task.WhenAll(tasks);
        }

        public async Task ClearCanvas()
        {
            var task = Clients.Others.SendAsync("clearCanvas");
            strokes.Clear();
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