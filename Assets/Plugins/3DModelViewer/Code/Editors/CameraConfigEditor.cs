using Plugins._3DModelViewer.Code.Camera;
using UnityEditor;
using UnityEngine;

namespace Plugins._3DModelViewer.Code.Editors
{
    [CustomEditor(typeof(CameraConfig))]
    public class CameraConfigEditor : Editor
    {
        private SerializedProperty defaultRadius;
        private SerializedProperty rotationSpeed;
        private SerializedProperty rotationAltitudeLimits;
        private SerializedProperty zoomSpeed;
        private SerializedProperty zoomLimits;
        private SerializedProperty panSpeed;
        private SerializedProperty panLimit;
        private const float PIBY2 = 1.57f; 

        private void OnEnable()
        {
            defaultRadius = serializedObject.FindProperty(nameof(CameraConfig.defaultRadius));
            rotationSpeed = serializedObject.FindProperty(nameof(CameraConfig.rotationSpeed));
            rotationAltitudeLimits = serializedObject.FindProperty(nameof(CameraConfig.rotationAltitudeLimits));
            zoomSpeed = serializedObject.FindProperty(nameof(CameraConfig.zoomSpeed));
            zoomLimits = serializedObject.FindProperty(nameof(CameraConfig.zoomLimits));
            panSpeed = serializedObject.FindProperty(nameof(CameraConfig.panSpeed));
            panLimit = serializedObject.FindProperty(nameof(CameraConfig.panLimit));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(defaultRadius);

            // Rotation Settings
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rotation Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(rotationSpeed, new GUIContent("Rotation Speed"));
            EditorGUILayout.LabelField("How far can you look vertically. The value ranges from -Pi/2 to +Pi/2.", EditorStyles.helpBox);
            EditorGUILayout.LabelField("Rotation Altitude Limits");
            EditorGUI.BeginChangeCheck();
            var rotationLow = rotationAltitudeLimits.vector2Value.x;
            var rotationHigh = rotationAltitudeLimits.vector2Value.y;
            EditorGUILayout.MinMaxSlider(ref rotationLow, ref rotationHigh, -PIBY2, PIBY2);
            rotationLow = EditorGUILayout.FloatField("Minimum Altitude", rotationLow);
            rotationHigh = EditorGUILayout.FloatField("Maximum Altitude", rotationHigh);
            if (EditorGUI.EndChangeCheck())
            {
                rotationAltitudeLimits.vector2Value = new Vector2(rotationLow, rotationHigh);
            }
            
            // Zoom Settings
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Zoom Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(zoomSpeed, new GUIContent("Zoom Speed"));
            EditorGUILayout.LabelField("How close and far can you zoom.", EditorStyles.helpBox);
            EditorGUI.BeginChangeCheck();
            var zoomLow = zoomLimits.vector2Value.x;
            var zoomHigh = zoomLimits.vector2Value.y;
            EditorGUILayout.MinMaxSlider(ref zoomLow, ref zoomHigh, 0f, 100f);
            zoomLow = EditorGUILayout.FloatField("Minimum Distance", zoomLow);
            zoomHigh = EditorGUILayout.FloatField("Maximum Distance", zoomHigh);
            if (EditorGUI.EndChangeCheck())
            {
                zoomLimits.vector2Value = new Vector2(zoomLow, zoomHigh);
            }

            // Pan Settings
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Pan Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(panSpeed, new GUIContent("Pan Speed"));
            EditorGUILayout.LabelField("The maximum magnitude of the offset vector.", EditorStyles.helpBox);
            EditorGUILayout.PropertyField(panLimit, new GUIContent("Pan Limit"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
