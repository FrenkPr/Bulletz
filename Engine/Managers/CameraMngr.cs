using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aiv.Fast2D;
using OpenTK;

namespace Bulletz
{
    struct CameraLimits
    {
        public float MaxX;
        public float MinX;
        public float MaxY;
        public float MinY;

        public CameraLimits(float maxX, float minX, float maxY, float minY)
        {
            MaxX = maxX;
            MinX = minX;
            MaxY = maxY;
            MinY = minY;
        }
    }

    static class CameraMngr
    {
        public static Camera MainCamera;
        private static Dictionary<string, Tuple<Camera, float>> cameras;
        public static GameObject Target;
        public static CameraLimits CameraLimits;
        private static float cameraSpeed;
        public static float HalfDiagonalSquared { get => MainCamera.pivot.LengthSquared; }

        public static void Init(GameObject target, CameraLimits cameraLimits)
        {
            MainCamera = new Camera();
            MainCamera.pivot = new Vector2(Game.OrthoHalfWidth, Game.OrthoHalfHeight);
            Target = target;
            CameraLimits = cameraLimits;
            cameraSpeed = 5;

            cameras = new Dictionary<string, Tuple<Camera, float>>();
        }

        public static void AddCamera(string cameraName, Camera camera = null, float cameraSpeed = 0)
        {
            if (camera == null)
            {
                camera = new Camera(MainCamera.position.X, MainCamera.position.Y);
                camera.pivot = MainCamera.pivot;
            }

            cameras[cameraName] = new Tuple<Camera, float>(camera, cameraSpeed);
        }

        public static Camera GetCamera(string cameraName)
        {
            if (cameras.ContainsKey(cameraName))
            {
                return cameras[cameraName].Item1;
            }

            return null;
        }

        public static void Update()
        {
            Vector2 oldCameraPos = MainCamera.position;

            MainCamera.position = Vector2.Lerp(MainCamera.position, Target.Position, cameraSpeed * Game.DeltaTime);
            CheckMainCameraOutOfScreen();

            Vector2 cameraDelta = MainCamera.position - oldCameraPos;

            if (cameraDelta != Vector2.Zero)
            {
                //camera moved
                foreach (var item in cameras)
                {
                    item.Value.Item1.position += cameraDelta * item.Value.Item2;  //camera position += delta * cameraSpeed
                }
            }
        }

        private static void CheckMainCameraOutOfScreen()
        {
            MainCamera.position.X = MathHelper.Clamp(MainCamera.position.X, CameraLimits.MinX, CameraLimits.MaxX);
            MainCamera.position.Y = MathHelper.Clamp(MainCamera.position.Y, CameraLimits.MinY, CameraLimits.MaxY);
        }
    }
}
