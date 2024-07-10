// Developed With Love by Ryan Boyer https://ryanjboyer.com <3

using UnityEngine;
using System.Linq;
using System;

namespace XcodeProjectSettings {
	internal readonly struct InfoPlistElement {
		public readonly string propertyName;
		public readonly string dictionaryKey;
		public readonly GUIContent editorLabel;
		public readonly string helpURL;

		public InfoPlistElement(
			string propertyName,
			string dictionaryKey,
			GUIContent editorLabel,
			string helpURL
		) {
			this.propertyName = propertyName;
			this.dictionaryKey = dictionaryKey;
			this.editorLabel = editorLabel;
			this.helpURL = helpURL;
		}

		public InfoPlistElement(
			string propertyName,
			string dictionaryKey,
			string title,
			string tooltip,
			string helpURL
		) : this(
			propertyName: propertyName,
			dictionaryKey: dictionaryKey,
			editorLabel: new GUIContent(title, tooltip),
			helpURL: helpURL
		) { }

		public static readonly InfoPlistElement[] Common = new InfoPlistElement[] {
			new InfoPlistElement(
				propertyName: "displayName",
				dictionaryKey: "CFBundleDisplayName",
				title: "Bundle Display Name",
				tooltip: "The user-visible name for the bundle, used by Siri and visible on the iOS Home screen.",
				helpURL: "https://developer.apple.com/documentation/bundleresources/information_property_list/cfbundledisplayname"
			),
			new InfoPlistElement(
				propertyName: "accessibilityBundleName",
				dictionaryKey: "CFBundleSpokenName",
				title: "Accessibility Bundle Name",
				tooltip: "A replacement for the app name in text-to-speech operations.",
				helpURL: "https://developer.apple.com/documentation/bundleresources/information_property_list/cfbundlespokenname"
			),
			new InfoPlistElement(
				propertyName: "disableMinimumFramerate",
				dictionaryKey: "CADisableMinimumFrameDurationOnPhone",
				title: "Disable Minimum Framerate",
				tooltip: "Enable the full range of refresh rates on ProMotion displays.",
				helpURL: "https://developer.apple.com/documentation/quartzcore/optimizing_promotion_refresh_rates_for_iphone_13_pro_and_ipad_pro"
			),
			new InfoPlistElement(
				propertyName: "appUsesNonExemptEncryption",
				dictionaryKey: "ITSAppUsesNonExemptEncryption",
				title: "App Uses Non-Exempt Encryption",
				tooltip: "A Boolean value indicating whether the app uses encryption.",
				helpURL: "https://developer.apple.com/documentation/bundleresources/information_property_list/itsappusesnonexemptencryption"
			),
			new InfoPlistElement(
				propertyName: "applicationRequiresIPhoneEnvironment",
				dictionaryKey: "LSRequiresIPhoneOS",
				title: "Application requires iPhone environment",
				tooltip: "A Boolean value indicating whether the app must run in iOS.",
				helpURL: "https://developer.apple.com/documentation/bundleresources/information_property_list/lsrequiresiphoneos"
			)
		};

		public enum SearchMode {
			PropertyName,
			DictionaryKey
		}

		public static InfoPlistElement SearchCommon(string query, SearchMode searchMode = SearchMode.PropertyName) => searchMode switch {
			SearchMode.PropertyName => Common.First(e => e.propertyName == query),
			SearchMode.DictionaryKey => Common.First(e => e.dictionaryKey == query),
			_ => throw new ArgumentException()
		};
	}
}