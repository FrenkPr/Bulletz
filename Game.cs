using System.Collections.Generic;
using Aiv.Fast2D;
using OpenTK;

namespace Bulletz
{
    static class Game
    {
        public static Window Window { get; private set; }
        public static float DeltaTime { get { return Window.DeltaTime; } }
        public static int WindowWidth { get { return Window.Width; } }
        public static int WindowHeight { get { return Window.Height; } }
        public static int HalfWindowWidth { get { return (int)(Window.Width * 0.5f); } }
        public static int HalfWindowHeight { get { return (int)(Window.Height * 0.5f); } }
        public static float OrthoWidth { get { return Window.OrthoWidth; } }
        public static float OrthoHeight { get { return Window.OrthoHeight; } }
        public static float OrthoHalfWidth { get { return Window.OrthoWidth * 0.5f; } }
        public static float OrthoHalfHeight { get { return Window.OrthoHeight * 0.5f; } }
        public static Scene Scene;
        public static KeyboardController KeyboardCtrl;
        public static List<JoypadController> JoypadCtrls;
        private static int numMaxJoypadCtrls;
        private static float optimalUnitSize;
        private static float optimalScreenHeight;
        private static bool isMousePressed;
        private static Vector2 lastMousePositionClicked;

        public static void Init()
        {
            Window = new Window(1920, 1080, "Bulletz");
            Window.Position = Vector2.Zero;
            Window.SetDefaultViewportOrthographicSize(10);
            optimalScreenHeight = 1080;
            optimalUnitSize = optimalScreenHeight / Window.OrthoHeight;

            //System.Console.WriteLine(Window.CurrentViewportOrthographicSize + "\n" + Window.OrthoWidth + "\n" + Window.OrthoHeight);

            KeyboardCtrl = new KeyboardController(0);

            string[] joypadsConnected = Window.Joysticks;
            JoypadCtrls = new List<JoypadController>();
            numMaxJoypadCtrls = 1;

            for (int i = 0; i < joypadsConnected.Length; i++)
            {
                if (i >= numMaxJoypadCtrls)
                {
                    break;
                }

                //System.Console.WriteLine(joypadsConnected[i] + "pos: " + i);

                if (joypadsConnected[i] != null && joypadsConnected[i] != "Unmapped Controller")
                {
                    JoypadCtrls.Add(new PS4Controller(i));
                }
            }

            Scene = new PlayScene();
        }

        public static void Run()
        {
            Scene.Start();

            while (Window.IsOpened)
            {
                //System.Console.WriteLine("Mouse X: " + (Window.MouseX) + "\nMouse Y: " + (Window.MouseY));

                //for (int i = 0; i < JoypadCtrls.Count; i++)
                //{
                //    System.Console.WriteLine(Window.JoystickDebug(i));
                //}

                if (Scene.IsPlaying)
                {
                    if (Window.MouseLeft)
                    {
                        if (!isMousePressed)
                        {
                            lastMousePositionClicked = Window.MousePosition;
                            isMousePressed = true;
                        }
                    }

                    else if (isMousePressed)
                    {
                        isMousePressed = false;
                    }

                    if (Window.MousePosition == lastMousePositionClicked &&
                        Window.MouseX >= -0.1178782f &&
                        Window.MouseX <= 17.84246f &&
                        Window.MouseY >= -0.476787925f &&
                        Window.MouseY <= -0.01254705f &&
                        isMousePressed &&
                        !Window.IsFullScreen())
                    {
                        Window.Update();
                        continue;
                    }

                    Scene.Update();

                    //window update
                    Window.Update();
                }

                else
                {
                    Scene.OnExit();

                    if (Scene.NextScene != null)
                    {
                        Scene = Scene.NextScene;
                        Scene.Start();
                    }

                    else
                    {
                        return;
                    }
                }
            }
        }

        public static float PixelsToUnits(float val)
        {
            return val / optimalUnitSize;
        }
    }
}
