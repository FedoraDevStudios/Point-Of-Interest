using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace FedoraDev.PointOfInterest
{
    public interface IHierarchyPiece
    {
        string Name { get; }
        IHierarchyPiece Parent { get; }
        [InlineEditor] IHierarchyPiece[] LocalHierarchy { get; }

        void AssignParents(IHierarchyPiece parent);
        List<IHierarchyPiece> GetPathTo(IHierarchyPiece to);
    }
}
