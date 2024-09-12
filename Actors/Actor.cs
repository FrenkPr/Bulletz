using System;
using OpenTK;

namespace Bulletz
{
    abstract class Actor : GameObject
    {
        protected int energy;
        protected int maxEnergy;
        public int CollisionDmg;  //the damage it takes to the player in case of collision with enemy
        protected BulletType bulletType;
        protected Vector2 shootSpeed;
        public bool IsAlive { get { return energy > 0; } }
        public ProgressBar EnergyBar;

        public Actor(string textureName, Vector2 moveSpeed, int numFrames = 0, int width = 0, int height = 0) : base(textureName, numFrames, width, height)
        {
            maxEnergy = 100;
            EnergyBar = new ProgressBar("frameProgressBar", "progressBar", new Vector2(Game.PixelsToUnits(4)));
            ResetEnergy();

            RigidBody = new RigidBody(this, moveSpeed);
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            RigidBody.IsGroundAffected = true;

            //DebugMngr.AddItem(RigidBody.Collider);

            UpdateMngr.Add(this);
            DrawMngr.Add(this);
        }

        public void Shoot(Vector2 direction)
        {
            Bullet bullet = BulletMngr.GetBullet(bulletType);

            if (bullet != null)
            {
                bullet.Shoot(Position, shootSpeed * direction);
            }
        }

        public override void Update()
        {
            if (IsActive)
            {
                base.Update();

                if (!(this is Player))
                {
                    EnergyBar.Position = new Vector2(Position.X - EnergyBar.HalfWidth, Position.Y - HalfHeight - EnergyBar.Height - 0.1f);
                }
            }
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            OnWallCollision(collisionInfo);
        }

        protected virtual void OnWallCollision(CollisionInfo collisionInfo)
        {
            if (!(collisionInfo.Collider is EarthGrass))
            {
                return;
            }

            if (collisionInfo.Delta.X < collisionInfo.Delta.Y)
            {
                // Horizontal Collision
                if (X < collisionInfo.Collider.X)
                {
                    // Collision from Left (inverse horizontal delta)
                    collisionInfo.Delta.X = -collisionInfo.Delta.X;
                }

                X += collisionInfo.Delta.X;
                RigidBody.CurrentMoveSpeed.X = 0.0f;
            }

            else
            {
                // Vertical Collision
                if (Y < collisionInfo.Collider.Y)
                {
                    // Collision from Top
                    collisionInfo.Delta.Y = -collisionInfo.Delta.Y;
                    RigidBody.CurrentMoveSpeed.Y = 0.0f;
                }

                else
                {
                    // Collision from Bottom
                    RigidBody.CurrentMoveSpeed.Y = -RigidBody.CurrentMoveSpeed.Y * 0.8f;
                }

                Y += collisionInfo.Delta.Y;
            }
        }

        public virtual void OnDie()
        {

        }

        public void ResetEnergy()
        {
            energy = maxEnergy;
            EnergyBar.Scale((float)energy / (float)maxEnergy);
        }

        public void AddDamage(int dmg)
        {
            energy -= dmg;
            EnergyBar.Scale((float)energy / (float)maxEnergy);
        }
    }
}
