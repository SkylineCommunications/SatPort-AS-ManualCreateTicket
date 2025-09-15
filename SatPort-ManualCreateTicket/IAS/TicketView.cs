namespace Skyline.Automation.SatPort.IAS
{
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
			InitializeDropDown();
			InitializeButtons();

			SetupLayout();
		}

		public TextBox ShortDescription { get; private set; }

		public TextBox Description { get; private set; }

		public DropDown TicketType { get; private set; }

		public DropDown Impact { get; private set; }

		public TextBox WorkNotes { get; private set; }

		public Button CreateButton { get; private set; }

		public Button CancelButton { get; private set; }

		private void InitializeTextBox()
		{
			ShortDescription = new TextBox { Width = 300, Height = 30 };
			Description = new TextBox { Width = 300, Height = 30 };
			WorkNotes = new TextBox { Width = 300, Height = 200, IsMultiline = true };
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

			var shortDescriptionLabel = new Label("Short Description") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(shortDescriptionLabel, ShortDescription);

			var descriptionLabel = new Label("Description") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(descriptionLabel, Description);

			var ticketTypeLabel = new Label("Ticket Type") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(ticketTypeLabel, TicketType);

			var impactLabel = new Label("Impact") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(impactLabel, Impact);

			var workNotesLabel = new Label("Work Notes") { MinWidth = 200, Height = 30 };
			section.AppendWidgets(workNotesLabel, WorkNotes);

			section.AppendWidgets(new Widget[] { new WhiteSpace() });
			section.AppendWidgets(CancelButton, CreateButton);

			this.AddSection(section, 0, 0);
		}
	}
}