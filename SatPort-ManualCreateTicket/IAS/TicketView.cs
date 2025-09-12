namespace Skyline.Automation.SatPort.IAS
{
	using System;
	using Skyline.Automation.SatPort.IAS.IasExtensions;
	using Skyline.Automation.SatPort.IAS.Interfaces;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class TicketView : Dialog, ITicketView
	{
		public TicketView(IEngine engine) : base(engine)
		{
			Title = "Create new ticket";
			MinWidth = 500;

			InitializeTextBox();
			InitializeDateTimePicker();
			InitializeDropDown();
			InitializeButtons();

			SetupLayout();
		}

		public TextBox Name { get; private set; }

		public TextBox Description { get; private set; }

		public DropDown TicketType { get; private set; }

		public DropDown Impact { get; private set; }

		public TextBox WorkNotes { get; private set; }

		public DateTimePicker RequestResolutionDate { get; private set; }

		public DateTimePicker ExpectedResolutionDate { get; private set; }

		public Button CreateButton { get; private set; }

		public Button CancelButton { get; private set; }

		private void InitializeTextBox()
		{
			Name = new TextBox { Width = 300, Height = 30 };
			Description = new TextBox { Width = 300, Height = 30 };
			WorkNotes = new TextBox { Width = 300, Height = 200, IsMultiline = true };
		}

		private void InitializeDateTimePicker()
		{
			RequestResolutionDate = new DateTimePicker { Width = 300, Kind = DateTimeKind.Unspecified, Height = 30 };
			ExpectedResolutionDate = new DateTimePicker { Width = 300, Kind = DateTimeKind.Unspecified, Height = 30 };
		}

		private void InitializeDropDown()
		{
			TicketType = new DropDown { IsDisplayFilterShown = true, Width = 300, Height = 30 };
			Impact = new DropDown { Width = 300, Height = 30};
		}

		private void InitializeButtons()
		{
			CreateButton = new Button("Create") { MinWidth = 200, Height = 30 };
			CancelButton = new Button("Cancel") { MinWidth = 200, Height = 30 };
		}

		private void SetupLayout()
		{
			var section = new Section();

			var nameLabel = new Label("Name") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(nameLabel, Name);

			var descriptionLabel = new Label("Description") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(descriptionLabel, Description);

			var ticketTypeLabel = new Label("Ticket Type") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(ticketTypeLabel, TicketType);

			var impactLabel = new Label("Impact") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(impactLabel, Impact);

			var requestResolutionDateLabel = new Label("Request Resolution Date") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(requestResolutionDateLabel, RequestResolutionDate);

			var expectedResolutionDateLabel = new Label("Expected Resolution Date") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(expectedResolutionDateLabel, ExpectedResolutionDate);

			var workNotesLabel = new Label("Work Notes") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(workNotesLabel, WorkNotes);

			section.AppendWidgets(new Widget[] { new WhiteSpace() });
			section.AppendWidgets(CancelButton, CreateButton);

			this.AddSection(section, 0, 0);
		}
	}
}