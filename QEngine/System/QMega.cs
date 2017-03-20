using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine.System
{
	public class QMega2D
	{
		internal QMega2D(RenderTarget2D target, Dictionary<string, Rectangle> rects)
		{
			Texture = target;
			Rectangles = rects;
		}

		public Texture2D Texture { get; internal set; }

		public Dictionary<string, Rectangle> Rectangles { get; internal set; }

		public static QMega2D Compile(QRenderer2D render, Dictionary<string, Texture2D> textures, bool saveMegaToDisk = false)
		{
			var width = 0;
			var height = 0;

			foreach(var t in textures.Values)
			{
				width += t.Width + 1;
				if(t.Height > height)
					height = t.Height;
			}

			var pos = Vector2.Zero;

			var target = new RenderTarget2D(render.Device, width, height);

			var rects = new Dictionary<string, Rectangle>();

			render.Device.SetRenderTarget(target);

			render.Clear(Color.Transparent);

			render.Begin();

			foreach(var t in textures)
			{
				render.Draw(t.Value, pos, Color.White);
				rects.Add(t.Key, new Rectangle(pos.ToPoint(), t.Value.Bounds.Size));
				pos.X += t.Value.Width + 1;
			}

			render.End();

			render.Device.SetRenderTarget(null);

			if(saveMegaToDisk)
			{
				QScene.SaveTextureAsPng(target);
				Task.Delay(1000);
			}

			return new QMega2D(target, rects);
		}
	}
}
