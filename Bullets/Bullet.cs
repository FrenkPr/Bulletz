using OpenTK;

namespace Bulletz
{
    enum BulletType
    {
        PlayerBullet,
        EnemyBullet,
        Length
    }

    abstract class Bullet : GameObject
    {
        public int Dmg;

        public BulletType Type { get; protected set; }

        public Bullet(string textureName, int width = 0, int height = 0) : base(textureName, 1, width, height, DrawLayer.MiddleGround)
        {
            UpdateMngr.Add(this);
            DrawMngr.Add(this);

            RigidBody = new RigidBody(this, Vector2.Zero);
            RigidBody.Collider = ColliderFactory.CreateCircleFor(this);
            RigidBody.IsGravityAffected = false;
            RigidBody.AddCollisionType(RigidBodyType.Earth);
        }

        protected override void CheckOutOfScreen()
        {
            Vector2 distToOutOfScreen = Position - CameraMngr.MainCamera.position;

            if (IsActive && distToOutOfScreen.LengthSquared > CameraMngr.HalfDiagonalSquared)
            {
                BulletMngr.RestoreBullet(this);
            }
        }

        public void Shoot(Vector2 pos, Vector2 speed)
        {
            Position = pos;
            RigidBody.CurrentMoveSpeed = speed;
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            //on any collision type this bullet will be restored
            BulletMngr.RestoreBullet(this);

            if (collisionInfo.Collider is Bullet)
            {
                BulletMngr.RestoreBullet((Bullet)collisionInfo.Collider);
            }

            else if (this is PlayerBullet && collisionInfo.Collider is Enemy enemy)
            {
                enemy.AddDamage(Dmg);

                if (!enemy.IsAlive && Player.Score < uint.MaxValue)
                {
                    Player.Score += 25;

                    if (Player.Score > uint.MaxValue)
                    {
                        Player.Score = uint.MaxValue;
                    }

                    //PhraseMngr.EditPhrase("score", "SCORE:" + Player.Score.ToString("0000000000"));

                    enemy.OnDie();
                }
            }
        }

        public virtual void Reset()
        {
            IsActive = false;
        }
    }
}
