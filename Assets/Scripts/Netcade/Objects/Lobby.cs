using Netcade.Games;
using Netcade.Interfaces;
using Netcade.SocketObjects;
using Netcade.Systems;

namespace Netcade.Objects
{
    public class Lobby
    {
        public int Id;
        public string Name;
        public GameSystem System;
        public Netcade.Interfaces.Game GameName;
        public User Owner;
        public int[] Users; // not a User because lots of these are made, it would be better to fetch later. i think

        public Lobby(SOLobby lobby)
        {
            this.Id = lobby.id;
            this.Name = lobby.name;
            this.System = new Model2(); // TODO: not hardcode this
            this.GameName = new Daytona();
            {
                this.Owner = new User(); // TODO: UserFromSO
                this.Owner.Username = lobby.owner.username;
            }
            this.Users = lobby.users;
        }
    }
}