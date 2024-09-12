using Aiv.Fast2D;
using OpenTK;

namespace Bulletz
{
    class Player : Actor
    {
        //private float timeToNextBullet;
        private bool isFirePressed;
        private bool isJumpPressed;
        public static long Score;
        private float moveSpeed;
        private float jumpSpeed;
        private int id;
        public bool IsGrounded { get { return RigidBody.CurrentMoveSpeed.Y == 0; } }

        public Player(int id) : base("actor", Vector2.Zero, 2)
        {
            this.id = id;

            Position = new Vector2(Game.OrthoHalfWidth, Game.OrthoHalfHeight);
            EnergyBar.Position = Vector2.One;

            moveSpeed = 5;
            jumpSpeed = -11;
            shootSpeed = new Vector2(10);

            //timeToNextBullet = 0;
            IsActive = true;
            EnergyBar.IsActive = true;

            EnergyBar.Sprite.Camera = CameraMngr.GetCamera("GUI");
            EnergyBar.BarSprite.Camera = EnergyBar.Sprite.Camera;

            bulletType = BulletType.PlayerBullet;
            RigidBody.Type = RigidBodyType.Player;
            RigidBody.AddCollisionType(RigidBodyType.Enemy | RigidBodyType.EnemyBullet | RigidBodyType.Earth);

            Score = 0;

            //DebugMngr.AddItem(RigidBody.Collider);

            //TextMngr.AddPhraseAt("score", "SCORE:" + Score.ToString("0000000000"), Vector2.Zero);
        }

        private bool IsJoypadFirePressed()
        {
            if (Game.JoypadCtrls.Count != 0)
            {
                if (Game.JoypadCtrls[id].IsFirePressed())
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsJoypadJumpPressed()
        {
            if (Game.JoypadCtrls.Count != 0)
            {
                if (Game.JoypadCtrls[id].IsJumpPressed())
                {
                    return true;
                }
            }

            return false;
        }

        public void Shoot()
        {
            //SHOOT INPUT
            //timeToNextBullet -= Game.DeltaTime;

            //if ((Game.KeyboardCtrl.IsFirePressed() || IsJoypadFirePressed()) && timeToNextBullet <= 0)
            //{
            //    base.Shoot();
            //    timeToNextBullet = 0.6f;
            //}

            if (Game.KeyboardCtrl.IsFirePressed() || IsJoypadFirePressed())
            {
                if (!isFirePressed)
                {
                    Vector2 mouseAbsolutePosition = CameraMngr.MainCamera.position - CameraMngr.MainCamera.pivot;
                    Vector2 dir = (mouseAbsolutePosition + Game.Window.MousePosition - Position).Normalized();

                    Shoot(dir);
                    isFirePressed = true;
                    CurrentFrame = 1;
                }
            }

            else if (isFirePressed)
            {
                isFirePressed = false;
                CurrentFrame = 0;
            }
            //END SHOOT INPUT
        }

        private void Jump()
        {
            if (Game.KeyboardCtrl.IsJumpPressed() || IsJoypadJumpPressed())
            {
                if (!isJumpPressed)
                {
                    isJumpPressed = true;

                    if (IsGrounded)
                    {
                        RigidBody.CurrentMoveSpeed.Y = jumpSpeed;
                    }
                }
            }
            else if (isJumpPressed)
            {
                isJumpPressed = false;
            }
        }

        public void KeyboardInput()
        {
            //MOVE INPUT
            float dirX = Game.KeyboardCtrl.GetHorizontal();

            RigidBody.CurrentMoveSpeed.X = moveSpeed * dirX;
            //END MOVE INPUT

            //JUMP
            Jump();

            //SHOOT
            Shoot();
        }

        public void JoypadInput()
        {
            if (Game.JoypadCtrls.Count == 0)
            {
                return;
            }

            //MOVE INPUT
            float dirX = Game.JoypadCtrls[id].GetHorizontal();

            RigidBody.CurrentMoveSpeed.X = moveSpeed * dirX;
            //END MOVE INPUT

            //JUMP
            Jump();

            //SHOOT
            Shoot();
        }

        protected override void CheckOutOfScreen()
        {
            //horizontal collisions
            if (Position.X - HalfWidth < CameraMngr.CameraLimits.MinX - Game.OrthoHalfWidth)
            {
                Sprite.position.X = CameraMngr.CameraLimits.MinX - Game.OrthoHalfWidth + HalfWidth;
            }

            else if (Position.X + HalfWidth > CameraMngr.CameraLimits.MaxX + Game.OrthoHalfWidth)
            {
                Sprite.position.X = CameraMngr.CameraLimits.MaxX + Game.OrthoHalfWidth - HalfWidth;
            }

            //vertical collisions
            if (Position.Y - HalfHeight < CameraMngr.CameraLimits.MinY - Game.OrthoHalfHeight)
            {
                Sprite.position.Y = CameraMngr.CameraLimits.MinY - Game.OrthoHalfHeight + HalfHeight;

                if (RigidBody.IsGravityAffected && RigidBody.CurrentMoveSpeed.Y < 0)
                {
                    RigidBody.CurrentMoveSpeed.Y *= -1;
                }
            }
        }

        public override void OnDie()
        {
            IsActive = false;
            EnergyBar.IsActive = false;
        }

        public override void OnCollision(CollisionInfo collisionInfo)
        {
            base.OnCollision(collisionInfo);

            if (collisionInfo.Collider is EnemyBullet bullet)
            {
                AddDamage(bullet.Dmg);
                BulletMngr.RestoreBullet(bullet);
            }

            else if (collisionInfo.Collider is Enemy enemy)
            {
                AddDamage(enemy.CollisionDmg);
                enemy.OnDie();
            }

            if (!IsAlive)
            {
                OnDie();
            }
        }

        //public override void Update()
        //{
        //    base.Update();

        //    System.Console.WriteLine("Player position: " + Position);
        //}
    }
}
