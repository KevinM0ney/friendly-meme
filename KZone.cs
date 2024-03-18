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
	public class KZone : Strategy
	{
		private double Long1;
		private double Long2;
		private double Short1;
		private int Short2;
		private bool LongTrue;
		private bool ShortTrue;


		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Strategy here.";
				Name										= "KZone";
				Calculate									= Calculate.OnEachTick;
				EntriesPerDirection							= 2;
				EntryHandling								= EntryHandling.UniqueEntries;
				IsExitOnSessionCloseStrategy				= true;
				ExitOnSessionCloseSeconds					= 30;
				IsFillLimitOnTouch							= false;
				MaximumBarsLookBack							= MaximumBarsLookBack.TwoHundredFiftySix;
				OrderFillResolution							= OrderFillResolution.Standard;
				Slippage									= 0;
				StartBehavior								= StartBehavior.ImmediatelySubmitSynchronizeAccount;
				TimeInForce									= TimeInForce.Day;
				TraceOrders									= false;
				RealtimeErrorHandling						= RealtimeErrorHandling.StopCancelClose;
				StopTargetHandling							= StopTargetHandling.ByStrategyPosition;
				BarsRequiredToTrade							= 20;
				// Disable this property for performance gains in Strategy Analyzer optimizations
				// See the Help Guide for additional information
				IsInstantiatedOnEachOptimizationIteration	= true;
				W					= 1000;
				S					= 1500;
				Long1					= 1;
				Long2					= 1;
				Short1					= 1;
				Short2					= 1;
				LongTrue					= false;
				ShortTrue					= false;
			}
			else if (State == State.Configure)
			{
				SetProfitTarget(CalculationMode.Currency, W);
				SetStopLoss(CalculationMode.Currency, S);
			}
		}

		protected override void OnBarUpdate()
		{
			if (BarsInProgress != 0) 
				return;

			if (CurrentBars[0] < 1)
				return;

			 // Set 1
			if (Times[0][0].TimeOfDay == new TimeSpan(14, 30, 0))
			{
				Long1 = (Median[0] - 12) ;
				Short1 = (Median[0] + 12) ;
				Long2 = (Median[0] - 20) ;
				Short2 = Convert.ToInt32((Median[0] + 20) );
			}
			
			 // Set 3
			if ((Times[0][0].TimeOfDay == new TimeSpan(15, 30, 0))
				 && (Open[0] > Long1))
			{
				LongTrue = true;
			}
			
			 // Set 4
			if ((Times[0][0].TimeOfDay == new TimeSpan(15, 30, 0))
				 && (Open[0] < Short1))
			{
				ShortTrue = true;
			}
			
			if (
				 // Time
				((Times[0][0].TimeOfDay >= new TimeSpan(15, 30, 0))
				 && (Times[0][0].TimeOfDay == new TimeSpan(17, 0, 0)))
				 && (LongTrue == true))
			{
				EnterLongLimit(1, Long1, @"Long KZone");
				EnterLongLimit(1, Long2, @"Pray Hard Long");
			}
			
			 // Set 5
			if (
				 // Time
				((Times[0][0].TimeOfDay >= new TimeSpan(15, 30, 0))
				 && (Times[0][0].TimeOfDay == new TimeSpan(17, 0, 0)))
				 && (ShortTrue == true))
			{
				EnterShortLimit(1, Short1, @"Short KZone");
				EnterShortLimit(1, Short2, @"Pray Hard Short");
			}
			
			 // Set 6
			if ((Times[0][0].TimeOfDay == new TimeSpan(19, 0, 0))
				 && (Position.MarketPosition == MarketPosition.Long))
			{
				if(Position.Quantity == 1)
					ExitLong(Convert.ToInt32(DefaultQuantity), @"Bye Long", "");
				else ExitLong(Convert.ToInt32(2), @"Bye 2Long", "");
			}
			
			 // Set 7
			if ((Times[0][0].TimeOfDay == new TimeSpan(19, 0, 0))
				 && (Position.MarketPosition == MarketPosition.Short))
			{
				if(Position.Quantity == 1)
					ExitShort(Convert.ToInt32(DefaultQuantity), @"Bye Short", "");
				else ExitShort(Convert.ToInt32(2), @"Bye 2Short", "");
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
		#endregion

	}
}
