// Developed With Love by Ryan Boyer https://ryanjboyer.com <3

using UnityEngine;
using UnityEditor;
using Foundation.Editors;

namespace XcodeProjectSettings {
	[CustomEditor(typeof(ProjectSettings))]
	internal sealed class ProjectSettingsEditor : Editor {
		// MARK: - GUI

		public override void OnInspectorGUI() {
			serializedObject.Update();

			OnMainGUI();
			OnInfoPlistGUI();

			serializedObject.ApplyModifiedProperties();
		}

		private void OnMainGUI() {
			using (new FoundationEditorGUI.BoxGroupScope()) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty(Properties.enableBitcode), Styles.enableBitcode);
			}
		}

		private void OnInfoPlistGUI() {
			using (new FoundationEditorGUI.BoxGroupScope(Styles.infoPlist)) {
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

		// MARK: - Utility

		private static GUIContent MakeHelpIcon(in string tooltip)
			=> EditorGUIUtility.IconContent(Icon.HELP, tooltip);

		// MARK: - Constants

		internal static class Icon {
			public const string HELP = "_Help";
		}

		internal static class Properties {
			public const string enableBitcode = "enableBitcode";
		}

		public sealed class Styles {
			public static readonly GUIContent enableBitcode = new GUIContent("Enable Bitcode");

			public static readonly GUIContent infoPlist = new GUIContent("Info.plist");

			public static readonly GUIContent displayName = InfoPlistElement.SearchCommon("displayName").editorLabel;
			public static readonly GUIContent accessibilityBundleName = InfoPlistElement.SearchCommon("accessibilityBundleName").editorLabel;
			public static readonly GUIContent disableMinimumFramerate = InfoPlistElement.SearchCommon("disableMinimumFramerate").editorLabel;
			public static readonly GUIContent appUsesNonExemptEncryption = InfoPlistElement.SearchCommon("appUsesNonExemptEncryption").editorLabel;
			public static readonly GUIContent applicationRequiresIPhoneEnvironment = InfoPlistElement.SearchCommon("applicationRequiresIPhoneEnvironment").editorLabel;
		}
	}
}