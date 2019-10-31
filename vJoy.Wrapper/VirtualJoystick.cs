using System;

namespace vJoy.Wrapper
{
    /// <summary>
    /// VirtualJoystick: Helper class to set and read button states.  Helper keeps its own state and updates the vJoy object after state changes.
    /// If vJoy object's button states or axis are set directly through the "Joystick" property the internal state is not updated.
    /// </summary>
    public class VirtualJoystick : IDisposable
    {
        // Common vJoy Instance
        static private vJoyInterfaceWrap.vJoy vJoyInstance = new vJoyInterfaceWrap.vJoy();

        /// <summary>
        /// Virtual Joystick is Aquired and connected
        /// </summary>
        public bool Aquired { get; private set; }

        /// <summary>
        /// Direct access to the vJoy object.
        /// </summary>
        public vJoyInterfaceWrap.vJoy Joystick { get => vJoyInstance; }

        /// <summary>
        /// vJoy Joystick number
        /// </summary>
        public uint JoystickId { get; }

        private vJoyInterfaceWrap.vJoy.JoystickState State = new vJoyInterfaceWrap.vJoy.JoystickState();

        /// <summary>
        /// Constructor: vJoystick Id is the vJoy number to aquire
        /// </summary>
        /// <param name="vJoystickId"></param>
        public VirtualJoystick(uint vJoystickId)
        {
            JoystickId = vJoystickId;
        }

        /// <summary>
        /// Connect to the virtual joystick
        /// </summary>
        public void Aquire()
        {
            State.bDevice = (byte)JoystickId;
            Joystick.AcquireVJD(JoystickId);
            Aquired = true;
            Joystick.ResetVJD(JoystickId);
        }

        /// <summary>
        /// Disconnect from the virtual joystick
        /// </summary>
        public void Release()
        {
            Joystick?.RelinquishVJD(JoystickId);
            Aquired = false;
        }

        public void Dispose()
        {
            if (Aquired)
                Joystick?.RelinquishVJD(JoystickId);
        }

        /// <summary>
        /// Update the virtual joystick with the current state
        /// </summary>
        public void Update()
        {
            Joystick.UpdateVJD(JoystickId, ref State);
        }

        /// <summary>
        /// Press/Release a virtual joystick button
        /// </summary>
        /// <param name="down">true = button pressed, false = button released</param>
        /// <param name="vButtonNumber">virtual button number</param>
        public void SetJoystickButton(bool down, uint vButtonNumber)
        {
            // Offset by one
            var vButton = vButtonNumber - 1;

            // Set the position or don't
            var buttons = down ? (uint)0x1 << (int)vButton : 0;

            // Build a mask for that position
            var mask = (uint)0x1 << (int)vButton;

            // Clear just that position
            var holdValues = State.Buttons & ~mask;

            // Set the new value
            State.Buttons = holdValues | buttons;

            // Update
            Joystick.UpdateVJD(JoystickId, ref State);
        }

        /// <summary>
        /// Get the current state of a virtual button
        /// </summary>
        /// <param name="vButtonNumber">virtual button number</param>
        /// <returns>true = button pressed, false = button released</returns>
        public bool GetJoystickButton(uint vButtonNumber)
        {
            // Offset by one
            var vButton = vButtonNumber - 1;

            // Build a mask for that position
            var mask = (uint)0x1 << (int)vButton;

            var result = State.Buttons & mask;
            return (State.Buttons & mask) == mask;
        }

        /// <summary>
        /// Sets the state of all the buttons (max 32 button device)
        /// </summary>
        /// <param name="buttons">binary state of all the buttons</param>
        /// <param name="mask">bitmask for applying changes</param>
        public void SetJoystickButtons(UInt32 buttons, UInt32 mask = 0xFFFFFFFF)
        {
            // Clear the buttons we are assigning
            var holdValues = State.Buttons & ~mask;
            State.Buttons = holdValues | buttons;
            Joystick.UpdateVJD(JoystickId, ref State);
        }

        /// <summary>
        /// Set the value for a virtual axis
        /// </summary>
        /// <param name="value">axis value</param>
        /// <param name="usage">axis to set</param>
        public void SetJoystickAxis(int value, Axis usage)
        {
            switch (usage)
            {
                case Axis.HID_USAGE_X:
                    State.AxisX = value;
                    break;
                case Axis.HID_USAGE_Y:
                    State.AxisY = value;
                    break;
                case Axis.HID_USAGE_Z:
                    State.AxisZ = value;
                    break;
                case Axis.HID_USAGE_RX:
                    State.AxisXRot = value;
                    break;
                case Axis.HID_USAGE_RY:
                    State.AxisYRot = value;
                    break;
                case Axis.HID_USAGE_RZ:
                    State.AxisZRot = value;
                    break;
                case Axis.HID_USAGE_SL0:
                    State.Slider = value;
                    break;
                case Axis.HID_USAGE_SL1:
                    State.Dial = value;
                    break;
                case Axis.HID_USAGE_WHL:
                    State.Wheel = value;
                    break;
                case Axis.HID_USAGE_POV:
                    //State. = value;  //Not sure where this maps
                    break;
            }

            Joystick.UpdateVJD(JoystickId, ref State);
        }

        /// <summary>
        /// Set the value for a virtual hat
        /// </summary>
        /// <param name="value">hat value</param>
        /// <param name="hat">virtual hat</param>
        public void SetJoystickHat(int value, Hats hat)
        {
            switch (hat)
            {
                case Hats.Hat:
                    State.bHats = (byte)value;
                    break;
                case Hats.HatExt1:
                    State.bHatsEx1 = (byte)value;
                    break;
                case Hats.HatExt2:
                    State.bHatsEx2 = (byte)value;
                    break;
                case Hats.HatExt3:
                    State.bHatsEx3 = (byte)value;
                    break;
            }
            Joystick.UpdateVJD(JoystickId, ref State);
        }
    }
}
