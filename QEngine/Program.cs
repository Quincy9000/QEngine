using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QEngine.DemoGame;
using QEngine.System;

namespace QEngine
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			using(var q = new QWindow("QEngine", "Content/", 1280, 600, args))
			{
				//q.IsFullScreen = true;
				q.Run(new DemoLevel());
			}
		}
	}
}
