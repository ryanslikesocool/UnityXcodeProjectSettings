using UnityEngine;
using UnityEditor;

namespace XcodeProjectSettings {
	[CustomEditor(typeof(XcodeProjectSettings))]
	internal sealed class XcodeProjectSettingsEditor : Editor {
		public sealed class Styles {
			public static readonly GUIContent enableBitcode = new GUIContent("Enable Bitcode");

			public static readonly GUIContent displayName = InfoPlistElement.SearchCommon("displayName").editorLabel;
			public static readonly GUIContent accessibilityBundleName = InfoPlistElement.SearchCommon("accessibilityBundleName").editorLabel;
			public static readonly GUIContent disableMinimumFramerate = InfoPlistElement.SearchCommon("disableMinimumFramerate").editorLabel;
			public static readonly GUIContent appUsesNonExemptEncryption = InfoPlistElement.SearchCommon("appUsesNonExemptEncryption").editorLabel;
			public static readonly GUIContent applicationRequiresIPhoneEnvironment = InfoPlistElement.SearchCommon("applicationRequiresIPhoneEnvironment").editorLabel;
		}
		private static GUIContent MakeHelpIcon(string tooltip)
			=> EditorGUIUtility.IconContent("_Help", tooltip);

		public override void OnInspectorGUI() {
			serializedObject.Update();

			OnGUI_Main();
			OnGUI_InfoPlist();

			serializedObject.ApplyModifiedProperties();
		}

		private void OnGUI_Main() {
			using (new EditorGUILayout.VerticalScope("HelpBox")) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty("enableBitcode"), Styles.enableBitcode);
			}
		}

		private void OnGUI_InfoPlist() {
			using (new EditorGUILayout.VerticalScope("HelpBox")) {
				EditorGUILayout.LabelField("Info.plist", EditorStyles.boldLabel);

				foreach (InfoPlistElement element in InfoPlistElement.Common) {
					using (new EditorGUILayout.HorizontalScope()) {
						EditorGUILayout.LabelField(element.editorLabel);
						EditorGUILayout.PropertyField(serializedObject.FindProperty(element.propertyName), GUIContent.none);
						if (!string.IsNullOrEmpty(element.helpURL)) {
							if (GUILayout.Button(MakeHelpIcon(element.helpURL), EditorStyles.iconButton)) {
								Application.OpenURL(element.helpURL);
							}
						} else {
							GUILayout.Space(EditorStyles.iconButton.fixedWidth);
						}
					}
				}
			}
		}
	}
}