using Netcade.Interfaces;

namespace Netcade.Objects
{
    public class Game
    {
        public int Id;
        public string Name;
        public GameSystem System;
        public Netcade.Interfaces.Game GameName;
        public User Owner;
        public int[] Users; // not a User because lots of these are made, it would be better to fetch later. i think
    }
}