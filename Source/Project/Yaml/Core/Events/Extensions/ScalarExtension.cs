using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace HansKindberg.Text.Formatting.Yaml.Core.Events.Extensions
{
	public static class ScalarExtension
	{
		#region Methods

		/// <summary>
		/// If the text we want to parse contains only comments or only empty lines we may get an "empty" scalar parsing-event that we need to remove.
		/// We find this scalar parsing-event with the following criteria.
		/// </summary>
		public static bool IsEmpty(this Scalar scalar)
		{
			if(scalar == null)
				throw new ArgumentNullException(nameof(scalar));

			return scalar is { IsPlainImplicit: true, Style: ScalarStyle.Plain, Value: "" } && scalar.End.Column == scalar.Start.Column && scalar.End.Index == scalar.Start.Index && scalar.End.Line == scalar.Start.Line;
		}

		#endregion
	}
}