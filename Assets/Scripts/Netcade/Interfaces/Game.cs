namespace Netcade.Interfaces
{
    public abstract class Game
    {
        public string Name;
        public string IName;
        public GameSystem System;

        public void StartGame(int position)
        {
            PrepareGame(position);
            System.Start(IName);
        }

        public abstract void PrepareGame(int position);
    }
}