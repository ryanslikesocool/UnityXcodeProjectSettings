using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace XcodeProjectSettings {
	internal sealed class XcodeProjectSettingsProvider : SettingsProvider {
		private SerializedObject xcodeSettings;

		private sealed class Styles {
			public static readonly GUIContent displayName = new GUIContent("Display Name", "CFBundleDisplayName");
			public static readonly GUIContent enableBitcode = new GUIContent("Enable Bitcode");
			public static readonly GUIContent disableMinimumFramerate = new GUIContent("Disable Minimum Framerate", "CADisableMinimumFrameDurationOnPhone");
			public static readonly GUIContent appUsesNonExemptEncryption = new GUIContent("App Uses Non-Exempt Encryption", "ITSAppUsesNonExemptEncryption");
		}

		public XcodeProjectSettingsProvider(string path, SettingsScope scope = SettingsScope.User) : base(path, scope) { }

		public static bool IsSettingsAvailable()
			=> XcodeProjectSettings.FindSettings() != null;

		public override void OnActivate(string searchContext, VisualElement rootElement) {
			xcodeSettings = XcodeProjectSettings.GetSerializedSettings();
		}

		public override void OnGUI(string searchContext) {
			EditorGUILayout.PropertyField(xcodeSettings.FindProperty("displayName"), Styles.displayName);
			EditorGUILayout.PropertyField(xcodeSettings.FindProperty("enableBitcode"), Styles.enableBitcode);
			EditorGUILayout.PropertyField(xcodeSettings.FindProperty("disableMinimumFramerate"), Styles.disableMinimumFramerate);
			EditorGUILayout.PropertyField(xcodeSettings.FindProperty("appUsesNonExemptEncryption"), Styles.appUsesNonExemptEncryption);

			xcodeSettings.ApplyModifiedPropertiesWithoutUndo();
		}

		[SettingsProvider]
		public static SettingsProvider CreateXcodeProjectSettingsProvider() {
			if (IsSettingsAvailable()) {
				XcodeProjectSettingsProvider provider = new XcodeProjectSettingsProvider("Project/Xcode Project Settings", SettingsScope.Project);

				provider.keywords = GetSearchKeywordsFromGUIContentProperties<Styles>();
				return provider;
			}

			return null;
		}
	}
}