using System;
using QEngine.System;
using Microsoft.Xna.Framework;
using QEngine.Interfaces;

namespace QEngine.Prefabs
{
	public class QCamera : QBehavior, IQStart
	{
		bool _isDirty = true;

		public Matrix TransformMatrix { get; private set; } = Matrix.Identity;

		public float Zoom
		{
			get
			{
				return Transform.Scale.X;
			}
			set
			{
				Transform.Scale = new Vector2(MathHelper.Clamp(value, 1f, 1000f), 0);
				_isDirty = true;
			}
		}

		public Vector2 Position
		{
			get
			{
				return Transform.Position;
			}
			set
			{
				Transform.Position = value;
				_isDirty = true;
			}
		}

		public float Rotation
		{
			get
			{
				return Transform.Rotation;
			}
			set
			{
				Transform.Rotation = MathHelper.ToRadians(value);
				_isDirty = true;
			}
		}

		/// <summary>
		/// Convert the Vector2 to the screen position
		/// </summary>
		/// <param name="worldPos"></param>
		/// <returns></returns>
		public Vector2 WorldToScreen(Vector2 worldPos)
		{
			return Vector2.Transform(worldPos, TransformMatrix);
		}

		/// <summary>
		/// Convert the Vector2 to the world inside the camera view
		/// </summary>
		/// <param name="screenPos"></param>
		/// <returns></returns>
		public Vector2 ScreenToWorld(Vector2 screenPos)
		{
			return Vector2.Transform(screenPos, Matrix.Invert(TransformMatrix));
		}

		public bool LoadData()
		{
			try
			{
				Zoom = QSaves.GetFloat("CameraZoom");
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		public void SaveData()
		{
			QSaves.AddFloat("CameraZoom", Zoom);
		}

		public QCamera() : base("QCamera") {}

		public void Start()
		{
			_isDirty = true;
			if(!LoadData())
				Zoom = 100f;
			Position = Vector2.Zero;
			UpdateMatrix();
		}

		public Rectangle GetBounds => new Rectangle((Position - new Vector2(Window.Width, Window.Height) / 2f).ToPoint(), new Vector2(Window.Width, Window.Height).ToPoint());

		/// <summary>
		/// Smoothly move the camera around
		/// </summary>
		/// <param name="location"></param>
		/// <param name="interp"></param>
		/// <param name="delta"></param>
		public void Lerp(Vector2 location, float interp, float delta)
		{
			Position = Vector2.Lerp(Position, location, interp * delta);
		}

		public void SmoothLerp(Vector2 location, float interp, float delta)
		{
			Position = Vector2.SmoothStep(Position, location, interp * delta);
		}

		public void PreciseLerp(Vector2 location, float interp, float delta)
		{
			Position = Vector2.LerpPrecise(Position, location, interp * delta);
		}

		public void MoveTo(Vector2 location, float speed)
		{
			Position += Position.MoveTo(location, speed);
		}

		internal void UpdateMatrix()
		{
			if(_isDirty)
			{
				var f = (float)Math.Round(Zoom / 100f, 2, MidpointRounding.ToEven);
				TransformMatrix = Matrix.CreateTranslation(-(int)Position.X, -(int)Position.Y, 0) *
				                  Matrix.CreateRotationZ(Rotation) *
				                  Matrix.CreateScale(f, f, 1) *
				                  Matrix.CreateTranslation((int)(Scene.Window.Width / 2f), (int)(Scene.Window.Height / 2f), 0);
				_isDirty = false;
			}
		}

		public void Unload()
		{
			SaveData();
		}
	}
}