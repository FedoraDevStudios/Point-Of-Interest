using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	public class SimpleHierarchy : IHierarchy
	{
		public IHierarchyPiece Root => _root;

		[SerializeField, InlineEditor] IHierarchyPiece _root;

		public IHierarchyPiece[] GetPath(IHierarchyPiece from, IHierarchyPiece to)
		{
			if (from.Name == to.Name)
				return new IHierarchyPiece[] { from };

			List<IHierarchyPiece> pathFrom = Root.GetPathTo(from); // A -> B -> C -> D
			List<IHierarchyPiece> pathTo = Root.GetPathTo(to); // A -> B -> F -> G -> H

			if (pathFrom == null || pathTo == null)
				return null;

			while (pathFrom[0].Name == pathTo[0].Name)
			{
				pathFrom.RemoveAt(0); //C -> D
				pathTo.RemoveAt(0); //F -> G -> H
			}

			for (int i = 0; i < pathFrom.Count; i++)
				pathTo.Insert(0, pathFrom[i]); //D -> C -> F -> G -> H

			return pathTo.ToArray();
		}
	}
}
