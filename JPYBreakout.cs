#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Strategies in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Strategies
{
	public class JPYBreakout : Strategy
	{
		private HMA HMA1;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Strategy here.";
				Name										= "JPYBreakout";
				Calculate									= Calculate.OnBarClose;
				EntriesPerDirection							= 1;
				EntryHandling								= EntryHandling.AllEntries;
				IsExitOnSessionCloseStrategy				= true;
				ExitOnSessionCloseSeconds					= 30;
				IsFillLimitOnTouch							= false;
				MaximumBarsLookBack							= MaximumBarsLookBack.TwoHundredFiftySix;
				OrderFillResolution							= OrderFillResolution.Standard;
				Slippage									= 0;
				StartBehavior								= StartBehavior.WaitUntilFlat;
				TimeInForce									= TimeInForce.Gtc;
				TraceOrders									= false;
				RealtimeErrorHandling						= RealtimeErrorHandling.StopCancelClose;
				StopTargetHandling							= StopTargetHandling.PerEntryExecution;
				BarsRequiredToTrade							= 20;
				// Disable this property for performance gains in Strategy Analyzer optimizations
				// See the Help Guide for additional information
				IsInstantiatedOnEachOptimizationIteration	= true;
				W					= 1000;
				S					= 1000;
				TimeX					= 70000;
				TimeY					= 1600;
				Length					= 24;
			}
			else if (State == State.Configure)
			{
			}
			else if (State == State.DataLoaded)
			{				
				HMA1				= HMA(Close, Convert.ToInt32(Length));
				SetProfitTarget("", CalculationMode.Currency, W);
				SetStopLoss("", CalculationMode.Currency, S, false);
			}
		}

		protected override void OnBarUpdate()
		{
			if (BarsInProgress != 0) 
				return;

			if (CurrentBars[0] < 1)
				return;

			 // Set 1
			if ((Times[0][0].TimeOfDay == new TimeSpan(7, 0, 0))
				 && (HMA1[0] > HMA1[1]))
			{
				EnterLong(Convert.ToInt32(DefaultQuantity), "");
			}
			
			 // Set 2
			if ((Times[0][0].TimeOfDay == new TimeSpan(7, 0, 0))
				 && (HMA1[0] < HMA1[1]))
			{
				EnterShort(Convert.ToInt32(DefaultQuantity), "");
			}
			
			 // Set 3
			if ((Times[0][0].TimeOfDay == new TimeSpan(16, 0, 0))
				 && (Position.MarketPosition == MarketPosition.Long))
			{
				ExitLong(Convert.ToInt32(DefaultQuantity), "", "");
			}
			
			 // Set 4
			if ((Times[0][0].TimeOfDay == new TimeSpan(16, 0, 0))
				 && (Position.MarketPosition == MarketPosition.Short))
			{
				ExitShort(Convert.ToInt32(DefaultQuantity), "", "");
			}
			
		}

		#region Properties
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="W", Order=1, GroupName="Parameters")]
		public int W
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="S", Order=2, GroupName="Parameters")]
		public int S
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="TimeX", Order=3, GroupName="Parameters")]
		public int TimeX
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="TimeY", Order=4, GroupName="Parameters")]
		public int TimeY
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="Length", Order=5, GroupName="Parameters")]
		public int Length
		{ get; set; }
		#endregion

	}
}
