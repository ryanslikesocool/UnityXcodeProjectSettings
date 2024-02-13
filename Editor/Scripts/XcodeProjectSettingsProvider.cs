using UnityEditor;
using UnityEngine.UIElements;

namespace XcodeProjectSettings {
	internal sealed class XcodeProjectSettingsProvider : SettingsProvider {
		private SerializedObject projectSettings;
		private Editor projectSettingsEditor;

		public XcodeProjectSettingsProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope) { }

		public static bool IsSettingsAvailable()
			=> XcodeProjectSettings.DoesSettingsExists();

		public override void OnActivate(string searchContext, VisualElement rootElement) {
			projectSettings = XcodeProjectSettings.GetSerializedSettings();
			projectSettingsEditor = Editor.CreateEditor(projectSettings.targetObject);
		}

		// MARK: - Draw

		public override void OnGUI(string searchContext) {
			projectSettingsEditor.OnInspectorGUI();
			projectSettings.ApplyModifiedPropertiesWithoutUndo();
		}

		// MARK: -

		[SettingsProvider]
		public static SettingsProvider CreateXcodeProjectSettingsProvider() {
			XcodeProjectSettingsProvider provider = new XcodeProjectSettingsProvider("Project/Xcode Project Settings", SettingsScope.Project);
			provider.keywords = GetSearchKeywordsFromGUIContentProperties<XcodeProjectSettingsEditor.Styles>();
			return provider;
		}
	}
}