using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using QEngine.Interfaces;

namespace QEngine.System
{
	public class QRenderer3D : QRenderer
	{
		public QRenderer3D(GraphicsDevice device) : base(device) {}

		public string Name { get; set; }
		public Type Type { get; set; }
		public void Destroy() {}
	}
}