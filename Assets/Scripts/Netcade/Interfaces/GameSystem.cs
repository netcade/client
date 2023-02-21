namespace Netcade.Interfaces
{
    public abstract class GameSystem
    {
        public abstract string Name { get; }
        public abstract string EmuName { get; }
        public abstract string SystemName { get; }
        public abstract string Download { get; }

        public virtual void Start(string romName)
        {
            
        }
    }
}