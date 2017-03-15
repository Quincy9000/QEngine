using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QEngine.Interfaces;

namespace QEngine.System
{
	public static class QTextureExtentions
	{
		public static bool IsEven(float number)
		{
			return (int)(number * 1000) % 2 == 0;
		}
	}
}
