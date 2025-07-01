using HansKindberg.Text.Formatting.Collections.Generic.Extensions;
using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Collections
{
	/// <inheritdoc cref="IYamlNode" />
	public abstract class YamlNode : YamlObject, IYamlNode
	{
		#region Properties

		IEnumerable<IYamlNode> IYamlNode.Children => this.Children;
		public virtual IList<IYamlNode> Children { get; } = [];

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

		protected internal virtual string DoubleQuoted(string text)
		{
			return $"\"{text}\"";
		}

		protected internal virtual string? GetAnchor()
		{
			return null;
		}

		protected internal virtual string? GetAnchorAlias()
		{
			return null;
		}

		protected internal virtual string? GetKey(Quotation? quotation)
		{
			return null;
		}

		protected internal virtual string? GetTag()
		{
			return null;
		}

		protected internal virtual string? GetText(Quotation? quotation, Scalar scalar)
		{
			if(scalar == null)
				throw new ArgumentNullException(nameof(scalar));

			var text = scalar.Value;

			if(quotation != null)
				return text;

			return text;

//			return quotation switch
//			{
//				null when !scalar.IsQuotedImplicit => text,
//#pragma warning disable IDE0072 // Add missing cases
//				null => scalar.Style switch
//				{
//					ScalarStyle.DoubleQuoted => await this.DoubleQuoted(text),
//					ScalarStyle.SingleQuoted => await this.SingleQuoted(text),
//					_ => text
//				},
//#pragma warning restore IDE0072 // Add missing cases
//				Quotation.Clear => text,
//				Quotation.Double => await this.DoubleQuoted(text),
//				Quotation.Single => await this.SingleQuoted(text),
//				_ => text
//			};
		}

		//protected internal override IList<string> GetTextParts(YamlFormatOptions options)
		//{
		//	if(options == null)
		//		throw new ArgumentNullException(nameof(options));

		//	var parts = new List<string>();

		//	var key = this.GetKey(options.KeyQuotation);
		//	if(!string.IsNullOrWhiteSpace(key))
		//		parts.Add($"{key}{options.KeySuffix}");

		//	var tag = this.GetTag();
		//	if(!string.IsNullOrWhiteSpace(tag))
		//		parts.Add($"{options.TagPrefix}{tag}");

		//	var anchor = this.GetAnchor();
		//	if(!string.IsNullOrWhiteSpace(anchor))
		//		parts.Add($"{options.AnchorPrefix}{anchor}");

		//	var anchorAlias = this.GetAnchorAlias();
		//	if(!string.IsNullOrWhiteSpace(anchorAlias))
		//		parts.Add($"{options.AnchorAliasPrefix}{anchorAlias}");

		//	var value = this.GetValue(options.ValueQuotation);
		//	if(!string.IsNullOrWhiteSpace(value))
		//		parts.Add(value!);

		//	var comment = this.GetComment();
		//	if(comment != null)
		//		parts.Add($"{options.CommentPrefix}{comment}");

		//	return parts;
		//}

		protected internal virtual string? GetValue(Quotation? quotation)
		{
			return null;
		}

		protected internal virtual string SingleQuoted(string text)
		{
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

		public virtual async Task Write(IList<string> lines, YamlFormatOptions options)
		{
			if(lines == null)
				throw new ArgumentNullException(nameof(lines));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			//var indentation = new string(options.Indentation.Character, options.Indentation.Size * this.Level);

			//foreach(var comment in this.LeadingComments)
			//{
			//	lines.Add($"{indentation}{options.CommentPrefix}{this.GetCommentValue(comment)}");
			//}

			//var text = this.ToString(options);

			//if(!string.IsNullOrEmpty(text))
			//	lines.Add(text);

			foreach(var child in this.Children)
			{
				await child.Write(lines, options);
			}

			//foreach(var comment in this.TrailingComments)
			//{
			//	lines.Add($"{indentation}{options.CommentPrefix}{this.GetCommentValue(comment)}");
			//}
		}

		#endregion
	}
}