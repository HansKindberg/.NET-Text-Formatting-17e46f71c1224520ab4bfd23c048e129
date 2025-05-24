using HansKindberg.Text.Formatting.Collections.Generic.Extensions;
using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	/// <inheritdoc />
	public abstract class YamlNode : IYamlNode
	{
		#region Properties

		IEnumerable<IYamlNode> IYamlNode.Children => this.Children;
		public virtual IList<IYamlNode> Children { get; } = [];
		public virtual Comment? Comment { get; set; }
		public virtual IList<Comment> LeadingComments { get; } = [];

		public virtual int Level
		{
			get
			{
				if(this.Parent?.Parent == null)
					return 0;

				return this.Parent.Level + 1;
			}
		}

		public virtual IYamlNode? Parent { get; set; }
		public virtual bool Sequence { get; set; }
		public virtual IList<Comment> TrailingComments { get; } = [];

		#endregion

		#region Methods

		public virtual async Task Add(IYamlNode child)
		{
			if(child == null)
				throw new ArgumentNullException(nameof(child));

			await Task.CompletedTask;

			child.Parent = this;
			this.Children.Add(child);
		}

		public virtual async Task ApplyNamingConvention(INamingConvention namingConvention)
		{
			foreach(var child in this.Children)
			{
				await child.ApplyNamingConvention(namingConvention);
			}
		}

		protected internal virtual async Task<string> DoubleQuoted(string text)
		{
			await Task.CompletedTask;

			return $"\"{text}\"";
		}

		protected internal virtual async Task<string?> GetAnchor()
		{
			await Task.CompletedTask;
			return null;
		}

		protected internal virtual async Task<string?> GetAnchorAlias()
		{
			await Task.CompletedTask;
			return null;
		}

		protected internal virtual async Task<string?> GetComment()
		{
			return await this.GetCommentValue(this.Comment);
		}

		protected internal virtual async Task<string?> GetCommentValue(Comment? comment)
		{
			await Task.CompletedTask;
			return comment?.Value.Trim();
		}

		protected internal virtual async Task<string?> GetKey(Quotation? quotation)
		{
			await Task.CompletedTask;
			return null;
		}

		protected internal virtual async Task<string?> GetTag()
		{
			await Task.CompletedTask;
			return null;
		}

		protected internal virtual async Task<string?> GetText(Quotation? quotation, Scalar scalar)
		{
			if(scalar == null)
				throw new ArgumentNullException(nameof(scalar));

			var text = scalar.Value;

			return quotation switch
			{
				null when !scalar.IsQuotedImplicit => text,
#pragma warning disable IDE0072 // Add missing cases
				null => scalar.Style switch
				{
					ScalarStyle.DoubleQuoted => await this.DoubleQuoted(text),
					ScalarStyle.SingleQuoted => await this.SingleQuoted(text),
					_ => text
				},
#pragma warning restore IDE0072 // Add missing cases
				Quotation.Clear => text,
				Quotation.Double => await this.DoubleQuoted(text),
				Quotation.Single => await this.SingleQuoted(text),
				_ => text
			};
		}

		protected internal virtual async Task<IList<string>> GetTextParts(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var parts = new List<string>();

			var key = await this.GetKey(options.KeyQuotation);
			if(!string.IsNullOrWhiteSpace(key))
				parts.Add($"{key}{options.KeySuffix}");

			var tag = await this.GetTag();
			if(!string.IsNullOrWhiteSpace(tag))
				parts.Add($"{options.TagPrefix}{tag}");

			var anchor = await this.GetAnchor();
			if(!string.IsNullOrWhiteSpace(anchor))
				parts.Add($"{options.AnchorPrefix}{anchor}");

			var anchorAlias = await this.GetAnchorAlias();
			if(!string.IsNullOrWhiteSpace(anchorAlias))
				parts.Add($"{options.AnchorAliasPrefix}{anchorAlias}");

			var value = await this.GetValue(options.ValueQuotation);
			if(!string.IsNullOrWhiteSpace(value))
				parts.Add(value!);

			var comment = await this.GetComment();
			if(comment != null)
				parts.Add($"{options.CommentPrefix}{comment}");

			return parts;
		}

		protected internal virtual async Task<string?> GetValue(Quotation? quotation)
		{
			await Task.CompletedTask;
			return null;
		}

		protected internal virtual async Task<string> SingleQuoted(string text)
		{
			await Task.CompletedTask;

			return $"'{text}'";
		}

		public virtual async Task Sort(IComparer<IYamlNode> comparer)
		{
			foreach(var child in this.Children)
			{
				await child.Sort(comparer);
			}

			this.Children.Sort(comparer);
		}

		public virtual string ToString(YamlFormatOptions options)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var parts = this.GetTextParts(options).Result;

			if(parts.Any())
				return string.Join(options.Space.ToString(), parts);

			return string.Empty;
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
				lines.Add($"{indentation}{options.CommentPrefix}{await this.GetCommentValue(comment)}");
			}

			var parts = await this.GetTextParts(options);

			if(parts.Any())
				lines.Add(string.Join(options.Space.ToString(), parts));

			foreach(var child in this.Children)
			{
				await child.Write(lines, options);
			}

			foreach(var comment in this.TrailingComments)
			{
				lines.Add($"{indentation}{options.CommentPrefix}{await this.GetCommentValue(comment)}");
			}
		}

		#endregion
	}
}