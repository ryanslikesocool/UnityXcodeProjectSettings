// Developed With Love by Ryan Boyer https://ryanjboyer.com <3

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

		public static readonly InfoPlistElement[] BundleName = new InfoPlistElement[] {
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
		};

		public static readonly InfoPlistElement[] Versioning = new InfoPlistElement[] {
			new InfoPlistElement(
				propertyName: "bundleVersion",
				dictionaryKey: "CFBundleVersion",
				title: "Bundle Version",
				tooltip: "",
				helpURL: "https://developer.apple.com/documentation/bundleresources/information_property_list/cfbundleversion"
			),
			new InfoPlistElement(
				propertyName: "bundleVersionShort",
				dictionaryKey: "CFBundleShortVersionString",
				title: "Bundle Version (Short)",
				tooltip: "",
				helpURL: "https://developer.apple.com/documentation/bundleresources/information_property_list/cfbundleshortversionstring"
			)
		};

		public static readonly InfoPlistElement[] Common = new InfoPlistElement[] {
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
			),
		};

		//private static readonly InfoPlistElement[][] PresetCategories = new InfoPlistElement[][] {
		//	BundleName,
		//	Versioning,
		//	Common,
		//};

		public enum SearchMode {
			PropertyName,
			DictionaryKey,
		}

		public static InfoPlistElement SearchCategory(IEnumerable<InfoPlistElement> collection, string query, SearchMode searchMode = SearchMode.PropertyName) => searchMode switch {
			SearchMode.PropertyName => collection.First(e => e.propertyName == query),
			SearchMode.DictionaryKey => collection.First(e => e.dictionaryKey == query),
			_ => throw new ArgumentException()
		};

		public static InfoPlistElement SearchPresets(string query, SearchMode searchMode = SearchMode.PropertyName) {
			// this doesn't like being in the global scope for whatever reason...
			InfoPlistElement[][] presetCategories = new InfoPlistElement[][] {
				BundleName,
				Versioning,
				Common,
			};

			foreach (InfoPlistElement[] category in presetCategories) {
				InfoPlistElement categorySearchResult = SearchCategory(category, query, searchMode);
				if (!categorySearchResult.Equals(default(InfoPlistElement))) {
					return categorySearchResult;
				}
			}
			return default;
		}
	}
}