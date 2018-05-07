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
            }
        }

        // called when player click play button
        public async Task CreateRoom(string userName)
        {
            if (RoomManager.Count(x => x == userName) == 0)
            {
                RoomManager.Add(userName);
                await Groups.AddAsync(Context.ConnectionId, userName);
                await Clients.Caller.SendAsync("CreateRoom", "Room Created");
            } else
            {
                await Clients.Caller.SendAsync("CreateRoom", "Room Already Created");
            }
        }


        // called when sharing game render for others
        public async Task ShareBoard(string roomName, Board board)
        {
            await Clients.Group(roomName).SendAsync("SharedBoard", board);
        }

        // called when one player wants to watch other playing
        public async Task Spectate(string roomName, string userName)
        {
            var id = Context.ConnectionId;
            var playerId = ConnectedUsers.Find(x => x.UserName == roomName).ConnectionId;
            if (Spectators.Count(x => x.ConnectionId == id) == 0)
            {
                Spectators.Add(new UserDetail { UserName = userName, ConnectionId = id });
                await Groups.AddAsync(id, roomName);
                await Clients.Client(playerId).SendAsync("Spectator", $"{userName} is watching!");
            }
        }


        // called when a spectator stop watching other playing
        public async Task Leave(string roomName)
        {
            var id = Context.ConnectionId;
            var leave = Spectators.Find(x => x.ConnectionId == id);
            Spectators.Remove(leave);
            await Groups.RemoveAsync(id, roomName);
        }


        // called when one want to challenge other in duo mode
        public async Task SendChallengeMessage(string challenger, string rival)
        {
            if (ConnectedUsers.Count(x => x.UserName == rival) == 0)
            {
                await Clients.Caller.SendAsync("MessageSentFailed", $"{rival} is disconnected");
            } else
            {
                var rivalId = ConnectedUsers.Find(x => x.UserName == rival).ConnectionId;
                var challengerId = Context.ConnectionId;
                await Clients.Client(rivalId).SendAsync("MessageSentSuccessfully",
                    $"{challenger} challenges you in duo mode?", "Accept or Not");
            }
        }

        public async Task ReplyChallenge(string rival, string challenger, string response)
        {
            if (ConnectedUsers.Count(x => x.UserName == rival) == 0)
            {
                await Clients.Caller.SendAsync("MessageSentFailed", $"{rival} is disconnected");
            }
            else
            {
                var rivalId = Context.ConnectionId;
                var challengerId = ConnectedUsers.Find(x => x.UserName == rival).ConnectionId;
                await Clients.Client(challengerId).SendAsync("MessageSentSuccessfully", response);
            }
        }

        // broadcast the game to a player
        public async Task SendBoard(Board board)
        {
            await Clients.Caller.SendAsync("SendBoard", board);
        }

        // called when a player want to play game
        public async Task GetBoard()
        {
            await SendBoard(new Board());
        }


    }
}
