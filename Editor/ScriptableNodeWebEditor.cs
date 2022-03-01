using FedoraDev.PointOfInterest.Implementations;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace FedoraDev.PointOfInterest.Editor
{
    public class ScriptableNodeWebEditor : PropertyDrawer
    {
		[OnOpenAsset]
		public static bool OpenNodeWeb(int instanceID, int line)
		{
			ScriptableNodeWeb nodeWeb = EditorUtility.InstanceIDToObject(instanceID) as ScriptableNodeWeb;
			if (nodeWeb == null)
				return false;

			PointOfInterestNodeEditor.NodeWeb = nodeWeb;
			PointOfInterestNodeEditor.OpenWindow();
			return true;
		}
	}
}
