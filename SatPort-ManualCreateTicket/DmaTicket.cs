namespace SatPort_ManualCreateTicket
{
	using System;
	using Skyline.DataMiner.Analytics.GenericInterface.JoinFilter;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM.Ticketing;
	using Skyline.DataMiner.SDM.Ticketing.Models;
	using TicketType = Skyline.DataMiner.SDM.Ticketing.Models.TicketType;

	public class DmaTicket
	{
		public DmaTicket(AlarmEventMessage alarmMessage)
		{
			if (alarmMessage == null)
			{
				throw new ArgumentNullException(nameof(alarmMessage), "Alarm message cannot be null.");
			}

			Name = $"{alarmMessage.ParameterName}@{alarmMessage.Value}";
			Key = $"{alarmMessage.DataMinerID}/{alarmMessage.ElementID}/{alarmMessage.AlarmID}";
			Description = $"{alarmMessage.ElementName} - {Name}";
			Priority = MapPriority(alarmMessage.Type, alarmMessage.Severity);
			Severity = MapSeverity(alarmMessage.Severity);
			AlarmId = alarmMessage.AlarmID;
			ElementId = alarmMessage.ElementID;
			DataMinerId = alarmMessage.DataMinerID;
		}

		public string Name { get; }

		public string Key { get; }

		public string Description { get; }

		public TicketPriority Priority { get; }

		public TicketSeverity Severity { get; }

		public int AlarmId { get; }

		public int ElementId { get; }

		public int DataMinerId { get; }

		public TicketType Type { get; set; }

		public static implicit operator Ticket(DmaTicket ticket)
		{
			return new Ticket
			{
				Description = ticket.Description,
				Name = ticket.Name,
				Type = ticket.Type,
				Priority = ticket.Priority,
				Severity = ticket.Severity,
				RequestedResolutionDate = DateTime.Now.AddDays(7),
				ExpectedResolutionDate = DateTime.Now.AddDays(3),
				Status = TicketStatus.Pending,
			};
		}

		public void CreateIfNotExists(TicketingApiHelper helper)
		{
			var refTicket = helper.Tickets.Read(TicketExposers.ID.Equal(Key));

			if (refTicket is null)
			{
				helper.Tickets.Create(this);
			}
		}

		private TicketPriority MapPriority(string type, string severity)
		{
			// TODO: define priority with a set of parameters
			return TicketPriority.High;
		}

		private TicketSeverity MapSeverity(string severity)
		{
			switch (severity)
			{
				case "Critical":
					return TicketSeverity.Critical;

				case "Major":
					return TicketSeverity.Major;

				case "Minor":
				case "Warning":
					return TicketSeverity.Minor;

				default:
					return TicketSeverity.Minor;
			}
		}
	}
}