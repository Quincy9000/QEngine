using System;
using System.Collections.Generic;

namespace QEngine.System
{
	/// <summary>
	/// QNode object pool to grab objects for the scene when it needs to
	/// </summary>
	internal static class QPool
	{
		static QPool()
		{
			var tempList = new List<QNode>();
			for(var i = 0; i < PoolSize; ++i)
			{
				tempList.Add(GetNode());
			}
			for (var i = 0; i < PoolSize; ++i)
			{
				FreeNode(tempList[i]);
			}
		}

		const int PoolSize = 100;

		static readonly Stack<QNode> Pool = new Stack<QNode>();

		static int _totalNodes;

		public static QNode GetNode() => Pool.Count > 0 ? Pool.Pop() : new QNode("QNode", "Default", _totalNodes++);

		public static void FreeNode(QNode node) => Pool.Push(node);
	}
}