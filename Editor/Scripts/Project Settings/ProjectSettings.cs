// Developed With Love by Ryan Boyer https://ryanjboyer.com <3

using Foundation;
using UnityEngine;

namespace XcodeProjectSettings {
	internal sealed class ProjectSettings : ScriptableObject {
		[SerializeField] internal bool enableBitcode = false;

		[SerializeField] internal OptionalReference<string> displayName = default;
		[SerializeField] internal OptionalReference<string> accessibilityBundleName = default;
		[SerializeField] internal Optional<bool> disableMinimumFramerate = default;
		[SerializeField] internal Optional<bool> appUsesNonExemptEncryption = default;
		[SerializeField] internal Optional<bool> applicationRequiresIPhoneEnvironment = default;
	}
}