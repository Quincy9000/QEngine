using System;
using System.Collections.Generic;

namespace QEngine.System
{
	internal static class QPool
	{
		static readonly Stack<QNode> Pool = new Stack<QNode>();

		static int _totalNodes;

		public static QNode GetNode()
		{
//			if(Pool.Count > 0)
//			{
//				Console.WriteLine("pop");
//				return Pool.Pop();
//			}
//			Console.WriteLine("new");
//			return new QNode("QNode", "Default", _totalNodes++);
			return Pool.Count > 0 ? Pool.Pop() : new QNode("QNode", "Default", _totalNodes++);
		}

		public static void FreeNode(QNode node)
		{
			Pool.Push(node);
		}
	}
}