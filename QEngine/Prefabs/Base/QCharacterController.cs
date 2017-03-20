using QEngine.Interfaces;
using QEngine.System;

namespace QEngine.Prefabs.Base
{
	public abstract class QCharacterController : QBehavior, IQLoad, IQStart, IQUpdate, IQDraw
	{
		protected QCharacterController() : base("QCharacterController") { }

		public abstract void Load();

		public abstract void Start();

		public abstract void Update(float delta);

		public abstract void Draw(QRenderer2D render);
	}
}
