using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace QEngine.System
{
	public abstract class QRenderer
	{
		protected GraphicsDevice device;

		protected QRenderer(GraphicsDevice device)
		{
			this.device = device;
		}
	}
}
