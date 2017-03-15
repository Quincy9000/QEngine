using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEngine.Interfaces
{
	/// <summary>
	/// This method gets called right before the object is removed from the scene
	/// </summary>
	public interface IQDestroy
	{
		void Destroy();
	}
}
