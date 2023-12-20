using UnityEditor;
using UnityEngine;

namespace XcodeProjectSettings {
	internal sealed class XcodeProjectSettings : ScriptableObject {
		// MARK: - Fields

		[SerializeField] internal string displayName = string.Empty;
		[SerializeField] internal bool enableBitcode = false;
		[SerializeField] internal bool disableMinimumFramerate = false;
		[SerializeField] internal bool appUsesNonExemptEncryption = true;

		// MARK: - Constants

		private const string DEFAULT_SETTINGS_PATH = "Assets/Editor/Xcode Project Settings.asset";
		public const string SEARCH_STRING = "t:XcodeProjectSettings";

		// MARK: - Serialization

		internal static XcodeProjectSettings GetOrCreateSettings() {
			XcodeProjectSettings settings = FindSettings();
			if (settings == null) {
				settings = CreateInstance<XcodeProjectSettings>();

				AssetDatabase.CreateAsset(settings, DEFAULT_SETTINGS_PATH);
				AssetDatabase.SaveAssets();
			}
			return settings;
		}

		internal static SerializedObject GetSerializedSettings() {
			return new SerializedObject(GetOrCreateSettings());
		}

		internal static XcodeProjectSettings FindSettings() {
			string[] guids = AssetDatabase.FindAssets(SEARCH_STRING);
			if (guids.Length == 0) {
				return null;
			}
			string path = AssetDatabase.GUIDToAssetPath(guids[0]);
			return AssetDatabase.LoadAssetAtPath<XcodeProjectSettings>(path);
		}
	}
}