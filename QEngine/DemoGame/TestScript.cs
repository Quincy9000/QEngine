using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using QEngine.Interfaces;
using QEngine.System;

namespace QEngine.DemoGame
{
	class TestScript : QBehavior, IQLoad, IQStart, IQUpdate, IQDraw
	{
		QSprite sprite;

		QRigidbody body;

		public TestScript() : base("TestScript") {}

		public void Load()
		{
			//grabbing a texture from the content folder
			LoadTexture("Sprites/cyberPunk/layers/foreground");
			//adding a sprite to the component system to use later
			LoadSprite();
			//adding a rigidbody to the component system to use later
			LoadRigidbody();
		}

		public void Start()
		{
			//Gragging the sprite from the component system
			sprite = GetComponent<QSprite>();
			sprite.Source = GetSource("foreground");
			sprite.Origin = sprite.Source.Center();

			//grabbing the rigidbody that is attached to the node and giving it details
			body = GetComponent<QRigidbody>().CreateRectangle(sprite.Source.Width, sprite.Source.Height);
			body.BodyType = BodyType.Dynamic;
			body.FixedRotation = true;
			World.Gravity = new Vector2(0, 20);
		}

		public void Update(float delta)
		{
			Console.WriteLine($"Delta: " + DebugSettings.Fps + " FixedDelta: " + 1 / Window.FixedDeltaTime);
			var s = 500;
			if(QControls.KeyPressed(Keys.P))
				QScene.TakeScreenShot(Scene);
			if(QControls.KeyDown(Keys.Escape))
				Parent.Scene.Window.Exit();
			if(QControls.KeyDown(Keys.R))
				Parent.Scene.Window.ResetCurrentScene();
			if(QControls.KeyDown(Keys.W))
				Transform.Position += new Vector2(0, -1) * delta * s;
			if(QControls.KeyDown(Keys.A))
				Transform.Position += new Vector2(-1, 0) * delta * s;
			if(QControls.KeyDown(Keys.S))
				Transform.Position += new Vector2(0, 1) * delta * s;
			if(QControls.KeyDown(Keys.D))
				Transform.Position += new Vector2(1, 0) * delta * s;
		}
	}
}