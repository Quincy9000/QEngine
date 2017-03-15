using System;
using System.Collections.Generic;
using QEngine.Interfaces;

namespace QEngine.System
{
	public class QNode
	{
		internal Dictionary<Type, QComponent> Components { get; }

		internal QBehavior Script { get; set; }

		public QScene Scene { get; internal set; }

		public string Name { get; set; }

		public string Tag { get; set; }

		public int Id { get; }

		public QTransform2D Transform { get; }

		internal void AddComponent(QComponent component)
		{
			Components.Add(component.Type, component);
		}

		internal void ClearComponents()
		{
			foreach(var c in Components)
			{
				c.Value.Destroy();
			}
			Components.Clear();
		}

		internal QNode(string name, string tag, int id)
		{
			Name = name;
			Tag = tag;
			Id = id;
			Transform = new QTransform2D();
			Components = new Dictionary<Type, QComponent>();
		}

		internal void Reset()
		{
			Transform.Reset();
			ClearComponents();
		}
	}
}