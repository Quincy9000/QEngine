using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace QEngine.System
{
	public class QWorld
	{
		public World World { get; }

		internal bool IsHostOfConnection { get; set; } = true;

		public QWorld(Vector2 gravity)
		{
			World = new World(gravity);
		}

		public void Clear() => World.Clear();

		public Vector2 Gravity
		{
			get
			{
				return World.Gravity;
			}
			set
			{
				World.Gravity = value;
			}
		}

		public void Step(QTime time)
		{
			World.Step(time.FixedDelta);
		}

		public void Step(float time)
		{
			World.Step(time);
		}

		public void ClearCollisionEvents()
		{
			World.ContactManager.OnBroadphaseCollision = null;
		}

		public event BroadphaseDelegate OnBroadPhaseCollision
		{
			add
			{
				World.ContactManager.OnBroadphaseCollision += value;
			}
			remove
			{
				World.ContactManager.OnBroadphaseCollision -= value;
			}
		}

		public void RemoveBody(QRigidbody rigidBody)
		{
			World.RemoveBody(rigidBody.Core);
			rigidBody.Destroy();
		}
	}
}