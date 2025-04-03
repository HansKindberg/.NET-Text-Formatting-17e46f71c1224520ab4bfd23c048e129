using System.Globalization;
using HansKindberg.Text.Formatting.Xml;
using Microsoft.Extensions.Logging.Abstractions;

namespace IntegrationTests.Xml
{
	public class XmlFormatterTest
	{
		#region Methods

		[Fact]
		public async Task Format_Test()
		{
			CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en");

			using(var loggerFactory = new NullLoggerFactory())
			{
				var xmlFormatter = new XmlFormatter(loggerFactory, new XmlParser());

				var xml = await xmlFormatter.Format(new XmlFormat(), await this.GetFileContent("EPiServer.Web.config"));

				Assert.NotNull(xml);
			}
		}

		protected internal virtual async Task<string> GetFileContent(string fileName)
		{
			await Task.CompletedTask;

			return File.ReadAllText(Path.Combine(Global.ProjectDirectory.FullName, "Xml", "Resources", fileName));
		}

		#endregion
	}
}