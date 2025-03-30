using System.Collections;
using System.Xml;

namespace HansKindberg.Text.Formatting.Xml
{
	// TODO: Cleanup this class when we have a better solution.
	public class PermissiveXmlNamespaceManager(XmlNameTable xmlNameTable) : XmlNamespaceManager(xmlNameTable)
	{
		#region Methods

		public override IEnumerator GetEnumerator()
		{
			throw new NotImplementedException("GetEnumerator");
		}

		public override IDictionary<string, string>? GetNamespacesInScope(XmlNamespaceScope scope)
		{
			throw new NotImplementedException("GetNamespacesInScope");
		}

		public override bool HasNamespace(string prefix)
		{
			throw new NotImplementedException("HasNamespace");
		}

		public override string? LookupNamespace(string prefix)
		{
			var lookupNamespace = base.LookupNamespace(prefix);

			return lookupNamespace ?? string.Empty;
		}

		public override string? LookupPrefix(string uri)
		{
			throw new NotImplementedException("LookupPrefix");
		}

		#endregion
	}
}