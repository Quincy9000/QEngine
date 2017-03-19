using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QEngine.System;

namespace QEngine.Interfaces
{
	/// <summary>
	/// FixedUpdate gets called at a constant interval, can be called more than once an updateloop if falling behind
	/// </summary>
	public interface IQFixedUpdate
	{
		void FixedUpdate(float delta);
	}
}