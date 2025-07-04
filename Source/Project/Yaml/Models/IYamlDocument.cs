using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public interface IYamlDocument : IFormatableYaml, IYamlCommentSurroundable
	{
		#region Properties

		IList<IYamlDirective> Directives { get; }
		IYamlDocumentNotation End { get; }
		bool IncludeEndOnWrite { get; set; }
		bool IncludeStartOnWrite { get; set; }

		/// <summary>
		/// To be able to use stable sorting.
		/// </summary>
		int Index { get; set; }

		IEnumerable<IYamlNode> Nodes { get; }
		IYamlDocumentNotation Start { get; }

		#endregion

		#region Methods

		Task Add(IYamlNode node);
		void SetExplicitEnd(Token end, Comment? comment = null);
		void SetExplicitStart(Token start, Comment? comment = null);
		void SetImplicitEnd(Token end);
		Task Sort(IComparer<IYamlNode> comparer);

		#endregion
	}
}