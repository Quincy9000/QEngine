using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QEngine.Interfaces;
using QEngine.System;

namespace QEngine.Prefabs
{
	/// <summary>
	/// Debug tool that can display Text inside the game scene area
	/// </summary>
	public sealed class QConsole : QBehavior, IQLoad, IQStart, IQUpdate, IQGui
	{
		[ImmutableObject(true)]
		struct MessageBox
		{
			public MessageBox(string t, Vector2 v)
			{
				Text = t;
				MeasureMent = v;
			}

			public readonly string Text;
			public readonly Vector2 MeasureMent;
		}

		//using a linked list so that I can add to the first element each write and then remove the last element so that it stays
		// under a certain ammount

		public int Width { get; set; }
		public int Height { get; set; }
		public void SetFont(SpriteFont font) => _label.Font = font;
		public string FontName { get; private set; }
		public double FadeStart { get; set; } = 2;

		double _startFade;
		double _fade;
		QLabel _label;
		LinkedList<MessageBox> Messages { get; set; }

		public Color FontColor
		{
			get
			{
				return _label.Hue;
			}
			set
			{
				_label.Hue = value;
			}
		}

		public QConsole(int width, int height) : base("QConsole")
		{
			Width = width;
			Height = height;
			Messages = new LinkedList<MessageBox>();
		}

		Vector2 Measure(QLabel label)
		{
			return label.Font.MeasureString(label.Text);
		}

		Vector2 Measure(string text)
		{
			return _label.Font.MeasureString(text);
		}

		/// <summary>
		/// Writes to the screen and disappears after a certain amount of time has passes useful for debugging
		/// </summary>
		/// <param name="message"></param>
		public void WriteLine(string message)
		{
			_fade = FadeStart;
			_startFade = 0;
			_label.Hue = Color.White;
			var measure = Measure(message);
			if(measure.X > Width)
			{
				var s = WordWrap(message, Width);
				Messages.AddFirst(new MessageBox(s, measure));
			}
			else
			{
				Messages.AddFirst(new MessageBox(message, measure));
			}
			while(Messages.Count > Height)
				Messages.RemoveLast();
		}

		public void WriteLine<T>(T message)
		{
			WriteLine(message.ToString());
		}

		/// <summary>
		/// Clears all the messages in the console
		/// </summary>
		public void Clear()
		{
			Messages = new LinkedList<MessageBox>();
			_startFade = 0;
		}

		public void Load()
		{
			Clear();
			//loads default font, but you can change it
			FontName = "Arial";
			LoadFont("Fonts/" + FontName);
		}

		public void Start()
		{
			_label = new QLabel(GetFont(FontName), this);
			_startFade = 0;
		}

		public void Update(float delta)
		{
			_startFade += delta;
			if(_startFade > FadeStart)
			{
				_fade -= delta;
				_label.Hue = new Color((float)_fade, (float)_fade, (float)_fade, (float)_fade);
			}
		}

		public void Gui(QRenderer2D renderer)
		{
			Vector2 temp = Transform.Position;
//			for(int i = 0; i < Messages.Count; i++)
//			{
//				_label.Text = Messages[i].Text;
//				renderer.DrawString(_label, temp);
//				temp.Y += Messages[i].MeasureMent.Y * _label.Scale.Y;
//			}
			foreach(var m in Messages)
			{
				_label.SetText = m.Text;
				renderer.DrawString(_label, temp);
				temp.Y += m.MeasureMent.Y * _label.Scale.Y;
			}
		}

		//http://stackoverflow.com/questions/17586/best-word-wrap-algorithm
		static readonly char[] SplitChars = {' ', '-', '\t'};

		private static string WordWrap(string str, int width)
		{
			string[] words = Explode(str, SplitChars);

			int curLineLength = 0;
			StringBuilder strBuilder = new StringBuilder();
			for(int i = 0; i < words.Length; i += 1)
			{
				string word = words[i];
				// If adding the new word to the current line would be too long,
				// then put it on a new line (and split it up if it's too long).
				if(curLineLength + word.Length > width)
				{
					// Only move down to a new line if we have Text on the current line.
					// Avoids situation where wrapped whitespace causes emptylines in Text.
					if(curLineLength > 0)
					{
						strBuilder.Append(Environment.NewLine);
						curLineLength = 0;
					}

					// If the current word is too long to fit on a line even on it's own then
					// split the word up.
					while(word.Length > width)
					{
						strBuilder.Append(word.Substring(0, width - 1) + "-");
						word = word.Substring(width - 1);

						strBuilder.Append(Environment.NewLine);
					}

					// Remove leading whitespace from the word so the new line starts flush to the left.
					word = word.TrimStart();
				}
				strBuilder.Append(word);
				curLineLength += word.Length;
			}
			return strBuilder.ToString();
		}

		private static string[] Explode(string str, char[] splitChars)
		{
			List<string> parts = new List<string>();
			int startIndex = 0;
			while(true)
			{
				int index = str.IndexOfAny(splitChars, startIndex);

				if(index == -1)
				{
					parts.Add(str.Substring(startIndex));
					return parts.ToArray();
				}

				string word = str.Substring(startIndex, index - startIndex);
				char nextChar = str.Substring(index, 1)[0];
				// Dashes and the likes should stick to the word occuring before it. Whitespace doesn't have to.
				if(char.IsWhiteSpace(nextChar))
				{
					parts.Add(word);
					parts.Add(nextChar.ToString());
				}
				else
				{
					parts.Add(word + nextChar);
				}

				startIndex = index + 1;
			}
		}
	}
}