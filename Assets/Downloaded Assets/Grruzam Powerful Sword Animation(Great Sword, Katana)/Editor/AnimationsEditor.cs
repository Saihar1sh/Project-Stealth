using UnityEditor;
using UnityEngine;

namespace ShGames
{
    [CustomEditor(typeof(KatanaAnimations))]
    public class AnimationsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var targetAnimations = target as KatanaAnimations;
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Previous"))
            {
                targetAnimations.PlayPreviousAnimation();
            }
            if (GUILayout.Button("Next"))
            {
                targetAnimations.PlayNextAnimation();
            }
            GUILayout.EndHorizontal();
        }
    }
}
