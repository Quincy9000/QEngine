using System;
using FarseerPhysics;
using Microsoft.Xna.Framework;

namespace QEngine.System
{
	public class QTransform2D
	{
		Vector2 _position;
		float _rotation;
		internal QRigidbody body;

		public Vector2 Position
		{
			get
			{
				if(body?.Core != null && !body.Core.IsDisposed)
				{
					return body.Position;
				}
				return _position;
			}
			set
			{
				if(body?.Core != null && !body.Core.IsDisposed)
				{
					body.Position = value;
				}
				else
					_position = value;
			}
		}

		public Vector2 Scale { get; set; }

		public float Rotation
		{
			get
			{
				if(body?.Core != null && !body.Core.IsDisposed)
				{
					return body.Rotation;
				}
				return _rotation;
			}
			set
			{
				if(body?.Core != null && !body.Core.IsDisposed)
					body.Rotation = value;
				else
					_rotation = value;
			}
		}

		internal void Reset()
		{
			Position = Vector2.Zero;
			Scale = Vector2.One;
			Rotation = 0f;
			body = null;
		}

		public QTransform2D()
		{
			Reset();
		}
	}

	public class QTransform3D
	{
		public Vector3 Position = Vector3.Zero;
		public Vector3 Scale = Vector3.One;
		public Vector3 Rotation = Vector3.Zero;

		public void Reset()
		{
			Position = Vector3.Zero;
			Scale = Vector3.One;
			Rotation = Vector3.Zero;
		}
	}

	public static class Vectors
	{
		public static Vector2 ToVec2(this Vector3 vec)
		{
			return new Vector2(vec.X, vec.Y);
		}

		public static Vector2 Up(this Vector2 vec)
		{
			return -Vector2.UnitY;
		}

		public static Vector2 Down(this Vector2 vec)
		{
			return Vector2.UnitY;
		}

		public static Vector2 Left(this Vector2 vec)
		{
			return -Vector2.UnitX;
		}

		public static Vector2 Right(this Vector2 vec)
		{
			return Vector2.UnitX;
		}

		public static int ToInt(this float number)
		{
			return (int)Math.Round(number);
		}

		public static Vector2 Round(this Vector2 vec)
		{
			return new Vector2((int)Math.Round(vec.X), (int)Math.Round(vec.Y));
		}

		public static Vector2 ToDis(int x, int y)
		{
			return ConvertUnits.ToDisplayUnits(x, y);
		}

		public static float ToDis(this int temp)
		{
			return ConvertUnits.ToDisplayUnits(temp);
		}

		public static Vector2 ToSim(int x, int y)
		{
			return ConvertUnits.ToSimUnits(x, y);
		}

		public static float ToSim(this int temp)
		{
			return ConvertUnits.ToSimUnits(temp);
		}

		public static float ToSim(this float temp)
		{
			return ConvertUnits.ToSimUnits(temp);
		}

		public static float ToDis(this float temp)
		{
			return ConvertUnits.ToDisplayUnits(temp);
		}

		public static Vector2 ToSim(this Vector2 vec)
		{
			return ConvertUnits.ToSimUnits(vec);
		}

		public static Vector2 ToDis(this Vector2 vec)
		{
			return ConvertUnits.ToDisplayUnits(vec);
		}

		public static Vector2 Floor(this Vector2 vec)
		{
			return new Vector2((int)vec.X, (int)vec.Y);
		}

		public static Rectangle UpdateRect(Vector2 pos, Vector2 size)
		{
			return new Rectangle(pos.ToPoint(), size.ToPoint());
		}

		public static Vector2 TopRight(this Rectangle rec)
		{
			return new Vector2(rec.Right, rec.Y);
		}

		public static Vector2 TopLeft(this Rectangle rec)
		{
			return new Vector2(rec.X, rec.Y);
		}

		public static Vector2 BottomLeft(this Rectangle rec)
		{
			return new Vector2(rec.X, rec.Bottom);
		}

		public static Vector2 BottomRight(this Rectangle rec)
		{
			return new Vector2(rec.Right, rec.Bottom);
		}

		public static Vector2 Center(this Rectangle rec)
		{
			return new Vector2((rec.Width / 2f), (rec.Height / 2f));
		}

		public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
		{
			var x = MathHelper.Clamp(value.X, min.X, max.X);
			var y = MathHelper.Clamp(value.Y, min.Y, max.Y);

			return new Vector2(x, y);
		}

		/// <summary>
		/// Gets the middle point between two vectors
		/// </summary>
		/// <param name="vec1"></param>
		/// <param name="vec2"></param>
		/// <returns></returns>
		public static Vector2 Middle(this Vector2 vec1, Vector2 vec2)
		{
			Vector2 m;
			m.X = (vec1.X + vec2.X) / 2f;
			m.Y = (vec1.Y + vec2.Y) / 2f;
			return m;
		}

		/// <summary>
		/// Moves towards another vector at a constant speed
		/// </summary>
		/// <param name="vec"></param>
		/// <param name="target"></param>
		/// <param name="speed"></param>
		/// <returns></returns>
		public static Vector2 MoveTo(this Vector2 vec, Vector2 target, float speed)
		{
			Vector2 direction = target - vec;
			direction.Normalize();
			return direction * speed;
		}

		/// <summary>
		/// Returns a vector that is moving in the direction of the input, from that starting pos
		/// </summary>
		/// <param name="vec"></param>
		/// <param name="direction"></param>
		public static Vector2 MoveTo(this Vector2 startingPos, float direction)
		{
			Vector2 v;
			v.X = (float)Math.Sin(direction);
			v.Y = (float)Math.Cos(direction);
			return v;
		}

		/// <summary>
		/// Returns the angle at which to turn to look at the target
		/// </summary>
		/// <param name="position"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static float LookAt(this Vector2 position, Vector2 target)
		{
			var angle = (float)Math.Atan2(target.Y - position.Y, target.X - position.X);
			return MathHelper.ToRadians(angle * (180 / (float)Math.PI));
		}

		/// <summary>
		/// Returns the direction of the vector
		/// </summary>
		/// <param name="angle"></param>
		/// <returns></returns>
		public static Vector2 Direction(float angle)
		{
			return Vector2.Normalize(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
		}

		public static float Angle(this Vector2 from, Vector2 to)
		{
			return (float)Math.Atan2(from.Y - to.Y, from.X - to.X);
		}

		public static float Angle(this Vector2 from)
		{
			return (float)Math.Atan2(from.Y, from.X);
		}

		/// <summary>
		/// Trys to parse a vector2 from a string
		/// </summary>
		/// <param name="parse"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool Vector2TryParse(string parse, out Vector2 value)
		{
			var s = parse.Split(' ', '{', '}', ':');
			if(float.TryParse(s[2], out float x) && float.TryParse(s[4], out float y))
			{
				value = new Vector2(x, y);
				return true;
			}
			value = Vector2.Zero;
			return false;
		}
	}
}