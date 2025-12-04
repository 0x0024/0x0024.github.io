
#region Using declarations
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Data;

using NinjaTrader.NinjaScript.DrawingTools;
using System.Runtime.InteropServices;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	/// <summary>
	/// Displays ask, bid, and/or last lines on the chart.
	/// </summary>
	public class _Price_Line : Indicator
	{
		private double preAsk	= -1;
		private double preBid	= -1;
		private double preLast	= -1;

		private System.Timers.Timer myTimer;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description						= @"_Price_Line";
				Name							= "_Price_Line";

				Calculate						= Calculate.OnBarClose;
				IsOverlay						= true;

				AskStroke						= new Stroke(Brushes.DarkGreen, DashStyleHelper.Solid, 1);
				BidStroke						= new Stroke(Brushes.Blue, DashStyleHelper.Solid, 1);
				LastStroke						= new Stroke(Brushes.Yellow, DashStyleHelper.Solid, 1);
			}
			else if (State == State.Configure)
			{
			}
			else if (State == State.DataLoaded)
			{
				myTimer = new System.Timers.Timer(200); // 1초 : 1000
				myTimer.Elapsed += TimerEventProcessor;
				myTimer.Enabled = true;
			}
			else if (State == State.Terminated)
			{
				// Stops the timer and removes the timer event handler
				if (myTimer != null)
				{
					myTimer.Enabled = false;
					myTimer.Elapsed -= TimerEventProcessor;
					myTimer = null;
				}
			}
		}

		protected override void OnBarUpdate()
		{
		}
		
		private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
		{
			if (State == State.Historical) return;
			TriggerCustomEvent(PrintThePrice, Close[0]);
		}

		private void PrintThePrice(object price)
		{
			if (preAsk != GetCurrentAsk())
			{
				preAsk = GetCurrentAsk();
				Draw.HorizontalLine(this, "e.Ask", false, preAsk, AskStroke.Brush, DashStyleHelper.Solid, 1);
			}

			if (preBid != GetCurrentBid())
			{
				preBid = GetCurrentBid();
				Draw.HorizontalLine(this, "e.Bid", false, preBid, BidStroke.Brush, DashStyleHelper.Solid, 1);
			}
		}

		protected override void OnMarketData(MarketDataEventArgs e)
		{
			if (e.MarketDataType != MarketDataType.Last || 0 > CurrentBar) return;

			if (preAsk != e.Ask)
			{
				preAsk = e.Ask;
				Draw.HorizontalLine(this, "e.Ask", false, e.Ask, AskStroke.Brush, DashStyleHelper.Solid, 1);
			}

			if (preBid != e.Bid)
			{
				preBid = e.Bid;
				Draw.HorizontalLine(this, "e.Bid", false, e.Bid, BidStroke.Brush, DashStyleHelper.Solid, 1);
			}

			if (preLast != e.Price)
			{
				preLast = e.Price;
				Draw.HorizontalLine(this, "e.Price", false, e.Price, LastStroke.Brush, DashStyleHelper.Solid, 1);
			}
		}

        #region Miscellaneous

		public override string FormatPriceMarker(double price)
		{
			double	trunc		= Math.Truncate(price);
			double	fraction	= Convert.ToInt32(320 * Math.Abs(price - trunc) - 0.0001); // rounding down for ZF and ZT
			string	priceMarker	= "";

			if (Instrument.MasterInstrument.Compare(TickSize, 0.03125) == 0) 
			{
				priceMarker = string.Format("{0}'{1:D2}", trunc, (int)fraction / 10);
			}
			else if (Instrument.MasterInstrument.Compare(TickSize, 0.015625) == 0 || Instrument.MasterInstrument.Compare(TickSize, 0.0078125) == 0)
			{
				priceMarker = string.Format("{0}'{1:00.0}", trunc, fraction / 10);
			}
			else
				priceMarker = price.ToString(Core.Globals.GetTickFormatString(TickSize));

			if (0 > price && -1 == priceMarker.IndexOf('-'))
				priceMarker = "-" + priceMarker;

			return priceMarker;
		}

		#endregion

		#region Properties

		[Display(ResourceType = typeof(Custom.Resource), Name = "Ask", Order = 0)]
		public Stroke AskStroke { get; set; }

		[Display(ResourceType = typeof(Custom.Resource), Name = "Bid", Order = 1)]
		public Stroke BidStroke { get; set; }

		[Display(ResourceType = typeof(Custom.Resource), Name = "Last", Order = 2)]
		public Stroke LastStroke { get; set; }

		#endregion
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private _Price_Line[] cache_Price_Line;
		public _Price_Line _Price_Line()
		{
			return _Price_Line(Input);
		}

		public _Price_Line _Price_Line(ISeries<double> input)
		{
			if (cache_Price_Line != null)
				for (int idx = 0; idx < cache_Price_Line.Length; idx++)
					if (cache_Price_Line[idx] != null &&  cache_Price_Line[idx].EqualsInput(input))
						return cache_Price_Line[idx];
			return CacheIndicator<_Price_Line>(new _Price_Line(), input, ref cache_Price_Line);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators._Price_Line _Price_Line()
		{
			return indicator._Price_Line(Input);
		}

		public Indicators._Price_Line _Price_Line(ISeries<double> input )
		{
			return indicator._Price_Line(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators._Price_Line _Price_Line()
		{
			return indicator._Price_Line(Input);
		}

		public Indicators._Price_Line _Price_Line(ISeries<double> input )
		{
			return indicator._Price_Line(input);
		}
	}
}

#endregion
