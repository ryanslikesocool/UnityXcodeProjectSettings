#if UNITY_EDITOR && UNITY_IOS
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace XcodeProjectSettings {
    internal static class XcodeBuild {
        [PostProcessBuild]
        private static void OnPostprocessBuild(BuildTarget buildTarget, string path) {
			XcodeProjectSettings projectSettings = XcodeProjectSettings.FindSettings();

            WriteInfoPlist(projectSettings, path);
            DisableBitcode(projectSettings, buildTarget, path);
        }

        private static void WriteInfoPlist(XcodeProjectSettings projectSettings, in string path) {
            if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) {
                return;
            }

            // Read plist
            string plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            PlistElementDict rootDict = plist.root;

			{ // Apply properties
				if (projectSettings.displayName.Length > 0) {
					rootDict.SetString("CFBundleDisplayName", projectSettings.displayName);
				}
				rootDict.SetBoolean("CADisableMinimumFrameDurationOnPhone", projectSettings.disableMinimumFramerate);
				rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", projectSettings.appUsesNonExemptEncryption);
			}

            // Write plist
            File.WriteAllText(plistPath, plist.WriteToString());
        }

        private static void DisableBitcode(XcodeProjectSettings projectSettings, BuildTarget buildTarget, in string path) {
            if (buildTarget != BuildTarget.iOS && buildTarget != BuildTarget.tvOS) {
                return;
            }
			if(projectSettings.enableBitcode) {
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
