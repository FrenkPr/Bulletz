using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulletz
{
    class ChasingState : State
    {
        private Enemy owner;

        public ChasingState(Enemy enemy, StateMachine fsm) : base(fsm)
        {
            owner = enemy;
        }

        public override void Update()
        {
            Player player = ((PlayScene)Game.Scene).Player;

            if (owner.CanShoot(player))
            {
                fsm.GoTo(StateType.Attack);
            }

            else if (owner.CanSee(player))
            {
                owner.OnChase(player);
            }

            else
            {
                fsm.GoTo(StateType.Patrol);
            }
        }
    }
}
