using UnityEngine;

namespace Command
{
    public class MoveCommand : ACommand
    {
        public MoveCommand(PlayerMovement playerMovement) : base(playerMovement){}
        public override void Execute()
        {
            _playerMovement.Move(Vector2.zero);
        }

        public override void Execute(object data)
        {
            _playerMovement.Move((Vector2)data);
        }
    }
}