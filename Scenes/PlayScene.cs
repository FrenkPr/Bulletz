using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenTK;

namespace Bulletz
{
    class PlayScene : Scene
    {
        public Background Background;
        public Player Player;
        private EarthGrass earthGrass;

        public PlayScene() : base()
        {

        }

        private void LoadAssets()
        {
            TextureMngr.AddTexture("actor", "Assets/player.png");
            TextureMngr.AddTexture("background", "Assets/sky.png", true);
            TextureMngr.AddTexture("bullet", "Assets/greenGlobe.png");
            TextureMngr.AddTexture("textImage", "Assets/comics.png");
            TextureMngr.AddTexture("frameProgressBar", "Assets/loadingBar_frame.png");
            TextureMngr.AddTexture("progressBar", "Assets/loadingBar_bar.png");
            TextureMngr.AddTexture("earthGrass", "Assets/Levels/earthGrass.png");

            for (int i = 0; i < 4; i++)
            {
                TextureMngr.AddTexture($"bg_{i}.png", $"Assets/bg_{i}.png");
            }
        }

        public override void Start()
        {
            base.Start();

            LoadAssets();

            Background = new Background(4);

            CameraMngr.Init(null, new CameraLimits(Background.Position.X + Background.Width - Game.OrthoHalfWidth - Background.Width * 0.5f * 0.5f, Background.Position.X + Background.Width * 0.5f * 0.5f, Game.OrthoHeight, Game.OrthoHalfHeight));

            CameraMngr.AddCamera("GUI", new Aiv.Fast2D.Camera());
            CameraMngr.AddCamera("Sky", cameraSpeed: 0.02f);
            CameraMngr.AddCamera("Bg_0", cameraSpeed: 0.15f);
            CameraMngr.AddCamera("Bg_1", cameraSpeed: 0.2f);
            CameraMngr.AddCamera("Bg_2", cameraSpeed: 0.9f);
            CameraMngr.AddCamera("Bg_3", cameraSpeed: 0.9f);
            Background.InitCameras();

            TextMngr.Init();

            Player = new Player(0);

            CameraMngr.Target = Player;
            Player.X = CameraMngr.MainCamera.position.X;

            BulletMngr.Init();
            EnemyMngr.Init();

            EnemyMngr.SpawnEnemy();

            earthGrass = new EarthGrass();
        }

        public override void Update()
        {
            Player.KeyboardInput();

            for (int i = 0; i < Game.JoypadCtrls.Count; i++)
            {
                Player.JoypadInput();
            }

            //update
            PhysicsMngr.Update();
            UpdateMngr.Update();
            CameraMngr.Update();

            PhysicsMngr.CheckCollisions();

            //draw
            DrawMngr.Draw();
            DebugMngr.Draw();

            //printing phrases
            TextMngr.PrintPhrases();

            if (!Player.IsAlive)
            {
                IsPlaying = false;

                Game.Window.Update();
                Thread.Sleep(2000);
            }
        }

        public override void OnExit()
        {
            EnemyMngr.ClearAll();
            BulletMngr.ClearAll();
            UpdateMngr.ClearAll();
            PhysicsMngr.ClearAll();
            TextMngr.ClearAll();
            DrawMngr.ClearAll();
            TextureMngr.ClearAll();
            DebugMngr.ClearAll();

            Player = null;
            Background = null;
            NextScene = null;
        }
    }
}
