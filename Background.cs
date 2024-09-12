using Aiv.Fast2D;
using OpenTK;

namespace Bulletz
{
    class Background : IDrawable
    {
        private Texture skyTexture;
        private Sprite skySprite;
        private Texture[] textures;
        private Sprite[] sprites;
        public Vector2 Position { get => skySprite.position; }
        public float Width { get => skySprite.Width; }
        public float Height { get => skySprite.Height; }
        public DrawLayer DrawLayer { get; private set; }

        public Background(int numTextures)
        {
            DrawLayer = DrawLayer.Background;

            skyTexture = TextureMngr.GetTexture("background");
            skySprite = new Sprite(Game.PixelsToUnits(skyTexture.Width * 2), Game.PixelsToUnits(skyTexture.Height));
            textures = new Texture[numTextures];
            sprites = new Sprite[numTextures * 2];

            float[] positions = { 2.5f, 0, 6.55f, 6.55f };
            bool[] pivotCenterScreen = { true, true, false, false };

            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = new Texture($"Assets/bg_{i}.png");

                float width = i == 0 ? Game.PixelsToUnits(textures[i].Width) + 1 : Game.PixelsToUnits(textures[i].Width);
                float height = i == 0 ? Game.PixelsToUnits(textures[i].Height) + 1 : Game.PixelsToUnits(textures[i].Height);

                sprites[i] = new Sprite(width, height);
                sprites[i].position.Y = positions[i];
                sprites[i].pivot = new Vector2(sprites[i].Width * 0.5f, sprites[i].Height * 0.5f);

                sprites[i].pivot = pivotCenterScreen[i] ? new Vector2(Game.OrthoHalfWidth, Game.OrthoHalfHeight) : new Vector2(sprites[i].Width * 0.5f, sprites[i].Height * 0.5f);

                int cloneIndex = i + numTextures;

                sprites[cloneIndex] = new Sprite(sprites[i].Width, sprites[i].Height);
                sprites[cloneIndex].position.Y = sprites[i].position.Y;
                sprites[cloneIndex].position.X = sprites[i].Width;
                sprites[cloneIndex].pivot = sprites[i].pivot;
            }

            DrawMngr.Add(this);
        }

        public void InitCameras()
        {
            int cloneIndex;

            skySprite.Camera = CameraMngr.GetCamera("Sky");
            skySprite.Camera.pivot = Vector2.Zero;

            for (int i = 0; i < textures.Length; i++)
            {
                cloneIndex = i + textures.Length;

                sprites[i].Camera = CameraMngr.GetCamera($"Bg_{i}");
                sprites[cloneIndex].Camera = sprites[i].Camera;
            }
        }

        public void Draw()
        {
            skySprite.DrawTexture(skyTexture);

            for (int i = 0; i < textures.Length; i++)
            {
                sprites[i].DrawTexture(textures[i]);
                sprites[i + textures.Length].DrawTexture(textures[i]);
            }
        }
    }
}