using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace QEngine.System
{
	public static class QControls
	{
		static KeyboardState _previousKeyboardState;
		static KeyboardState _currentKeyboardState;

		static MouseState _previousMouseState;
		static MouseState _currentMouseState;

		public enum ControlModes
		{
			Yes,
			No
		}

		public static ControlModes ControlMode { get; set; }

		static QControls()
		{
			_currentMouseState = Mouse.GetState();
			_currentKeyboardState = Keyboard.GetState();
			Task.Run(() =>
			{
				while(true)
				{
					Update();
					Thread.Sleep(TimeSpan.FromSeconds(1 / 120.0));
				}
			});
		}

		public static void Flush()
		{
			_previousMouseState = _currentMouseState;
			_previousKeyboardState = _currentKeyboardState;
		}

		public static void Update()
		{
			Flush();
			_currentKeyboardState = Keyboard.GetState();
			_currentMouseState = Mouse.GetState();
		}

		public static bool KeyPressed(Keys k) => _currentKeyboardState.IsKeyDown(k) && _previousKeyboardState.IsKeyUp(k);

		public static bool KeyReleased(Keys k) => _currentKeyboardState.IsKeyUp(k) && _previousKeyboardState.IsKeyDown(k);

		public static bool KeyDown(Keys k) => _currentKeyboardState.IsKeyDown(k);

		public static Point MousePosition => _currentMouseState.Position;

		public static bool LeftMousePressed => _currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released;

		public static bool LeftMouseReleased => _currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed;

		public static bool LeftMouseDown => _currentMouseState.LeftButton == ButtonState.Pressed;

		public static bool RightMouseDown => _currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released;

		public static bool RightMouseUp => _currentMouseState.RightButton == ButtonState.Released && _previousMouseState.RightButton == ButtonState.Pressed;

		public static bool RightMouseHeld => _currentMouseState.RightButton == ButtonState.Pressed;

		public static bool MiddleMouseDown => _currentMouseState.MiddleButton == ButtonState.Pressed && _previousMouseState.MiddleButton == ButtonState.Released;

		public static bool MiddleMouseUp => _currentMouseState.MiddleButton == ButtonState.Released && _previousMouseState.MiddleButton == ButtonState.Pressed;

		public static bool MiddleMouseHeld => _currentMouseState.MiddleButton == ButtonState.Pressed;

		public static bool ScrollUp => _currentMouseState.ScrollWheelValue > _previousMouseState.ScrollWheelValue;

		public static bool ScrollDown => _currentMouseState.ScrollWheelValue < _previousMouseState.ScrollWheelValue;
	}
}