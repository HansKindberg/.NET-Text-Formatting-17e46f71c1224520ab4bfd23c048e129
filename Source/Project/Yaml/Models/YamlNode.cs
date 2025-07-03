using HansKindberg.Text.Formatting.Yaml.Configuration;
using YamlDotNet.Serialization;

namespace HansKindberg.Text.Formatting.Yaml.Models
{
	public abstract class YamlNode : YamlCommentSurroundable, IYamlNode
	{
		#region Properties

		IEnumerable<IYamlNode> IYamlNode.Children => this.Children;
		public virtual IList<IYamlNode> Children { get; } = [];
		public virtual bool Flow { get; set; }

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
			await Task.CompletedTask;
			throw new NotImplementedException();
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