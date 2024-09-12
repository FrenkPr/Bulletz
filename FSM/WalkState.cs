using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Bulletz
{
    class WalkState : State
    {
        private Enemy owner;

        public WalkState(Enemy enemy, StateMachine fsm) : base(fsm)
        {
            owner = enemy;
        }

        public override void OnEnter()
        {
            owner.RigidBody.CurrentMoveSpeed.X = owner.RigidBody.MoveSpeed.X;
        }

        public override void Update()
        {
            Player player = ((PlayScene)Game.Scene).Player;

            if (owner.CanSee(player))
            {
                fsm.GoTo(StateType.Chase);
            }
        }
    }
}
