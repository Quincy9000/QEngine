using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QEngine.Interfaces;
using QEngine.Prefabs;

namespace QEngine.System
{
	public class QRenderer2D : QRenderer, IDisposable
	{
		internal readonly SpriteBatch _batch;

		public Color BackgroundRenderColor { get; set; } = Color.CornflowerBlue;

		public SpriteSortMode SortMode { get; set; }

		public SamplerState SamplerMode { get; set; }

		public BlendState BlendMode { get; set; }

		public DepthStencilState DepthMode { get; set; }

		public RasterizerState CullMode { get; set; }

		public Effect EffectMode { get; set; }

		public Matrix MatrixMode { get; set; }

		public GraphicsDevice Device => device;

		public QMega2D MegaTexture;

		public void Clear() => device.Clear(BackgroundRenderColor);

		public void Clear(Color c) => device.Clear(c);

		/// <summary>
		/// Set modes majority focus on either 2d or 3d
		/// </summary>
		public enum RenderModes
		{
			SpriteMajority,
			ModelMajority
		}

		/// <summary>
		/// Sets all the modes to the default values
		/// </summary>
		/// <param name="mode"></param>
		internal void SetRenderMode(RenderModes mode)
		{
			switch(mode)
			{
				case RenderModes.SpriteMajority:
				{
					SortMode = SpriteSortMode.FrontToBack;
					SamplerMode = SamplerState.PointClamp;
					BlendMode = BlendState.NonPremultiplied;
					DepthMode = DepthStencilState.Default;
					CullMode = RasterizerState.CullNone;
					EffectMode = null;
					MatrixMode = Matrix.Identity;
					break;
				}
				case RenderModes.ModelMajority:
				{
					SortMode = SpriteSortMode.Texture;
					SamplerMode = SamplerState.AnisotropicClamp;
					BlendMode = BlendState.NonPremultiplied;
					DepthMode = DepthStencilState.Default;
					CullMode = RasterizerState.CullClockwise;
					EffectMode = null;
					MatrixMode = Matrix.Identity;
					break;
				}
			}
		}

		internal void CompileMega(Dictionary<string, Texture2D> textures)
		{
			MegaTexture?.Texture?.Dispose();
			MegaTexture = QMega2D.Compile(this, textures);
		}

		internal void Begin()
		{
			_batch.Begin(SortMode, BlendMode, SamplerMode, DepthMode, CullMode, EffectMode, MatrixMode);
		}

		public void Draw(Texture2D t, QSprite spr, QTransform2D tr)
		{
			_batch.Draw(t, tr.Position, spr.Source, spr.Hue, tr.Rotation, spr.Origin, tr.Scale, spr.Effect, spr.Layer);
		}

		public void Draw(QSprite spr, QTransform2D tr)
		{
			if(MegaTexture != null && !MegaTexture.Texture.IsDisposed)
				_batch.Draw(MegaTexture.Texture, tr.Position, spr.Source, spr.Hue, tr.Rotation, spr.Origin, tr.Scale, spr.Effect, spr.Layer);
		}

		public void DrawString(QText text)
		{
			_batch.DrawString(text.Font, text.Text, text.Script.Transform.Position, text.Hue);
		}

		public void DrawString(QText text, Vector2 pos)
		{
			_batch.DrawString(text.Font, text.Text, pos, text.Hue);
		}

		internal void End()
		{
			_batch.End();
		}

		/// <summary>
		/// Takes a list of QObjects and turns their draw calls into one texture
		/// Useful for creating tilemaps
		/// </summary>
		/// <param name="device"></param>
		/// <param name="render"></param>
		/// <param name="list"></param>
		/// <param name="size"></param>
		/// <returns></returns>
		public RenderTarget2D CreateTextureFromMany(List<IQDraw> list, Vector2 size)
		{
			var target = new RenderTarget2D(Device, (int)size.X, (int)size.Y);
			Device.SetRenderTarget(target);
			Clear(Color.Transparent);
			Begin();
			foreach(var l in list)
			{
				l.Draw(this);
			}
			End();
			Device.SetRenderTarget(null);
			return target;
		}

		/// <summary>
		/// Get a partial section from a large texture and get it as a smaller portion
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		public RenderTarget2D GetPartialTexture2D(Texture2D texture, int w, int h, Rectangle source)
		{
			var d = Device;
			var render = new RenderTarget2D(d, w, h);

			d.SetRenderTarget(render);

			d.Clear(Color.Transparent);
			_batch.Begin(samplerState: SamplerState.PointClamp);
			_batch.Draw(texture, Vector2.Zero, source, Color.White);
			_batch.End();

			d.SetRenderTarget(null);

			return render;
		}

		/// <summary>
		/// Get a partial section from a large texture and get it as a smaller portion
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="source"></param>
		/// <returns></returns>
		public RenderTarget2D GetPartialTexture2D(Texture2D texture, Rectangle source)
		{
			var d = texture.GraphicsDevice;
			var render = new RenderTarget2D(d, source.Width, source.Height);

			d.SetRenderTarget(render);

			d.Clear(Color.Transparent);
			_batch.Begin(samplerState: SamplerState.PointClamp);
			_batch.Draw(texture, Vector2.Zero, source, Color.White);
			_batch.End();

			d.SetRenderTarget(null);

			return render;
		}

		/// <summary>
		/// Creates Rectangle Texture with One Color
		/// </summary>
		/// <param name="gd"></param>
		/// <param name="w"></param>
		/// <param name="h"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		internal Texture2D CreateRectangleTexture(GraphicsDevice gd, int w, int h, Color color)
		{
			var texture = new Texture2D(gd, w, h);
			Color[] toTexture = new Color[w * h];
			for (int i = 0; i < toTexture.Length; i++)
				toTexture[i] = color;
			texture.SetData(toTexture);
			return texture;
		}

		/// <summary>
		/// Creates a Cicle Texture with one color
		/// </summary>
		/// <param name="gd"></param>
		/// <param name="radius"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		internal Texture2D CreateCircleTexture(GraphicsDevice gd, int radius, Color color)
		{
			Texture2D texture = new Texture2D(gd, radius, radius);
			Color[] colorData = new Color[radius * radius];

			float diam = radius / 2f;
			float diamsq = diam * diam;

			for (int x = 0; x < radius; x++)
			{
				for (int y = 0; y < radius; y++)
				{
					int index = x * radius + y;
					Vector2 pos = new Vector2(x - diam, y - diam);
					if (pos.LengthSquared() <= diamsq)
					{
						colorData[index] = color;
					}
					else
					{
						colorData[index] = Color.TransparentBlack;
					}
				}
			}

			texture.SetData(colorData);
			return texture;
		}

		public QRenderer2D(GraphicsDevice device, RenderModes mode) : base(device)
		{
			_batch = new SpriteBatch(device);
			SetRenderMode(mode);
		}

		public QRenderer2D(GraphicsDevice device) : base(device)
		{
			_batch = new SpriteBatch(device);
			SetRenderMode(RenderModes.SpriteMajority);
		}

		public void Dispose()
		{
			if(_batch.IsDisposed == false)
				_batch.Dispose();
		}

		public string Name { get; set; }
		public Type Type { get; set; }
		public void Destroy() {}
	}
}