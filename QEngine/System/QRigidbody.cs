using System;
using System.Collections.Generic;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using QEngine.Code;
using QEngine.Interfaces;

namespace QEngine.System
{
	public class QRigidbody : QComponent
	{
		public override string Name { get; protected set; }

		public override Type Type { get; protected set; }

		public Body Core { get; private set; }

		readonly World _world;

		public event OnCollisionEventHandler OnCollision
		{
			add
			{
				Core.OnCollision += value;
			}
			remove
			{
				Core.OnCollision -= value;
			}
		}

		public event OnSeparationEventHandler OnSeparation
		{
			add
			{
				Core.OnSeparation += value;
			}
			remove
			{
				Core.OnSeparation -= value;
			}
		}

		public bool IsDisposed => Core.IsDisposed;

		/// <summary>
		/// completely removes this rigidbody from the currentScene
		/// </summary>
		public override void Destroy()
		{
			if(!IsDisposed)
				Core.Dispose();
		}

		public float Rotation
		{
			get
			{
				return Core.Rotation;
			}
			set
			{
				Core.Rotation = value;
			}
		}

		public Vector2 MaxLinearVelocity { get; set; }

		public Vector2 LinearVelocity
		{
			get
			{
				return Core.LinearVelocity;
			}
			set
			{
				Core.LinearVelocity = value;
			}
		}

		public float Mass
		{
			get
			{
				return Core.Mass;
			}
			set
			{
				Core.Mass = value;
			}
		}

		public T UserData<T>() where T : QBehavior => Core.UserData as T;

		public object UserData() => Core.UserData;

		public bool FixedRotation
		{
			get
			{
				return Core.FixedRotation;
			}
			set
			{
				Core.FixedRotation = value;
			}
		}

		public List<Shape> Shapes
		{
			get
			{
				var shapes = new List<Shape>();
				foreach(var s in Core.FixtureList)
				{
					shapes.Add(s.Shape);
				}
				return shapes;
			}
		}

		public List<Fixture> Fixtures => Core.FixtureList;

		public float Friction
		{
			get
			{
				return Core.Friction;
			}
			set
			{
				Core.Friction = value;
			}
		}

		public void ApplyForce(Vector2 force)
		{
			Core.ApplyForce(force);
		}

		public bool Enabled
		{
			get
			{
				return Core.Enabled;
			}
			set
			{
				Core.Enabled = value;
			}
		}

		public bool AllowSleep
		{
			get
			{
				return Core.SleepingAllowed;
			}
			set
			{
				Core.SleepingAllowed = value;
			}
		}

		public float LinearDamping
		{
			get
			{
				return Core.LinearDamping;
			}
			set
			{
				Core.LinearDamping = value;
			}
		}

		public float Gravity
		{
			get
			{
				return Core.GravityScale;
			}
			set
			{
				Core.GravityScale = value;
			}
		}

		public float Restitution
		{
			get
			{
				return Core.Restitution;
			}
			set
			{
				Core.Restitution = value;
			}
		}

		public bool IgnoreGravity
		{
			get
			{
				return Core.IgnoreGravity;
			}
			set
			{
				Core.IgnoreGravity = value;
			}
		}

		public bool IsBullet
		{
			get
			{
				return Core.IsBullet;
			}
			set
			{
				Core.IsBullet = value;
			}
		}

		public bool IsSensor
		{
			set
			{
				Core.IsSensor = value;
			}
		}

		public int BodyId => Core.BodyId;

		public bool Awake
		{
			get
			{
				return Core.Awake;
			}
			set
			{
				Core.Awake = value;
			}
		}

		public bool IsIgnoreCcd
		{
			get
			{
				return Core.IgnoreCCD;
			}
			set
			{
				Core.IgnoreCCD = value;
			}
		}

		public void IgnoreCollisionWith(QRigidbody rigidBody)
		{
			Core.IgnoreCollisionWith(rigidBody.Core);
		}

