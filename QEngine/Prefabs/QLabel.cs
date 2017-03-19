using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QEngine.Interfaces;
using QEngine.System;

namespace QEngine.Prefabs
{
	public class QLabel : QComponent
	{
		internal QLabel(SpriteFont font, QBehavior script)
		{
			Type = typeof(QLabel);
			Font = font;
			Script = script;
		}

		internal QBehavior Script { get; set; }

		internal SpriteFont Font { get; set; }

		StringBuilder PerformanceString { get; set; } = new StringBuilder();

		public Vector2 Measure(string measureWith) => Font.MeasureString(measureWith);

		public Color Hue { get; set; } = Color.White;

		public Vector2 Scale { get; set; } = Vector2.One;

		/// <summary>
		/// Adds the text to the next line on the output
		/// </summary>
		public string AppendLine
		{
			get
			{
				return PerformanceString.ToString();
			}
			set
			{
				PerformanceString.AppendLine(value);
			}
		}

		/// <summary>
		/// Clears the string and then sets the string to the new values
		/// </summary>
		public string SetText
		{
			set
			{
				PerformanceString.Clear();
				PerformanceString.Append(value);
			}
		}

		public string Text
		{
			get
			{
				return PerformanceString.ToString();
			}
			set
			{
				SetText = value;
			}
		} 

		public void ClearText()
		{
			PerformanceString.Clear();
		}

		public override string Name { get; protected set; }

		public sealed override Type Type { get; protected set; }

		public override void Destroy() {}
	}
}