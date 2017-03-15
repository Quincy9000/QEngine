using QEngine.Interfaces;
using QEngine.System;

namespace QEngine.Prefabs.Base
{
	public abstract class QStaticDrawable : QBehavior, IQLoad, IQStart, IQDraw
	{
		protected QStaticDrawable() : base("QStaticDrawable") { }

		public abstract void Load();

		public abstract void Start();

		public abstract void Draw(QRenderer2D render);
	}
}
