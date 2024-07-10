// Developed With Love by Ryan Boyer https://ryanjboyer.com <3

using Foundation.Editors;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XcodeProjectSettings {
	internal sealed class ProjectSettingsProvider : SettingsProvider {
		private SerializedObject projectSettings;
		private Editor projectSettingsEditor;

		// MARK: - Lifecycle

		public ProjectSettingsProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope) { }

		public override void OnActivate(string searchContext, VisualElement rootElement) {
			projectSettings = GetSerializedSettings();
			projectSettingsEditor = Editor.CreateEditor(projectSettings.targetObject);
		}

		// MARK: - GUI

		public override void OnGUI(string searchContext) {
			projectSettingsEditor.OnInspectorGUI();
			projectSettings.ApplyModifiedPropertiesWithoutUndo();
		}

		// MARK: - Serialization

		private static ProjectSettings GetOrCreateSettings() {
			const string DEFAULT_SETTINGS_PATH = "Assets/Plugins/Developed With Love/Editor/Xcode Project Settings.asset";

			ProjectSettings settings = LoadExistingSettings();
			if (settings == null) {
				settings = ScriptableObject.CreateInstance<ProjectSettings>();

				FoundationEditorUtility.CreateDirectory(DEFAULT_SETTINGS_PATH);

				AssetDatabase.CreateAsset(settings, DEFAULT_SETTINGS_PATH);
				AssetDatabase.SaveAssets();
			}
			return settings;
		}

		private static SerializedObject GetSerializedSettings() {
			return new SerializedObject(GetOrCreateSettings());
		}

		private static ProjectSettings LoadExistingSettings() {
			string[] guids = AssetDatabase.FindAssets($"t:{typeof(ProjectSettings)}");
			if (guids.Length == 0) {
				return null;
			}
			string path = AssetDatabase.GUIDToAssetPath(guids[0]);
			return AssetDatabase.LoadAssetAtPath<ProjectSettings>(path);
		}

		// MARK: - Settings Provider

		[SettingsProvider]
		public static SettingsProvider CreateXcodeProjectSettingsProvider() {
			const string MENU_PATH = "Project/Developed With Love/Xcode";

			ProjectSettingsProvider provider = new ProjectSettingsProvider(MENU_PATH, SettingsScope.Project) {
				keywords = GetSearchKeywordsFromGUIContentProperties<ProjectSettingsEditor.Styles>()
			};
			return provider;
		}
	}
}