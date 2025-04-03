# .NET-Text-Formatting

Library for text-formatting. Eg. json-formatting, xml-formatting, yaml-formatting etc.

[![NuGet](https://img.shields.io/nuget/v/HansKindberg.Text.Formatting.svg?label=NuGet)](https://www.nuget.org/packages/HansKindberg.Text.Formatting)

## Temporary repository

**This, ".NET-Text-Formatting-17e46f71c1224520ab4bfd23c048e129", is a temporary repository. When ready it will be ".NET-Text-Formatting".**

## 1 Features

### 1.1 JSON

- JSONPath: search for it, https://www.newtonsoft.com/json/help/html/QueryJsonSelectTokenJsonPath.htm
- Sorting: https://stackoverflow.com/questions/14417235/c-sharp-sort-json-string-keys?lq=1

### 1.2 XML

- XPathNavigator.Matches: https://docs.microsoft.com/en-us/dotnet/api/system.xml.xpath.xpathnavigator.matches?view=netframework-4.7.2#System_Xml_XPath_XPathNavigator_Matches_System_String_

### 1.3 YAML

#### 1.3.1 Comments

At the moment deserializing comments is not supported. The comments are removed when deserializing.

- [Serialize comments #152](https://github.com/aaubry/YamlDotNet/issues/152)
	- Example: https://dotnetfiddle.net/8M6iIE
- [Process inline comments with DictionaryDeserializer-2 #868](https://github.com/aaubry/YamlDotNet/pull/868)

## 2 Development

### 2.1 Signing

Drop the "StrongName.snk" file in the repository-root. The file should not be included in source control.

## 3 Notes

GUI for it:
WPF
http://blog.danskingdom.com/adding-a-wpf-settings-page-to-the-tools-options-dialog-window-for-your-visual-studio-extension/

https://github.com/Microsoft/VSSDK-Extensibility-Samples/tree/master/Options

