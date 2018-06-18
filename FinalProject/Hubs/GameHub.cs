using GameDemo.GameObjects;
using GameDemo.Users;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameDemo.Hubs
{
    public class GameHub : Hub
    {
        public static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        public static List<string> RoomManager = new List<string>();
        public static List<UserDetail> Spectators = new List<UserDetail>();

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Disconnected", $"{Context.ConnectionId} left");
            await base.OnDisconnectedAsync(exception);
        }
        

        // called when player connects to hub
        public async Task Connect(string userName)
        {
            var id = Context.ConnectionId;
            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                ConnectedUsers.Add(new UserDetail { ConnectionId = id, UserName = userName });
                await Clients.Caller.SendAsync("Connected", "Connected...!");
                await Clients.All.SendAsync("Connection", userName);
            }
        }

        // called when sharing game render for others
        public async Task ShareBoard(int [,] cells)
        {
            await Clients.AllExcept(new string[] { Context.ConnectionId }).SendAsync("SharedBoard", cells);
        }

        // broadcast the game to a player
        public async Task SendBoard(int [,] cells)
        {
            await Clients.Caller.SendAsync("SendBoard", cells);
        }

        // called when a player want to play game
        public async Task GetBoard()
        {
            var board = new Board();
            await SendBoard(board.cells);
        }


    }
}
