namespace Skyline.Automation.SatPort.IAS
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Skyline.Automation.SatPort.Helpers;
	using Skyline.Automation.SatPort.IAS.Interfaces;
	using Skyline.DataMiner.SDM;
	using Skyline.DataMiner.SDM.Ticketing.Models;
	using TicketType = Skyline.DataMiner.SDM.Ticketing.Models.TicketType;

	public class TicketPresenter : IPresenter
	{
		private readonly ITicketView view;
		private readonly ITicketModel model;

		public TicketPresenter(ITicketView view, ITicketModel model)
		{
			this.view = view;
			this.model = model;

			InitPresenterEvents();
			LoadFromModel();
		}

		public event EventHandler<EventArgs> Cancel;

		public event EventHandler<EventArgs> Create;

		public void LoadFromModel()
		{
			var alarmMessage = model.ScriptContext.AlarmEvent;

			view.ShortDescription.Text = $"{alarmMessage.ParameterName}@{alarmMessage.Value}";
			view.Description.Text = $"{alarmMessage.ElementName} - {view.ShortDescription.Text}";
			view.TicketType.SetOptions(model.TicketTypes.Select(x => x.Name));
			view.Impact.SetOptions(new List<string> { Impact.High, Impact.Medium, Impact.Low });
		}

		public void StoreToModel()
		{
			model.Ticket.Name = view.ShortDescription.Text;
			model.Ticket.Description = view.Description.Text;
			model.Ticket.Type = new SdmObjectReference<TicketType>(model.TicketTypes.First(t => t.Name == view.TicketType.Selected).Guid);
			model.Ticket.Severity = ConvertImpactToSeverity();
			model.Ticket.RequestedResolutionDate = DateTime.Now.AddDays(7);
			model.Ticket.ExpectedResolutionDate = DateTime.Now.AddDays(3);

			CreateTicketNote();
			CreateAffectedResource();

			model.Helper.Tickets.Create(model.Ticket);
			model.ScriptContext.EditAlarmProperty("Ticket ID", model.Ticket.ID);
		}

		private TicketSeverity ConvertImpactToSeverity()
		{
			switch (view.Impact.Selected)
			{
				case Impact.High:
					{
						return TicketSeverity.Critical;
					}

				case Impact.Medium:
					{
						return TicketSeverity.Major;
					}

				case Impact.Low:
					{
						return TicketSeverity.Minor;
					}

				default:
					throw new InvalidCastException($"Unsupported impact value: {view.Impact.Selected}");
			}
		}

		private void CreateTicketNote()
		{
			if (!string.IsNullOrWhiteSpace(view.WorkNotes.Text))
			{
				model.Helper.Notes.Create(new TicketNote
				{
					Note = view.WorkNotes.Text,
					Ticket = model.Ticket,
				});
			}
		}

		private void CreateAffectedResource()
		{
			var alarmMessage = model.ScriptContext.AlarmEvent;
			model.Helper.AffectedResources.Create(new TicketAffectedResource
			{
				Id = $"{alarmMessage.DataMinerID}/{alarmMessage.ElementID}/{alarmMessage.AlarmID}",
				Name = $"{alarmMessage.ElementName}",
				Type = AffectedResourceType.Alarm,
				State = alarmMessage.Status,
				Ticket = model.Ticket,
			});
		}

		private void InitPresenterEvents()
		{
			view.CancelButton.Pressed += OnCancelButtonPressed;
			view.CreateButton.Pressed += OnCreateButtonPressed;
		}

		private void OnCancelButtonPressed(object sender, EventArgs e)
		{
			Cancel?.Invoke(this, EventArgs.Empty);
		}

		private void OnCreateButtonPressed(object sender, EventArgs e)
		{
			StoreToModel();
			Create?.Invoke(this, EventArgs.Empty);
		}
	}
}