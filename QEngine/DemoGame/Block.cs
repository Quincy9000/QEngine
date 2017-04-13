using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using QEngine.Prefabs.Base;
using QEngine.System;

namespace QEngine.DemoGame
{
	class Block : QStaticDrawable
	{
		QSprite sprite;

		QRigidbody body;

		public int Width = 100;

		public int Height => Width;

		public override void Load()
		{
			LoadTexture("Block", Width, Height, Color.Red);
			LoadSprite();
			LoadRigidbody();
		}

		public override void Start()
		{
			sprite = GetComponent<QSprite>();
			sprite.Source = GetSource("Block");
			sprite.Origin = sprite.Source.Center();
			body = GetComponent<QRigidbody>().CreateSquare(Width, 10f, Transform.Position, this);
			body.BodyType = BodyType.Static;
			Transform.Position = new Vector2(0, 100);
		}
	}
}
