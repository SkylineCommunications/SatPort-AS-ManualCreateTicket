namespace Skyline.Automation.SatPort.IAS.Interfaces
{
	using System.Collections.Generic;
	using Skyline.Automation.SatPort.Context;
	using Skyline.DataMiner.SDM.Ticketing;
	using Skyline.DataMiner.SDM.Ticketing.Models;

	public interface ITicketModel
	{
		ScriptContext ScriptContext { get; }

		TicketingApiHelper Helper { get; }

		List<TicketType> TicketTypes { get; }

		Ticket Ticket { get; }
	}
}