		public BodyType BodyType
		{
			get
			{
				return Core.BodyType;
			}
			set
			{
				Core.BodyType = value;
			}
		}

		public Vector2 Position
		{
			get
			{
				return Core.Position.ToDis();
			}
			set
			{
				Awake = true;
				Core.Position = value.ToSim();
			}
		}

		public QRigidbody CreateEmpty(Vector2 position = default(Vector2), float rotation = 0f, QBehavior userData = null)
		{
			if(Core != null)
				Destroy();
			Core = BodyFactory.CreateBody(_world, position.ToSim(), rotation, userData);
			return this;
		}

		public QRigidbody CreateEdge(Vector2 start = default(Vector2), Vector2 end = default(Vector2), QBehavior userData = null)
		{
			if(Core != null && !IsDisposed)
				Destroy();
			Core = BodyFactory.CreateEdge(_world, start.ToSim(), end.ToSim(), userData);
			return this;
		}

		public QRigidbody CreateRectangle(float width = 1f, float height = 1f, float density = 1f, Vector2 position = default(Vector2), QBehavior userData = null)
		{
			if(Core != null)
				Destroy();
			Core = BodyFactory.CreateRectangle(_world, width.ToSim(), height.ToSim(), density, position.ToSim(), userData);
			return this;
		}

		public QRigidbody CreateSquare(float length = 1f, float density = 1f, Vector2 position = default(Vector2), QBehavior userData = null)
		{
			if(Core != null)
				Destroy();
			Core = BodyFactory.CreateRectangle(_world, length.ToSim(), length.ToSim(), density, position.ToSim(), userData);
			return this;
		}

		public QRigidbody CreateCircle(float radius = 1f, float density = 1f, Vector2 position = default(Vector2), QBehavior userData = null)
		{
			if(Core != null)
				Destroy();
			Core = BodyFactory.CreateCircle(_world, radius.ToSim(), density, userData);
			Core.Position = position.ToSim();
			return this;
		}

		public QRigidbody CreateCapsule(float width = 1f, float endRadius = 1f, float density = 1f, Vector2 position = default(Vector2), QBehavior userData = null)
		{
			if(Core != null)
				Destroy();
			Core = BodyFactory.CreateCapsule(_world, width.ToSim(), endRadius.ToSim(), density, userData);
			Core.Position = position.ToSim();
			return this;
		}

		public QRigidbody CreateCapsule(float height, float topRadius, int topEdges, float bottomRadius, int bottomEdges, float density = 1f, Vector2 position = default(Vector2), QBehavior userData = null)
		{
			if(Core != null)
				Destroy();
			Core = BodyFactory.CreateCapsule(_world, height.ToSim(), topRadius.ToSim(), topEdges, bottomRadius.ToSim(), bottomEdges, density, position.ToSim(), userData);
			return this;
		}

		public List<Fixture> RayCast(Vector2 v, Vector2 v2)
		{
			return _world.RayCast(v.ToSim(), v2.ToSim());
		}

		public bool RayCastHitDistance(Vector2 a, Vector2 b, out float distanceFromHit)
		{
			var aSim = a.ToSim();
			var bSim = b.ToSim();
			var fl = _world.RayCast(aSim, aSim + bSim);
			if(fl.Count > 0)
			{
				distanceFromHit = Vector2.Distance(a, fl[0].Body.Position.ToDis());
				return true;
			}
			distanceFromHit = -1;
			return false;
		}

		public QRigidbody(Body b, QWorld world)
		{
			if(b == null)
				throw new QException("Body was fucking null");
			if(world == null)
				throw new QException("Core is fucking null");
			_world = world.Core;
			Core = b;
			Core.SleepingAllowed = true;
			Core.IgnoreCCD = true;
			Type = typeof(QRigidbody);
		}

		public QRigidbody(QWorld world)
		{
			if(world == null)
				throw new QException("Core is fucking null");
			_world = world.Core;
			Core = null;
			Type = typeof(QRigidbody);
		}
	}
}