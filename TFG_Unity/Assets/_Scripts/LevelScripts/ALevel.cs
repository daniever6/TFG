using _Scripts.Utilities.Command;
using Command;
using Utilities;

namespace _Scripts.LevelScripts
{
    public class ALevel : Singleton<ALevel>, ILevel
    {
        public static ICommand PerformCombinationCommand;

        protected virtual void Start()
        {
            PerformCombinationCommand = new CombinationCommand(this);
        }

        public virtual bool PerformCombination(string combination)
        {
            return true;
        }
        
        public virtual void PostPerformCombination(){}

    }
}