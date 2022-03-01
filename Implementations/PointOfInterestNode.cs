using FedoraDev.PointOfInterest.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	public class PointOfInterestNode : INode
	{
		public string Name => _pointOfInterest == null ? "New Node" : _pointOfInterest.Name;
		public IPointOfInterest PointOfInterest { get => _pointOfInterest; set => _pointOfInterest = value; }
		public INodeBridge[] Bridges { get => _bridges; set => _bridges = value; }
		public Rect Position { get => _position; set => _position = value; }

		[SerializeField] IPointOfInterest _pointOfInterest;
		[SerializeField, ReadOnly] INodeBridge[] _bridges = new INodeBridge[0];
		[SerializeField, ReadOnly] Rect _position;

		bool _isDragged = false;

		public bool ProcessEvents(Event currentEvent) => NodeDragHelper.ProcessDrag(currentEvent, ref _position, ref _isDragged);

		public INode CreateCopy()
		{
			return new PointOfInterestNode();
		}
	}
}
