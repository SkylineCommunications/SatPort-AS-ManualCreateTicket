namespace Skyline.Automation.SatPort.IAS
{
	using Skyline.Automation.SatPort.Context;
	using Skyline.Automation.SatPort.Helpers;
	using Skyline.DataMiner.Core.SRM.Utils.Automation.Exceptions;
	using Skyline.DataMiner.SDM.Ticketing;

	public class TicketWizard
	{
		private readonly TicketModel model;
		private readonly TicketView view;
		private readonly TicketPresenter presenter;

		private ScriptContext scriptContext;

		public TicketWizard(ScriptContext scriptContext, TicketingApiHelper helper)
		{
			this.scriptContext = scriptContext;

			model = new TicketModel(scriptContext, helper);
			view = new TicketView(scriptContext.Engine);
			presenter = new TicketPresenter(view, model);

			InitializeWizardEvents();
		}

		public TicketModel Model => model;

		public TicketView View => view;

		public TicketPresenter Presenter => presenter;

		private void InitializeWizardEvents()
		{
			presenter.Cancel += (sender, args) =>
			{
				scriptContext.Engine.ExitSuccess(ScreenMessages.ExitSuccessMessage);
			};

			presenter.Create += (sender, args) =>
			{
				throw new CloseUserInteractionException();
			};
		}
	}
}