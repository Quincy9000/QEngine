using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine.System
{
	/// <summary>
	/// Helper class to create maps from image files(png)
	/// </summary>
	public static class QMap
	{
		[ImmutableObject(true)]
		public struct TilePos
		{
			public Rectangle Source;
			public Vector2 Position;
		}

		public delegate Rectangle TileMapper(Color color);

		public delegate void ObjectCreator(Color color, Vector2 pos);

		//Instantiates objects at positions of a QMap
		static void ObjectSceneLoader(Color[,] colors, Vector2 startingPos, Vector2 tileSize, ObjectCreator spawner)
		{
			Vector2 pos = startingPos;
			for(int i = 0; i < colors.GetLength(0); ++i, pos.X = startingPos.X, pos.Y += tileSize.Y)
			{
				for(int j = 0; j < colors.GetLength(1); ++j, pos.X += tileSize.X)
				{
					spawner(colors[i, j], pos);
				}
			}
		}

		//turn array into 2d array
		static T[,] ConvertArray2D<T>(IReadOnlyList<T> array, int width, int height)
		{
			if(width == 0 || height == 0)
				throw new DivideByZeroException();
			var element = 0;
			var array2D = new T[height, width];
			var w = (int)Math.Round((double)array.Count / height);
			var h = (int)Math.Round((double)array.Count / width);
			for (var i = 0; i < h; i++)
			{
				for(var j = 0; j < w; j++)
				{
					array2D[i, j] = array[element++];
				}
			}
			return array2D;
		}

		//turns list of colors into list of rectangles and positions
		static List<TilePos> CompileLayer(Color[,] colors, Vector2 startingPos, Vector2 scale, TileMapper layer)
		{
			var tiles = new List<TilePos>();
			var pos = startingPos;
			for(int i = 0; i < colors.GetLength(0); i++)
			{
				for(int j = 0; j < colors.GetLength(1); j++)
				{
					var r = layer(colors[i, j]);
					if(r != Rectangle.Empty)
					{
						tiles.Add(new TilePos()
						{
							Source = r,
							Position = pos
						});
					}
					pos.X += scale.X;
				}
				pos.X = startingPos.X;
				pos.Y += scale.Y;
			}
			return tiles;
		}

		public static List<TilePos> CreateSpriteLayer(Texture2D texture, Vector2 startingPos, Vector2 scale, TileMapper layer)
		{
			Color[] colors = new Color[texture.Width * texture.Height];
			texture.GetData(colors);
			var c = ConvertArray2D(colors, texture.Width, texture.Height);
			return CompileLayer(c, startingPos, scale, layer);
		}

		public static void SpawnObjects(Texture2D texture, Vector2 startingPos, Vector2 scale, ObjectCreator spawner)
		{
			Color[] colors = new Color[texture.Width * texture.Height];
			texture.GetData(colors);
			var c = ConvertArray2D(colors, texture.Width, texture.Height);
			ObjectSceneLoader(c, startingPos, scale, spawner);
		}
	}
}