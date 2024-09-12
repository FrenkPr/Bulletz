using Aiv.Fast2D;
using OpenTK;

namespace Bulletz
{
    class GameObject : IDrawable, IUpdatable
    {
        protected Texture texture;
        public Sprite Sprite { get; protected set; }
        public RigidBody RigidBody;
        public bool IsActive;
        public virtual Vector2 Position { get { return Sprite.position; } set { Sprite.position = value; } }
        public virtual float X { get { return Sprite.position.X; } set { Sprite.position.X = value; } }
        public virtual float Y { get { return Sprite.position.Y; } set { Sprite.position.Y = value; } }
        public float Width { get { return Sprite.Width; } }
        public float Height { get { return Sprite.Height; } }
        public float HalfWidth { get { return Width * 0.5f; } }
        public float HalfHeight { get { return Height * 0.5f; } }
        public DrawLayer DrawLayer { get; protected set; }
        protected int numFrames;
        public int CurrentFrame;
        private float pixelsWidth;
        public float pixelsHeight;

        public GameObject(string textureId, int numFrames = 1, float spriteWidth = 0, float spriteHeight = 0, DrawLayer layer=DrawLayer.Playground)
        {
            DrawLayer = layer;

            texture = TextureMngr.GetTexture(textureId);

            this.numFrames = numFrames;
            CurrentFrame = 0;

            spriteWidth = spriteWidth <= 0 ? texture.Width : spriteWidth;
            spriteHeight = spriteHeight <= 0 ? texture.Height : spriteHeight;

            if (numFrames > 1)
            {
                spriteWidth /= numFrames;
            }

            pixelsWidth = spriteWidth;
            pixelsHeight = spriteHeight;

            spriteWidth = Game.PixelsToUnits(spriteWidth);
            spriteHeight = Game.PixelsToUnits(spriteHeight);

            Sprite = new Sprite(spriteWidth, spriteHeight);

            Sprite.pivot = new Vector2(spriteWidth * 0.5f, spriteHeight * 0.5f);
        }

        public virtual void OnCollision(CollisionInfo collisionInfo)
        {

        }

        public virtual void Update()
        {
            if (IsActive)
            {
                CheckOutOfScreen();
            }
        }

        protected virtual void CheckOutOfScreen()
        {
            //horizontal collisions
            if (Position.X - HalfWidth < CameraMngr.CameraLimits.MinX - Game.OrthoHalfWidth)
            {
                Sprite.position.X = CameraMngr.CameraLimits.MinX - Game.OrthoHalfWidth + HalfWidth;

                RigidBody.MoveSpeed.X *= -1;
                RigidBody.CurrentMoveSpeed.X = RigidBody.MoveSpeed.X;
            }

            else if (Position.X + HalfWidth > CameraMngr.CameraLimits.MaxX + Game.OrthoHalfWidth)
            {
                Sprite.position.X = CameraMngr.CameraLimits.MaxX + Game.OrthoHalfWidth - HalfWidth;

                RigidBody.MoveSpeed.X *= -1;
                RigidBody.CurrentMoveSpeed.X = RigidBody.MoveSpeed.X;
            }

            //vertical collisions
            if (Position.Y - HalfHeight < CameraMngr.CameraLimits.MinY - Game.OrthoHalfHeight)
            {
                Sprite.position.Y = CameraMngr.CameraLimits.MinY - Game.OrthoHalfHeight + HalfHeight;

                if (RigidBody.IsGravityAffected && RigidBody.CurrentMoveSpeed.Y < 0)
                {
                    RigidBody.MoveSpeed.Y *= -1;
                    RigidBody.CurrentMoveSpeed.Y = RigidBody.MoveSpeed.Y;
                }
            }
        }

        public virtual void Draw()
        {
            if (IsActive)
            {
                if (numFrames <= 1)
                {
                    Sprite.DrawTexture(texture);
                }

                else
                {
                    //if (this is Player)
                    //{
                    //    System.Console.WriteLine("Width: " + Width + "\nHeight: " + Height);
                    //}

                    Sprite.DrawTexture(texture, (int)(pixelsWidth * CurrentFrame), 0, (int)pixelsWidth, (int)pixelsHeight);
                }
            }
        }
    }
}
