namespace Context
{
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.Messages;

	public interface IScriptContext
	{
		int AlarmId { get; }

		int ElementId { get; }

		int DataMinerId { get; }

		Engine Engine { get; }

		AlarmEventMessage AlarmEvent { get; }
	}
}