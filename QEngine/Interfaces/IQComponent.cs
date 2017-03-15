using System;

namespace QEngine.Interfaces
{
	internal interface IQComponent
	{
		string Name { get; }

		Type Type { get; }

		void Destroy();
	}
}
