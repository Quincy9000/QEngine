using QEngine.Interfaces;
using QEngine.System;

namespace QEngine.Prefabs.Base
{
	public abstract class QCharacterController : QBehavior, IQLoad, IQStart, IQFixedUpdate, IQUpdate, IQDraw
	{
		protected QCharacterController() : base("QCharacterController") { }

		public abstract void Load();

		public abstract void Start();

		public abstract void FixedUpdate(QTime time);

		public abstract void Update(QTime time);

		public abstract void Draw(QRenderer2D render);
	}
}
