using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QEngine.System;

namespace QEngine.Interfaces
{
	/// <summary>
	/// FastUpdate is the main update call, it only gets called one an update loop
	/// </summary>
	public interface IQUpdate
	{
		void Update(float delta);
	}
}
