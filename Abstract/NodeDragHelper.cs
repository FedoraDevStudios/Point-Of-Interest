using UnityEngine;

namespace FedoraDev.PointOfInterest.Abstract
{
    public class NodeDragHelper
    {
		public static bool ProcessDrag(Event currentEvent, ref Rect position, ref bool isDragged)
		{
			switch (currentEvent.type)
			{
				case EventType.MouseDown:
					if (currentEvent.button == 0)
					{
						if (position.Contains(currentEvent.mousePosition))
							isDragged = true;
						GUI.changed = true;
					}
					break;

				case EventType.MouseUp:
					isDragged = false;
					break;

				case EventType.MouseDrag:
					if (currentEvent.button == 0 && isDragged)
					{
						position.position += currentEvent.delta;
						currentEvent.Use();
						return true;
					}
					break;
			}

			return false;
		}
	}
}
