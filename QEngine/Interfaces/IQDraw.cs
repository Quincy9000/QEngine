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
	/// </summary>
	public interface IQDraw
	{
		void Draw(QRenderer2D render);
	}
}
