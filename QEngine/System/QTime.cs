using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace QEngine.System
{
	[ImmutableObject(true)]
	public struct QTime
	{
		public QTime(GameTime time, float fdelta)
		{
			Delta = (float)time.ElapsedGameTime.TotalSeconds;
			FixedDelta = fdelta;
		}

		public float Delta { get; }

		public float FixedDelta { get; }
	}
}