using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRDraw
{
    public class DrawHub : Hub
    {
        private static ConcurrentDictionary<string, List<Stroke>> rooms =
            new ConcurrentDictionary<string, List<Stroke>>();

        public async Task NewStrokes(string roomName, IEnumerable<Stroke> newStrokes)
        {
            var strokes = rooms.GetOrAdd(roomName, _ => new List<Stroke>());
            lock(strokes)
            {
                strokes.AddRange(newStrokes);
            }
            var tasks = newStrokes.Select(
                s => Clients
                    .GroupExcept(roomName, Context.ConnectionId)
                    .SendAsync("newStroke", s.Start, s.End, s.Color));
            await Task.WhenAll(tasks);
        }

        public async Task ClearCanvas(string roomName)
        {
            var strokes = rooms.GetOrAdd(roomName, _ => new List<Stroke>());
            var task = Clients.Group(roomName).SendAsync("clearCanvas");
            lock(strokes)
            {
                strokes.Clear();
            }
            await task;
        }

        internal static void ClearAll()
        {
            rooms.Clear();
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            await Clients.Caller.SendAsync("clearCanvas");

            var strokes = rooms.GetOrAdd(roomName, _ => new List<Stroke>());
            var tasks = strokes.Select(s => Clients.Caller.SendAsync("newStroke", s.Start, s.End, s.Color));
            await Task.WhenAll(tasks);
        }
    }
}