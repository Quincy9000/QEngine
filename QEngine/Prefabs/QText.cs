using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QEngine.Interfaces;
using QEngine.System;

namespace QEngine.Prefabs
{
	public class QText : QComponent
	{
		internal QText(SpriteFont font, QBehavior script)
		{
			Type = typeof(QText);
			Font = font;
			Script = script;
		}

		internal QBehavior Script { get; set; }

		internal SpriteFont Font { get; set; }

		public Vector2 Measure(string measureWith) => Font.MeasureString(measureWith);

		public Color Hue { get; set; } = Color.White;

		public Vector2 Scale { get; set; } = Vector2.One;

		public string Text { get; set; } = "";

		public override string Name { get; protected set; }

		public sealed override Type Type { get; protected set; }

		public override void Destroy() {}
	}
}