// Developed With Love by Ryan Boyer https://ryanjboyer.com <3

using System.Collections.Generic;
using Foundation.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace XcodeProjectSettings {
	[CustomEditor(typeof(ProjectSettings))]
	internal sealed class ProjectSettingsEditor : Editor {
		[SerializeField] private StyleSheet styleSheet = default;

		// MARK: - GUI

		public override void OnInspectorGUI() {
			serializedObject.Update();

			OnMainGUI();

			OnSectionGUI(InfoPlistElement.BundleName, Styles.infoPlist);
			OnSectionGUI(InfoPlistElement.Versioning, null);
			OnSectionGUI(InfoPlistElement.Common, null);

			serializedObject.ApplyModifiedProperties();
		}

		private void OnMainGUI() {
			using (new FoundationEditorGUI.BoxGroupScope()) {
				EditorGUILayout.PropertyField(serializedObject.FindProperty(Properties.enableBitcode), Styles.enableBitcode);
			}
		}

		private void OnSectionGUI(IEnumerable<InfoPlistElement> collection, GUIContent title) {
			using (new FoundationEditorGUI.BoxGroupScope(title)) {
				foreach (InfoPlistElement element in collection) {
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

		public override VisualElement CreateInspectorGUI() {
			VisualElement root = new();
			root.styleSheets.Add(styleSheet);

			//root.Add(CreateInfoPlistItem());

			root.Add(CreateInfoPlistItems(InfoPlistElement.BundleName));
			root.Add(CreateInfoPlistItems(InfoPlistElement.Versioning));
			root.Add(CreateInfoPlistItems(InfoPlistElement.Common));

			return root;

			VisualElement CreateInfoPlistItems(IEnumerable<InfoPlistElement> elements) {
				VisualElement result = new();

				foreach (InfoPlistElement element in elements) {
					VisualElement elementView = CreateInfoPlistItem(element);
					result.Add(elementView);
				}

				return result;
			}

			VisualElement CreateInfoPlistItem(InfoPlistElement element) {
				VisualElement result = new();
				result.AddToClassList(USS.ListRow);

				PropertyField propertyField = new PropertyField(serializedObject.FindProperty(element.propertyName));
				propertyField.style.flexGrow = 1;
				result.Add(propertyField);

				if (!string.IsNullOrEmpty(element.helpURL)) {
					Button helpButton = new Button(OnHelpButton) {
#if UNITY_2023_2_OR_NEWER
						iconImage = Background.FromTexture2D(EditorGUIUtility.FindTexture(Icon.HELP))
#endif
					};
					helpButton.AddToClassList(USS.HelpIcon);

					result.Add(helpButton);
				}

				return result;

				void OnHelpButton() {
					Application.OpenURL(element.helpURL);
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

		internal static class USS {
			public const string ListRow = "list-row";
			public const string HelpIcon = "help-icon";
		}

		public sealed class Styles {
			public static readonly GUIContent enableBitcode = new("Enable Bitcode");

			public static readonly GUIContent infoPlist = new("Info.plist");

			//public static readonly GUIContent displayName = InfoPlistElement.SearchPresets("displayName").editorLabel;
			//public static readonly GUIContent accessibilityBundleName = InfoPlistElement.SearchPresets("accessibilityBundleName").editorLabel;

			//public static readonly GUIContent bundleVersion = InfoPlistElement.SearchPresets("bundleVersion").editorLabel;
			//public static readonly GUIContent bundleVersionShort = InfoPlistElement.SearchPresets("bundleVersionShort").editorLabel;

			//public static readonly GUIContent disableMinimumFramerate = InfoPlistElement.SearchPresets("disableMinimumFramerate").editorLabel;
			//public static readonly GUIContent appUsesNonExemptEncryption = InfoPlistElement.SearchPresets("appUsesNonExemptEncryption").editorLabel;
			//public static readonly GUIContent applicationRequiresIPhoneEnvironment = InfoPlistElement.SearchPresets("applicationRequiresIPhoneEnvironment").editorLabel;
		}
	}
}