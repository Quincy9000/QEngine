using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace QEngine.System
{
	public class QWorld
	{
		public World Core { get; }

		internal bool IsHostOfConnection { get; set; } = true;

		internal QWorld(Vector2 gravity)
		{
			Core = new World(gravity);
		}

		public void Clear() => Core.Clear();

		public Vector2 Gravity
		{
			get
			{
				return Core.Gravity;
			}
			set
			{
				Core.Gravity = value;
			}
		}

		internal void Step(QTime time)
		{
			Core.Step(time.FixedDelta);
		}

		internal void Step(float time)
		{
			Core.Step(time);
		}

		public void ClearCollisionEvents()
		{
			Core.ContactManager.OnBroadphaseCollision = null;
		}

		public event BroadphaseDelegate OnBroadPhaseCollision
		{
			add
			{
				Core.ContactManager.OnBroadphaseCollision += value;
			}
			remove
			{
				Core.ContactManager.OnBroadphaseCollision -= value;
			}
		}

		public void RemoveBody(QRigidbody rigidBody)
		{
			Core.RemoveBody(rigidBody.Core);
			rigidBody.Destroy();
		}
	}
}