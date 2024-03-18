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
	public class AgoritmicaMES : Strategy
	{
		private SMA SMA1;
		private Range Range1;
		private ATR ATR1;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Strategy here.";
				Name										= "AgoritmicaMES";
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
				Period					= 5;
				Multi_atr_L					= 1;
				Multi_atr_S					= 1;
			}
			else if (State == State.Configure)
			{
			}
			else if (State == State.DataLoaded)
			{				
				SMA1				= SMA(Close, Convert.ToInt32(Period));
				Range1				= Range(Close);
				ATR1				= ATR(Close, 10);
			}
		}

		protected override void OnBarUpdate()
		{
			if (BarsInProgress != 0) 
				return;

			if (CurrentBars[0] < 1)
				return;

			 // Set 1
			if ((SMA1[0] > Close[0])
				 && (Range1[0] > (ATR1[1] * Multi_atr_L) ))
			{
				EnterLongLimit(Convert.ToInt32(DefaultQuantity), Low[0], "");
			}
			
			 // Set 2
			if ((SMA1[0] < Close[0])
				 && (Range1[0] > (ATR1[1] * Multi_atr_S) ))
			{
				EnterShortLimit(Convert.ToInt32(DefaultQuantity), High[0], "");
			}
			
			 // Set 3
			if ((Close[0] > Open[0])
				 && (Close[1] > Open[1]))
			{
				ExitLong(Convert.ToInt32(DefaultQuantity), "", "");
			}
			
			 // Set 4
			if (Close[0] < Open[0])
			{
				ExitShort(Convert.ToInt32(DefaultQuantity), "", "");
			}
			
		}

		#region Properties
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="Period", Order=1, GroupName="Parameters")]
		public int Period
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="Multi_atr_L", Order=2, GroupName="Parameters")]
		public double Multi_atr_L
		{ get; set; }

		[NinjaScriptProperty]
		[Range(0, double.MaxValue)]
		[Display(Name="Multi_atr_S", Order=3, GroupName="Parameters")]
		public double Multi_atr_S
		{ get; set; }
		#endregion

	}
}
