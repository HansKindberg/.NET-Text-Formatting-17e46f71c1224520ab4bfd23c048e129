using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	/// <inheritdoc />
	public interface IYamlDocumentNode : IYamlContentNode
	{
		#region Properties

		IList<IYamlDirective> Directives { get; }
		IYamlDocumentNotation End { get; }
		bool IncludeEndOnWrite { get; set; }
		bool IncludeStartOnWrite { get; set; }
		IYamlDocumentNotation Start { get; }

		#endregion

		#region Methods

		void SetEnd(Token end, Comment? comment = null);
		void SetStart(Token start, Comment? comment = null);

		#endregion
	}
}