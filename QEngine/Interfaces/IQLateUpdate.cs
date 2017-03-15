using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QEngine.System;

namespace QEngine.Interfaces
{
	/// <summary>
	/// LateUpdate is the last type of update to be called before any draw calls, it's guaranteed to come after all other updates
	/// </summary>
	public interface IQLateUpdate
	{
		void LateUpdate(QTime time);
	}
}
