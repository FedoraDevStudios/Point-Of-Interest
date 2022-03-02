using FedoraDev.PointOfInterest.Abstract;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Implementations
{
	public class PointOfInterestNode : INode
	{
		public string Name => _pointOfInterest == null ? "New Node" : _pointOfInterest.Name;
		public Vector2 Size => new Vector2(100, 100);
		public IPointOfInterest PointOfInterest { get => _pointOfInterest; set => _pointOfInterest = value; }
		public INodeBridge[] Bridges { get => _bridges; set => _bridges = value; }
		
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

		[SerializeField] IPointOfInterest _pointOfInterest;
		[SerializeField, ReadOnly] INodeBridge[] _bridges = new INodeBridge[0];
		[SerializeField, ReadOnly] Rect _position;

		bool _isDragged = false;

		public bool ProcessEvents(Event currentEvent) => NodeDragHelper.ProcessDrag(currentEvent, Move, Place, Position, ref _isDragged);
		public void Place()
		{
			Rect rect = _position;
			rect.position = new Vector2(Mathf.Round(rect.position.x / 10) * 10, Mathf.Round(rect.position.y / 10) * 10);
			_position = rect;
		}
		public void Move(Vector2 delta) => _position.position += delta;


		public INode CreateCopy()
		{
			return new PointOfInterestNode();
		}
	}
}
