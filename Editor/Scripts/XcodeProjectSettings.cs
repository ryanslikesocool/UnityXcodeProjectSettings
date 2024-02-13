using Foundation;
using UnityEditor;
using UnityEngine;

namespace XcodeProjectSettings {
	internal sealed class XcodeProjectSettings : ScriptableObject {
		// MARK: - Fields

#pragma warning disable 0414
		[SerializeField] internal bool enableBitcode;

		[SerializeField] internal OptionalReference<string> displayName;
		[SerializeField] internal OptionalReference<string> accessibilityBundleName;
		[SerializeField] internal Optional<bool> disableMinimumFramerate;
		[SerializeField] internal Optional<bool> appUsesNonExemptEncryption;
		[SerializeField] internal Optional<bool> applicationRequiresIPhoneEnvironment;
#pragma warning restore 0414

		// MARK: - Constants

		private const string DEFAULT_SETTINGS_PATH = "Assets/Editor/Xcode Project Settings.asset";

		// MARK: - Serialization

		internal static XcodeProjectSettings GetOrCreateSettings() {
			XcodeProjectSettings settings = LoadExistingSettings();
			if (settings == null) {
				settings = CreateInstance<XcodeProjectSettings>();

				settings.enableBitcode = false;
				settings.displayName = default;
				settings.disableMinimumFramerate = default;
				settings.appUsesNonExemptEncryption = default;
				settings.applicationRequiresIPhoneEnvironment = default;
				settings.accessibilityBundleName = default;

				AssetDatabase.CreateAsset(settings, DEFAULT_SETTINGS_PATH);
				AssetDatabase.SaveAssets();
			}
			return settings;
		}

		internal static SerializedObject GetSerializedSettings() {
			return new SerializedObject(GetOrCreateSettings());
		}

		internal static XcodeProjectSettings LoadExistingSettings() {
			string[] guids = AssetDatabase.FindAssets($"t:{typeof(XcodeProjectSettings)}");
			if (guids.Length == 0) {
				return null;
			}
			string path = AssetDatabase.GUIDToAssetPath(guids[0]);
			return AssetDatabase.LoadAssetAtPath<XcodeProjectSettings>(path);
		}

		internal static bool DoesSettingsExists() {
			string[] guids = AssetDatabase.FindAssets($"t:{typeof(XcodeProjectSettings)}");
			return guids.Length > 0;
		}
	}
}