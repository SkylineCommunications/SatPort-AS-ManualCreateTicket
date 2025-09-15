namespace Skyline.Automation.SatPort.Context
{
	using System;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.Messages;

	public class ScriptContext : IScriptContext
	{
		public ScriptContext(Engine engine)
		{
			Engine = engine;
			InitializeContext(engine);
		}

		public int AlarmId { get; private set; }

		public int ElementId { get; private set; }

		public int DataMinerId { get; private set; }

		public Engine Engine { get; }

		public AlarmEventMessage AlarmEvent { get; private set; }

		public void EditAlarmProperty(string propertyName, string propertyValue)
		{
			var propertyArray = new PSA
			{
				Psa = new[]
				{
					new SA
					{
						Sa = new[] { propertyName, "read-write", propertyValue ?? "Unknown" },
					},
				},
			};

			var infoMessage = new Skyline.DataMiner.Net.Messages.Advanced.SetDataMinerInfoMessage
			{
				bInfo1 = Int32.MaxValue,
				bInfo2 = Int32.MaxValue,
				DataMinerID = DataMinerId,
				ElementID = -1,
				HostingDataMinerID = DataMinerId,
				IInfo1 = Int32.MaxValue,
				IInfo2 = Int32.MaxValue,
				Psa2 = propertyArray,
				What = (int)NotifyType.EditProperty,
				StrInfo1 = $"alarm:{AlarmId}:{DataMinerId}:{ElementId}",
			};

			Engine.SendSLNetMessage(infoMessage);
		}

		private void InitializeContext(Engine engine)
		{
			DataMinerId = Convert.ToInt32(engine.GetScriptParam("DmaID")?.Value);
			ElementId = Convert.ToInt32(engine.GetScriptParam("ElementID")?.Value);
			AlarmId = Convert.ToInt32(engine.GetScriptParam("AlarmID")?.Value);

			AlarmEvent = GetAlarm();
		}

		private AlarmEventMessage GetAlarm()
		{
			var getAlarmDetailsMessage = new GetAlarmDetailsMessage(new Skyline.DataMiner.Net.AlarmTreeID(DataMinerId, ElementId, AlarmId));

			var response = Engine.SendSLNetMessage(getAlarmDetailsMessage);

			var alarmDetails = (AlarmEventMessage)response[0];

			return alarmDetails;
		}
	}
}