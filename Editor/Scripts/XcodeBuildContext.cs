//// Developed With Love by Ryan Boyer https://ryanjboyer.com <3
//
//#if UNITY_IOS
//using UnityEngine;
//using System.IO;
//
//namespace XcodeProjectSettings {
//	public sealed class XcodeBuildContext {
//		private readonly string pbxProjectPath;
//		private readonly string infoPlistPath;
//
//		public PBXProject pbxProject;
//		public PlistDocument infoPlist;
//
//		// MARK: - Lifecycle
//
//		internal XcodeBuildContext(in string path) {
//			pbxProjectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
//			pbxProject = new();
//			pbxProject.ReadFromFile(pbxProjectPath);
//
//			infoPlistPath = Path.Combine(path, "Info.plist");
//			infoPlist = new();
//			infoPlist.ReadFromFile(infoPlistPath);
//		}
//
//		internal void Finalize() {
//			// write `pbxProject`
//			pbxProject.WriteToFile(pbxProjectPath);
//
//			// write `infoPlist`
//			File.WriteAllText(infoPlistPath, infoPlist.WriteToString());
//		}
//	}
//}
//#endif