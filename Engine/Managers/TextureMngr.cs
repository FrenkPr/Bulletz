using System.Collections.Generic;
using Aiv.Fast2D;

namespace Bulletz
{
    static class TextureMngr
    {
        private static Dictionary<string, Texture> textures;

        static TextureMngr()
        {
            textures = new Dictionary<string, Texture>();
        }

        public static void AddTexture(string id, string path, bool repeatX = false, bool repeatY = false)
        {
            if (id == "" || id == null || path == "" || path == null)
            {
                return;
            }

            if (textures.ContainsKey(id))
            {
                return;
            }

            Texture t = new Texture(path, repeatX: repeatX, repeatY: repeatY);

            textures.Add(id, t);
        }

        public static Texture GetTexture(string id)
        {
            return textures[id];
        }

        public static void ClearAll()
        {
            textures.Clear();
        }
    }
}
