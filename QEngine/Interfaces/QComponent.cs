using System;

namespace QEngine.Interfaces
{
	public abstract class QComponent
	{
		public abstract string Name { get; protected set; }
		public abstract Type Type { get; protected set; }
		public abstract void Destroy();
	}
}