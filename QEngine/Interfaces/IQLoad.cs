using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEngine.Interfaces
{
	/// <summary>
	/// The first call that happens when a QNode is created in a QScene
	/// </summary>
	public interface IQLoad
	{
		void Load();
	}
}
