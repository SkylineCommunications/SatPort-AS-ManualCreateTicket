namespace Skyline.Automation.SatPort.IAS.Interfaces
{
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public interface ITicketView
	{
		TextBox Name { get; }

		TextBox Description { get; }

		DropDown TicketType { get; }

		DropDown Impact { get; }

		TextBox WorkNotes { get; }

		Button CreateButton { get; }

		Button CancelButton { get; }
	}
}