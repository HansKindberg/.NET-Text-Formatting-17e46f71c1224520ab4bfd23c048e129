namespace HansKindberg.Text.Formatting.Configuration
{
	public class IndentationOptions
	{
		#region Fields

		private int _size = 1;

		#endregion

		#region Properties

		public char Character { get; set; } = '\t';
		public bool Enabled { get; set; } = true;

		public int Size
		{
			get => this._size;
			set
			{
				if(value < 0)
					throw new ArgumentOutOfRangeException(nameof(value), "Size can not be less than zero.");

				this._size = value;
			}
		}

		#endregion
	}
}