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