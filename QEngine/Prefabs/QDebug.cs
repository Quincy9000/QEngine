using System;
using Microsoft.Xna.Framework.Input;
using QEngine.Interfaces;
using QEngine.System;

namespace QEngine.Prefabs
{
	/// <summary>
	/// Debuger that displays fps
	/// </summary>
	public sealed class QDebug : QBehavior, IQLoad
	{
		QFrameCounter fps;

		public float Fps => fps.CurrentFramesPerSecond;

		public bool IsDebugMode;

		public QDebug() : base("QDebug") { }

		public void Load()
		{
			IsDebugMode = false;
			fps = new QFrameCounter();
		}

		public void Update(float delta)
		{
			if(QControls.KeyDown(Keys.F12))
				IsDebugMode = !IsDebugMode;
			fps.Update(delta);
			if(IsDebugMode)
				Console.WriteLine(Fps);
		}
	}
}