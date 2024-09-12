using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulletz
{
    class ShootState : State
    {
        private Enemy owner;

        public ShootState(Enemy enemy, StateMachine fsm) : base(fsm)
        {
            owner = enemy;
        }

        public override void OnEnter()
        {
            owner.RigidBody.CurrentMoveSpeed.X = 0;
            owner.CurrentFrame = 1;
        }

        public override void Update()
        {
            Player player = ((PlayScene)Game.Scene).Player;

            if (owner.CanShoot(player))
            {
                owner.ShootPlayer();
            }

            else
            {
                fsm.GoTo(StateType.Chase);
            }
        }

        public override void OnExit()
        {
            owner.CurrentFrame = 0;
            owner.TimeToNextShoot = 0;
        }
    }
}
