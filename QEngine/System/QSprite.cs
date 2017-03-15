using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QEngine.Interfaces;

namespace QEngine.System
{
	public sealed class QSprite : QComponent
	{
		public QBehavior Parent { get; set; }

		public override string Name { get; protected set; }

		public override Type Type { get; protected set; }

		public override void Destroy() {}

		public Vector2 Origin { get; set; } = Vector2.Zero;

		public Rectangle Source { get; set; } = Rectangle.Empty;

		public Rectangle Destination { get; set; } = Rectangle.Empty;

		public SpriteEffects Effect { get; set; } = SpriteEffects.None;

		public Color Hue { get; set; } = Color.White;

		float _layer;

		public float Layer
		{
			get
			{
				return _layer;
			}
			set
			{
				_layer = value / 1000f;
			}
		}

		QSprite()
		{
			Type = typeof(QSprite);
		}

		public QSprite(QBehavior parent)
		{
			this.Parent = parent;
		}

		internal static QSprite Sprite()
		{
			return new QSprite();
		}
	}

	public static class QSpriteExtentions
	{
		/// <summary>
		/// splits rectangle into an array of smaller rectangles, that are the dimention of the width and height
		/// Great for textures that are spritesheets, as long as you follow a convention
		/// </summary>
		/// <param name="rec"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <returns></returns>
		public static List<Rectangle> Split(this Rectangle rec, int w, int h)
		{
			int x = rec.Location.X;
			int y = rec.Location.Y;
			List<Rectangle> rects = new List<Rectangle>();

			for(int i = 0; i < rec.Height / h; i++)
			{
				for(int j = 0; j < rec.Width / w; j++)
				{
					rects.Add(new Rectangle(x + j * w, y + i * h, w, h));
				}
			}

			return rects;
		}

		/// <summary>
		/// Splits rectangle into 9 rectangles
		/// </summary>
		/// <param name="rec"></param>
		/// <returns></returns>
		public static Rectangle[] NineSplice(this Rectangle rec)
		{
			Rectangle r;
			Rectangle[] splices = new Rectangle[9];
			for(int i = 0; i < 3; i++)
			{
				for(int j = 0; j < 3; j++)
				{
					r.X = rec.Location.X + (rec.Width / 3) * j;
					r.Y = rec.Location.Y + (rec.Height / 3) * i;
					r.Width = (int)(rec.Width / 3f);
					r.Height = (int)(rec.Height / 3f);
					splices[i * 3 + j] = r;
				}
			}
			return splices;
		}
	}
}