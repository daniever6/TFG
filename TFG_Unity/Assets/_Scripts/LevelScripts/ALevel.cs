using Command;

namespace _Scripts.LevelScripts
{
    public class ALevel : Utilities.Singleton<ALevel>, ILevel
    {
        public static ICommand PerformCombinationCommand;

        private void Start()
        {
            PerformCombinationCommand = new CombinationCommand(this);
        }

        public virtual bool PerformCombination(string combination)
        {
            return true;
        }

    }
}