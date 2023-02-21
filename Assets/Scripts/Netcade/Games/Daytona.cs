using Netcade.Interfaces;
using UnityEngine.Windows.Speech;

namespace Netcade.Games
{
    public class Daytona : Game
    {
        public new string Name => "Daytona USA";
        public new string IName => "daytona";
        public new GameSystem System => new Systems.Model2();

        public override void PrepareGame(int position)
        {
            // TODO: modify save files and such
        }
    }
}