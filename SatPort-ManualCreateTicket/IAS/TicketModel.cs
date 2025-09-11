namespace Skyline.Automation.SatPort.IAS
{
	using System.Collections.Generic;
	using System.Linq;
	using Skyline.Automation.SatPort.Context;
	using Skyline.Automation.SatPort.IAS.Interfaces;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM.Ticketing;
	using Skyline.DataMiner.SDM.Ticketing.Models;

	public class TicketModel : ITicketModel
	{
		public TicketModel(ScriptContext scriptContext, TicketingApiHelper helper)
		{
			ScriptContext = scriptContext;
			Helper = helper;

			Ticket = new Ticket();
			TicketTypes = GetTicketTypes();
		}

		public ScriptContext ScriptContext { get; private set; }

		public TicketingApiHelper Helper { get; private set; }

		public Ticket Ticket { get; private set; }

		public List<TicketType> TicketTypes { get; private set; }

		private List<TicketType> GetTicketTypes()
		{
			return Helper.TicketTypes.Read(new TRUEFilterElement<TicketType>()).ToList();
		}
	}
}