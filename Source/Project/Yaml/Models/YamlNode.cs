using HansKindberg.Text.Formatting.Collections.Generic.Extensions;
using HansKindberg.Text.Formatting.Diagnostics;
using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	/// <inheritdoc />
	public abstract class YamlNode : IYamlNode
	{
		#region Properties

		IEnumerable<IYamlNode> IYamlNode.Children => this.Children;
		public virtual IList<IYamlNode> Children { get; } = [];
		public virtual string? Comment { get; set; }
		public virtual IList<string> LeadingComments { get; } = [];

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
		public virtual bool Sequence { get; set; }
		public abstract TextInformation TextInformation { get; }
		public virtual IList<string> TrailingComments { get; } = [];

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

		protected internal virtual LinePositionSpan CreateLinePositionSpan(Mark start, Mark end)
		{
			var startPosition = new LinePosition(start.Column - 1, start.Line - 1);
			var endPosition = new LinePosition(end.Column - 1, end.Line - 1);

			return new LinePositionSpan(startPosition, endPosition);
		}

		protected internal virtual TextInformation CreateTextInformation(ParsingEvent parsingEvent)
		{
			return this.CreateTextInformation(parsingEvent, parsingEvent);
		}

		protected internal virtual TextInformation CreateTextInformation(ParsingEvent startParsingEvent, ParsingEvent endParsingEvent)
		{
			if(startParsingEvent == null)
				throw new ArgumentNullException(nameof(startParsingEvent));

			if(endParsingEvent == null)
				throw new ArgumentNullException(nameof(endParsingEvent));

			var startLine = this.CreateLinePositionSpan(startParsingEvent.Start, startParsingEvent.End);
			var endLine = this.CreateLinePositionSpan(endParsingEvent.Start, endParsingEvent.End);

			return new TextInformation(startLine, endLine);
		}

		protected internal virtual async Task<IList<string>> GetParts(YamlFormatOptions options, IParsingEventStringifier parsingEventStringifier)
		{
			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(parsingEventStringifier == null)
				throw new ArgumentNullException(nameof(parsingEventStringifier));

			var parts = new List<string>();

			await Task.CompletedTask;
			//var key = await parsingEventStringifier.Stringify(options, this.Key);
			//if(!string.IsNullOrEmpty(key))
			//	parts.Add(key!);

			//var value = await parsingEventStringifier.Stringify(options, this.Value);
			//if(!string.IsNullOrEmpty(value))
			//	parts.Add(value!);

			//if(this.Comment != null)
			//	parts.Add($"{options.CommentPrefix}{this.Comment}");

			return parts;
		}

		public virtual async Task Sort(IComparer<IYamlNode> comparer)
		{
			foreach(var child in this.Children)
			{
				await child.Sort(comparer);
			}

			this.Children.Sort(comparer);
		}

		public virtual async Task Write(IList<string> lines, YamlFormatOptions options, IParsingEventStringifier parsingEventStringifier)
		{
			if(lines == null)
				throw new ArgumentNullException(nameof(lines));

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			if(parsingEventStringifier == null)
				throw new ArgumentNullException(nameof(parsingEventStringifier));

			var indentation = new string(options.Indentation.Character, options.Indentation.Size * this.Level);

			foreach(var comment in this.LeadingComments)
			{
				lines.Add($"{indentation}{options.CommentPrefix}{comment}");
			}

			var parts = await this.GetParts(options, parsingEventStringifier);

			if(parts.Any())
				lines.Add(string.Join(options.Space.ToString(), parts));

			foreach(var child in this.Children)
			{
				await child.Write(lines, options, parsingEventStringifier);
			}

			foreach(var comment in this.TrailingComments)
			{
				lines.Add($"{indentation}{options.CommentPrefix}{comment}");
			}
		}

		#endregion
	}
}