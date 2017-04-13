using QEngine.Interfaces;
using QEngine.System;

namespace QEngine.Prefabs.Base
{
	public abstract class QStaticDrawable : QBehavior, IQLoad, IQStart, IQDraw
	{
		protected QStaticDrawable() : base("QStaticDrawable") { }

		public virtual void Load()
		{
			
		}

		public virtual void Start()
		{
			
		}
	}
}
