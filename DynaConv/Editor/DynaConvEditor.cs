using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Rickter.Lavender.Tools.Adapters {

	[CustomEditor(typeof(DynaConv))]
	public class DynaConvEditor : Editor
	{
		DynaConv adapter = null;
		private static GUIContent postReplacementActionLabel = new GUIContent("Post-Replacement Action", "What should we do with the OG components once Jiggle replacements are added?");
		bool checkboxStateProxy = false;


		public void OnEnable()
		{
			adapter = (DynaConv) target;
			checkboxStateProxy = adapter.enforceOriginalValues;
		}

		public override void OnInspectorGUI()
		{
			EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode);

				EditorGUI.BeginChangeCheck();

					adapter.postReplacementAction = (PostReplacementActions)EditorGUILayout.EnumPopup(postReplacementActionLabel, adapter.postReplacementAction);

					EditorGUILayout.Space();

					checkboxStateProxy = EditorGUILayout.ToggleLeft("Force values into existing JiggleBone/Colliders?", checkboxStateProxy);
					adapter.enforceOriginalValues = (bool) checkboxStateProxy;

				if (EditorGUI.EndChangeCheck()) {
					EditorSceneManager.MarkSceneDirty(adapter.gameObject.scene);
				}

				EditorGUILayout.Space();

				if (GUILayout.Button("Run Conversion", GUILayout.Height(25))) {
					adapter.RunConversion();
				}


			EditorGUI.EndDisabledGroup();
		}
	}

}