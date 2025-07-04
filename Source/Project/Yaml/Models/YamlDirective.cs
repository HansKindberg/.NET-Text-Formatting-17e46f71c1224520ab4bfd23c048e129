using YamlDotNet.Core;
using YamlDotNet.Core.Tokens;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	/// <inheritdoc cref="IYamlDirective" />
	public abstract class YamlDirective<TDirective>(TDirective directive) : YamlComponent, IYamlDirective where TDirective : Token
	{
		#region Properties

		public virtual TDirective Directive { get; } = directive ?? throw new ArgumentNullException(nameof(directive));
		public virtual Mark End => this.Directive.End;
		public virtual Mark Start => this.Directive.Start;

		#endregion
	}
}