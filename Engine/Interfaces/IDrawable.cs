

namespace Bulletz
{
    interface IDrawable
    {
        DrawLayer DrawLayer { get; }

        void Draw();
    }
}
