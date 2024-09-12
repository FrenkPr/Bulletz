using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulletz
{
    abstract class Controller
    {
        protected int controllerIndex;

        public Controller(int index)
        {
            controllerIndex = index;
        }

        public abstract float GetHorizontal();
        public abstract float GetVertical();
        public abstract bool IsFirePressed();
        public abstract bool IsJumpPressed();
    }
}
