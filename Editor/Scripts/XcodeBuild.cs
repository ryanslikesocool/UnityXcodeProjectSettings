#if UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace XcodeProjectSettings {
	internal static class XcodeBuild {
		[PostProcessBuild]
		private static void OnPostprocessBuild(BuildTarget buildTarget, string path) {
			XcodeProjectSettings projectSettings = XcodeProjectSettings.LoadExistingSettings();

			if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS && buildTarget != BuildTarget.VisionOS) {
				return;
			}

			WriteInfoPlist(projectSettings, path);
			DisableBitcode(projectSettings, path);
		}

		private static void WriteInfoPlist(XcodeProjectSettings projectSettings, in string path) {
			// Read plist
			string plistPath = Path.Combine(path, "Info.plist");
			PlistDocument plist = new PlistDocument();
			plist.ReadFromFile(plistPath);
			PlistElementDict rootDict = plist.root;

			{ // Apply properties
				if (projectSettings.displayName.hasValue) {
					rootDict.SetString("CFBundleDisplayName", projectSettings.displayName.wrappedValue);
				}
				if (projectSettings.accessibilityBundleName.hasValue) {
					rootDict.SetString("CFBundleSpokenName", projectSettings.accessibilityBundleName.wrappedValue);
				}
				if (projectSettings.disableMinimumFramerate.hasValue) {
					rootDict.SetBoolean("CADisableMinimumFrameDurationOnPhone", projectSettings.disableMinimumFramerate.wrappedValue);
				}
				if (projectSettings.appUsesNonExemptEncryption.hasValue) {
					rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", projectSettings.appUsesNonExemptEncryption.wrappedValue);
				}
				if (projectSettings.applicationRequiresIPhoneEnvironment.hasValue) {
					rootDict.SetBoolean("LSRequiresIPhoneOS", projectSettings.applicationRequiresIPhoneEnvironment.wrappedValue);
				}
			}

			// Write plist
			File.WriteAllText(plistPath, plist.WriteToString());
		}

		private static void DisableBitcode(XcodeProjectSettings projectSettings, in string path) {
			if (projectSettings.enableBitcode) {
				return;
			}

			string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

			PBXProject pbxProject = new PBXProject();
			pbxProject.ReadFromFile(projectPath);

			//Disabling Bitcode on all targets

			//Main
			string target = pbxProject.GetUnityMainTargetGuid();
			pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

			//Unity Tests
			target = pbxProject.TargetGuidByName(PBXProject.GetUnityTestTargetName());
			pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

			//Unity Framework
			target = pbxProject.GetUnityFrameworkTargetGuid();
			pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");

			pbxProject.WriteToFile(projectPath);
		}
	}
}
#endif
