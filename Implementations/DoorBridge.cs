using FedoraDev.PointOfInterest.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	public class DoorBridge : INodeBridge
	{
		public string Name => "Door";
		public Vector2 Size => new Vector2(75, 50);
		public INodeBridgeConnection[] Connections { get => _connections; set => _connections = value; }

		public Rect Position
		{
			get
			{
				Rect rect = _position;
				rect.position = new Vector2((Mathf.Round(rect.position.x / 10) * 10) + (Size.x / 2), (Mathf.Round(rect.position.y / 10) * 10) + (Size.y / 2));
				return rect;
			}
			set => _position = value;
		}

		[SerializeField, ReadOnly] INodeBridgeConnection[] _connections = new INodeBridgeConnection[0];
		[SerializeField, ReadOnly] Rect _position;

		bool _isDragged;

		public bool ProcessEvents(Event currentEvent) => NodeDragHelper.ProcessDrag(currentEvent, Move, Place, Position, ref _isDragged);
		public INodeBridge CreateCopy() => new DoorBridge();
		public void Place()
		{
			Rect rect = _position;
			rect.position = new Vector2(Mathf.Round(rect.position.x / 10) * 10, Mathf.Round(rect.position.y / 10) * 10);
			_position = rect;
		}
		public void Move(Vector2 delta) => _position.position += delta;
	}
}
