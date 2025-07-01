//using YamlDotNet.Core.Tokens;

//namespace HansKindberg.Text.Formatting.Yaml.Tokens
//{
//	public class YamlDocument
//	{
//		#region Constructors

//		public YamlDocument(YamlStream stream)
//		{
//			if(stream == null)
//				throw new ArgumentNullException(nameof(stream));

//			this.End = new YamlDocumentEnd(stream.End.Start, stream.End.End);
//			this.Start = new YamlDocumentStart(stream.Start.Start, stream.Start.End);
//		}

//		#endregion

//		#region Properties

//		public virtual YamlDocumentEnd End { get; private set; }
//		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////public virtual IList<Token> LeadingTokens { get; } = [];
//		public virtual YamlDocumentStart Start { get; private set; }
//		public virtual IList<Token> Tokens { get; } = [];
//		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////public virtual IList<Token> TrailingTokens { get; } = [];

//		#endregion

//		#region Methods

//		public virtual void SetEnd(DocumentEnd end)
//		{
//			this.End = new YamlDocumentEnd(end);
//		}

//		public virtual void SetEnd(DocumentStart start)
//		{
//			this.End = new YamlDocumentEnd(start.Start, start.End);
//		}

//		public virtual void SetStart(DocumentEnd end)
//		{
//			this.Start = new YamlDocumentStart(end.Start, end.End);
//		}

//		public virtual void SetStart(DocumentStart start)
//		{
//			this.Start = new YamlDocumentStart(start);
//		}

//		#endregion
//	}
//}