# vJoy.Wrapper

C# [vJoy](http://vjoystick.sourceforge.net/site/) wrapper

## About

vJoy.Wrapper is a helper class to communicate with the vJoy library.  *vJoy is a device driver that bridges the gap between any device that is not a joystick and an application that requires a joystick.*  Please see the [vJoy](http://vjoystick.sourceforge.net/site/) documentation for more information.

## Prerequisites

vJoy must be installed and at least one virtual joystick configured.

## Usage

```
using vJoy.Wrapper;

var joystick = new VirtualJoystick(1);   // Aquire vJoy device 1
joystick.SetJoystickButton(true, 1);     // Press button 1
joystick.SetJoystickAxis(16000, Axis.HID_USAGE_X);  // Center X axis
joystick.SetJoystickButton(false, 1);    // Release button 1

```