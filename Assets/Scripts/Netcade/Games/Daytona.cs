using Netcade.Interfaces;
using UnityEngine.Windows.Speech;

namespace Netcade.Games
{
    public class Daytona : Game
    {
        public string Name = "Daytona USA";
        public string IName = "daytona";
        public GameSystem System = new Systems.Model2();

        public override void PrepareGame(int position)
        {
            // TODO: modify save files and such
        }
    }
}