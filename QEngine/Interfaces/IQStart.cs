using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEngine.Interfaces
{
	/// <summary>
	/// Gets called once before any update calls happen
	/// </summary>
	public interface IQStart
	{
		void Start();
	}
}
