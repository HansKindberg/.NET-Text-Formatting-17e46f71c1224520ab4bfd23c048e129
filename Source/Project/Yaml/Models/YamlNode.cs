using HansKindberg.Text.Formatting.Collections.Generic.Extensions;
using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public abstract class YamlNode : YamlComponent, IYamlNode
	{
		#region Properties

		IEnumerable<IYamlNode> IYamlNode.Children => this.Children;
		public virtual IList<IYamlNode> Children { get; } = [];
		public virtual bool Flow { get; set; }
		public virtual int Index { get; set; }
		public virtual IList<Comment> LeadingComments { get; } = [];

		public virtual int Level
		{
			get
			{
				if(this.Parent == null)
					return 0;

				return this.Parent.Level + 1;
			}
		}

		public virtual IYamlNode? Parent { get; set; }
		public virtual IList<Comment> TrailingComments { get; } = [];

		#endregion

		#region Methods

		public virtual async Task Add(IYamlNode child)
		{
			if(child == null)
				throw new ArgumentNullException(nameof(child));

			await Task.CompletedTask;

			child.Index = this.Children.Count;
			child.Parent = this;
			this.Children.Add(child);
		}

		public virtual async Task ApplyNamingConvention(INamingConvention namingConvention)
		{
			if(namingConvention == null)
				throw new ArgumentNullException(nameof(namingConvention));

			foreach(var child in this.Children)
			{
				await child.ApplyNamingConvention(namingConvention);
			}
		}

		public virtual async Task Sort(IComparer<IYamlNode> comparer)
		{
			if(comparer == null)
				throw new ArgumentNullException(nameof(comparer));

			foreach(var child in this.Children)
			{
				await child.Sort(comparer);
			}

			this.Children.Sort(comparer);
		}

		public virtual async Task Write(IList<string> lines, YamlFormatOptions options)
		{
			if(lines == null)
				throw new ArgumentNullException(nameof(lines));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var indentation = new string(options.Indentation.Character, options.Indentation.Size * this.Level);

			foreach(var comment in this.LeadingComments)
			{
				lines.Add($"{indentation}{options.CommentPrefix}{this.GetCommentValue(comment, options)}");
			}

			var text = this.ToString(options);

			if(!string.IsNullOrEmpty(text))
				lines.Add($"{indentation}{text}");

			foreach(var child in this.Children)
			{
				await child.Write(lines, options);
			}

			foreach(var comment in this.TrailingComments)
			{
				lines.Add($"{indentation}{options.CommentPrefix}{this.GetCommentValue(comment, options)}");
			}
		}

		#endregion
	}
}