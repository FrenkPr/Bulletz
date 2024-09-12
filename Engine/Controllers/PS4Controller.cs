using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulletz
{
    class PS4Controller : JoypadController
    {
        public PS4Controller(int index) : base(index)
        {

        }

        public override bool IsFirePressed()
        {
            return IsValuePressed(JoypadValue.Square);
        }

        public override bool IsJumpPressed()
        {
            return IsValuePressed(JoypadValue.PS4_X);
        }

        public override bool IsValuePressed(JoypadValue value)
        {
            bool valuePressed = false;

            switch (value)
            {
                case JoypadValue.PS4_X:
                    valuePressed = Game.Window.JoystickB(controllerIndex);
                    break;
                case JoypadValue.Circle:
                    valuePressed = Game.Window.JoystickX(controllerIndex);
                    break;
                case JoypadValue.Triangle:
                    valuePressed = Game.Window.JoystickY(controllerIndex);
                    break;
                case JoypadValue.Square:
                    valuePressed = Game.Window.JoystickA(controllerIndex);
                    break;
                case JoypadValue.Start:
                    valuePressed = Game.Window.JoystickRightStick(controllerIndex);
                    break;
            }

            return valuePressed;
        }
    }
}
