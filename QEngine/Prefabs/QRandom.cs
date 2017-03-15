using System;

namespace QEngine.Prefabs
{
	/// <summary>
	/// Random number generator
	/// </summary>
	public static class QRandom
	{
		private class Rng : Random
		{
			public override int Next(int minValue, int maxValue)
			{
				return base.Next(minValue, maxValue + 1);
			}
		}

		static readonly Rng R = new Rng();

		public static int Dice(int dice, int sides)
		{
			int total = 0;
			for(int i = 0; i < dice; i++)
			{
				total += Range(1, sides);
			}
			return total;
		}

		/// <summary>
		/// Inclusive Random Range
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <returns></returns>
		public static int Range(int min, int max)
		{
			return R.Next(min, max);
		}

		public static double Percent()
		{
			return R.NextDouble();
		}
	}
}