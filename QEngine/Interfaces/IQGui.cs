using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QEngine.System;

namespace QEngine.Interfaces
{
	/// <summary>
	/// Draw call that happens after the initial draw calls that will always be on top of IQDraw calls aka Gui elements 
	/// </summary>
	public interface IQGui
	{
		void Gui(QRenderer2D render);
	}
}
