using FedoraDev.PointOfInterest.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	public class DoorBridge : INodeBridge
	{
		public string Name => "Door";
		public INodeBridgeConnection[] Connections { get => _connections; set => _connections = value; }
		public Rect Position { get => _position; set => _position = value; }

		[SerializeField, ReadOnly] INodeBridgeConnection[] _connections = new INodeBridgeConnection[0];
		[SerializeField, ReadOnly] Rect _position;

		bool _isDragged;

		public bool ProcessEvents(Event currentEvent) => NodeDragHelper.ProcessDrag(currentEvent, ref _position, ref _isDragged);
		public INodeBridge CreateCopy() => new DoorBridge();
	}
}
