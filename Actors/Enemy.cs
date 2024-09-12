using OpenTK;
using System;

namespace Bulletz
{
    class Enemy : Actor
    {
        public float TimeToNextShoot;
        private float visionRay;
        private float attackRay;
        public float ChaseSpeed;
        private StateMachine stateMachine;

        public Enemy() : base("actor", new Vector2(2, 0), 2)
        {
            Sprite.SetAdditiveTint(200, 20, 20, 1);

            CollisionDmg = 25;
            visionRay = 5;
            attackRay = 3;
            ChaseSpeed = 3;
            TimeToNextShoot = 0;
            shootSpeed = new Vector2(10);

            bulletType = BulletType.EnemyBullet;
            RigidBody.Type = RigidBodyType.Enemy;

            RigidBody.AddCollisionType(RigidBodyType.Player | RigidBodyType.PlayerBullet);

            stateMachine = new StateMachine();

            stateMachine.AddState(StateType.Patrol, new WalkState(this, stateMachine));
            stateMachine.AddState(StateType.Chase, new ChasingState(this, stateMachine));
            stateMachine.AddState(StateType.Attack, new ShootState(this, stateMachine));

            stateMachine.GoTo(StateType.Patrol);
        }

        public void ShootPlayer()
        {
            if (IsActive)
            {
                TimeToNextShoot -= Game.DeltaTime;

                if (TimeToNextShoot <= 0)
                {
                    Player player = ((PlayScene)Game.Scene).Player;
                    Vector2 dir = (player.Position - Position).Normalized();
                    TimeToNextShoot = RandomGenerator.GetRandomFloat() * 0.5f + 0.5f;

                    Shoot(dir);
                }
            }
        }

        public bool CanSee(Player player)
        {
            Vector2 distToPlayer = player.Position - Position;

            return distToPlayer.LengthSquared <= visionRay * visionRay;
        }

        public bool CanShoot(Player player)
        {
            Vector2 distToPlayer = player.Position - Position;

            return distToPlayer.LengthSquared <= attackRay * attackRay || (distToPlayer.X >= -1 && distToPlayer.X <= 1);
        }

        public void OnChase(Player player)
        {
            Vector2 dir = player.Position - Position;

            RigidBody.CurrentMoveSpeed.X = ChaseSpeed * Math.Sign(dir.X);
        }

        public override void Update()
        {
            if (IsActive)
            {
                base.Update();
                stateMachine.Update();
            }
        }

        public override void OnDie()
        {
            EnemyMngr.RestoreEnemy(this);
        }
    }
}
