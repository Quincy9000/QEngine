using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QEngine.System;

namespace QEngine.Interfaces
{
	/// <summary>
	/// Draw call that gets called in the order it was created
	/// Inherit this if you want to control how the sprite it rendered to the screen 
	/// else you dont need this to draw stuff
	/// </summary>
	public interface IQDraw
	{
		void Draw(QRenderer2D render);
	}
}
