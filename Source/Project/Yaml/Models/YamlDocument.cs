using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public class YamlDocument(IYamlDocumentNotation start, IYamlDocumentNotation end) : YamlCommentSurroundable, IYamlDocument
	{
		#region Properties

		public virtual IList<IYamlDirective> Directives { get; } = [];
		public virtual IYamlDocumentNotation End { get; private set; } = end ?? throw new ArgumentNullException(nameof(end));
		public virtual bool IncludeEndOnWrite { get; set; }
		public virtual bool IncludeStartOnWrite { get; set; }
		IEnumerable<IYamlNode> IYamlDocument.Nodes => this.Nodes;
		public virtual IList<IYamlNode> Nodes { get; } = [];
		public virtual IYamlDocumentNotation Start { get; private set; } = start ?? throw new ArgumentNullException(nameof(start));

		#endregion

		#region Methods

		public virtual async Task Add(IYamlNode node)
		{
			if(node == null)
				throw new ArgumentNullException(nameof(node));

			await Task.CompletedTask;

			node.Parent = null;
			this.Nodes.Add(node);
		}

		public virtual async Task ApplyNamingConvention(INamingConvention namingConvention)
		{
			await Task.CompletedTask;
			throw new NotImplementedException();
		}

		public virtual void SetExplicitEnd(Token end, Comment? comment = null)
		{
			if(end == null)
				throw new ArgumentNullException(nameof(end));

			this.End = new YamlDocumentEnd(end) { Comment = comment };
		}

		public virtual void SetExplicitStart(Token start, Comment? comment = null)
		{
			if(start == null)
				throw new ArgumentNullException(nameof(start));

			this.Start = new YamlDocumentStart(start) { Comment = comment };
		}

		public virtual void SetImplicitEnd(Token end)
		{
			if(end == null)
				throw new ArgumentNullException(nameof(end));

			this.End = new YamlDocumentEnd(end.Start, end.End);
		}

		public virtual async Task Sort(IComparer<IYamlNode> comparer)
		{
			await Task.CompletedTask;
			throw new NotImplementedException();
		}

		public virtual async Task Write(IList<string> lines, YamlFormatOptions options)
		{
			await Task.CompletedTask;
			throw new NotImplementedException();
		}

		#endregion
	}
}