/*
****************************************************************************
*  Copyright (c) 2025,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

11/08/2025	1.0.0.1		BHO, Skyline	Initial version
****************************************************************************
*/

namespace SatPortManualCreateTicket
{
	using System;
	using System.Linq;
	using Context;
	using Newtonsoft.Json;
	using SatPort_ManualCreateTicket;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.SDM.Ticketing;
	using Skyline.DataMiner.SDM.Ticketing.Models;
	using TicketType = Skyline.DataMiner.SDM.Ticketing.Models.TicketType;

	/// <summary>
	/// Represents a DataMiner Automation script.
	/// </summary>
	public class Script
	{
		/// <summary>
		/// The script entry point.
		/// </summary>
		/// <param name="engine">Link with SLAutomation process.</param>
		public void Run(Engine engine)
		{
			try
			{
				// Initialize script context
				var scriptContext = new ScriptContext(engine);

				RunSafe(scriptContext);
			}
			catch (ScriptAbortException)
			{
				// Catch normal abort exceptions (engine.ExitFail or engine.ExitSuccess)
				throw; // Comment if it should be treated as a normal exit of the script.
			}
			catch (ScriptForceAbortException)
			{
				// Catch forced abort exceptions, caused via external maintenance messages.
				throw;
			}
			catch (ScriptTimeoutException)
			{
				// Catch timeout exceptions for when a script has been running for too long.
				throw;
			}
			catch (InteractiveUserDetachedException)
			{
				// Catch a user detaching from the interactive script by closing the window.
				// Only applicable for interactive scripts, can be removed for non-interactive scripts.
				throw;
			}
			catch (Exception e)
			{
				engine.ExitFail("Run|Something went wrong: " + e);
			}
		}

		private void RunSafe(IScriptContext context)
		{
			var userConnection = context.Engine.GetUserConnection();

			var ticketingApiHelper = new TicketingApiHelper(userConnection);
			var ticketType = PrepareTicketType(ticketingApiHelper);

			var dmaTicket = new DmaTicket(context.AlarmEvent)
			{
				Type = ticketType,
			};

			ticketingApiHelper.Tickets.Create(dmaTicket);
		}

		/// <summary>
		/// Gets the relevant ticket type. TODO: deduce from alarm, element, environment, parameter, etc.
		/// </summary>
		/// <param name="helper">Ticketing API Helper.</param>
		/// <returns>The <see cref="TicketType"/> to assign to a new ticket.</returns>
		private TicketType PrepareTicketType(TicketingApiHelper helper)
		{
			var unclassifiedIssueTicketType = new TicketType
			{
				Guid = Guid.NewGuid(),
				Name = "Unclassified Issue",
			};
			var ticketTypeFilter = TicketTypeExposers.Name.Equal(unclassifiedIssueTicketType.Name);

			var ticketType = helper.TicketTypes.Read(ticketTypeFilter).FirstOrDefault();
			if (ticketType is null)
			{
				helper.TicketTypes.Create(unclassifiedIssueTicketType);
				return unclassifiedIssueTicketType;
			}
			else
			{
				return ticketType;
			}
		}
	}
}