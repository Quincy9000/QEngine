using QEngine.Interfaces;
using QEngine.System;

namespace QEngine.Prefabs.Base
{
	public abstract class QLoadable : QBehavior, IQLoad
	{
		protected QLoadable() : base("QLoadable") {}

		public abstract void Load();
	}
}