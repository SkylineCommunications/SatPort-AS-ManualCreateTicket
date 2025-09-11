namespace Skyline.Automation.SatPort.IAS
{
	using System;
	using System.Linq;
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

			view.Name.Text = $"{alarmMessage.ParameterName}@{alarmMessage.Value}";
			view.Description.Text = $"{alarmMessage.ElementName} - {view.Name.Text}";
			view.TicketType.SetOptions(model.TicketTypes.Select(x => x.Name));
			view.Impact.SetOptions(Enum.GetNames(typeof(TicketSeverity)));
		}

		public void StoreToModel()
		{
			model.Ticket.Name = view.Name.Text;
			model.Ticket.Description = view.Description.Text;
			model.Ticket.Type = new SdmObjectReference<TicketType>(model.TicketTypes.First(t => t.Name == view.TicketType.Selected).Guid);
			model.Ticket.Severity = (TicketSeverity)Enum.Parse(typeof(TicketSeverity), view.Impact.Selected);

			model.Helper.Tickets.Create(model.Ticket);

			if(!string.IsNullOrWhiteSpace(view.WorkNotes.Text))
			{
				model.Helper.Notes.Create(new TicketNote
				{
					Note = view.WorkNotes.Text,
					Ticket = model.Ticket,
				});
			}
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