namespace Skyline.Automation.SatPort.IAS.IasExtensions
{
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public static class IasExtensions
	{
		public static void AppendWidgets(this Section section, params Widget[] widgets)
		{
			var row = section.RowCount;
			for (int i = 0; i < widgets.Length; i++)
			{
				var widget = widgets[i];
				section.AddWidget(widget, row, i);
			}
		}
	}
}