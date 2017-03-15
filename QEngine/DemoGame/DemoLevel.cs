using QEngine.System;

namespace QEngine.DemoGame
{
	class DemoLevel : QScene
	{
		protected override void OnLoad()
		{
			Instantiate(new TestScript());
			Instantiate(new Block());
		}

		public DemoLevel() : base("DemoLevel") {}
	}
}