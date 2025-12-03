
/*
버그를 수정하고 표준편차 밴드를 추가한 원시 파일
VWAP_Bands 지표에서 사용한다
*/

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
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;

#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class _iVWAP_Bands : Indicator
	{
		private	Series<double>	iCumVolume;
		private	Series<double>	iCumTypicalVolume;
		private	Series<double>	iCumSum;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Calculate	= Calculate.OnEachTick;
				IsOverlay	= true;

				AddPlot(Brushes.DarkSlateGray,	"Vwap");
				AddPlot(Brushes.DodgerBlue,		"Upper");
				AddPlot(Brushes.Firebrick,		"Lower");
			}
			else if (State == State.DataLoaded)
			{
				iCumVolume			= new Series<double>(this, MaximumBarsLookBack.Infinite);
				iCumTypicalVolume	= new Series<double>(this, MaximumBarsLookBack.Infinite);
				iCumSum				= new Series<double>(this, MaximumBarsLookBack.Infinite);
			}
			else if (State == State.Terminated)
			{
			}
		}

		protected override void OnBarUpdate()
		{
			if (Bars.IsFirstBarOfSession)
			{
				iCumVolume[0]			= Volume[0];
				iCumTypicalVolume[0]	= Volume[0] * Typical[0];
				iCumSum[0]				= Volume[0] * Typical[0] * Typical[0];
			}
			else
			{
				iCumVolume[0]			= iCumVolume[1] + Volume[0];
				iCumTypicalVolume[0]	= iCumTypicalVolume[1] + (Volume[0] * Typical[0]);
				iCumSum[0]				= iCumSum[1] + (Volume[0] * Typical[0] * Typical[0]);
			}

			Vwap[0]		= iCumTypicalVolume[0] / iCumVolume[0];
			double dev	= Math.Sqrt(Math.Max((iCumSum[0] / iCumVolume[0]) - (Vwap[0] * Vwap[0]), 0));

			Upper[0] = Vwap[0] + dev;
			Lower[0] = Vwap[0] - dev;
		}

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> Vwap
		{
			get { return Values[0]; }
		}

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> Upper
		{
			get { return Values[1]; }
		}

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> Lower
		{
			get { return Values[2]; }
		}
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private _iVWAP_Bands[] cache_iVWAP_Bands;
		public _iVWAP_Bands _iVWAP_Bands()
		{
			return _iVWAP_Bands(Input);
		}

		public _iVWAP_Bands _iVWAP_Bands(ISeries<double> input)
		{
			if (cache_iVWAP_Bands != null)
				for (int idx = 0; idx < cache_iVWAP_Bands.Length; idx++)
					if (cache_iVWAP_Bands[idx] != null &&  cache_iVWAP_Bands[idx].EqualsInput(input))
						return cache_iVWAP_Bands[idx];
			return CacheIndicator<_iVWAP_Bands>(new _iVWAP_Bands(), input, ref cache_iVWAP_Bands);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators._iVWAP_Bands _iVWAP_Bands()
		{
			return indicator._iVWAP_Bands(Input);
		}

		public Indicators._iVWAP_Bands _iVWAP_Bands(ISeries<double> input )
		{
			return indicator._iVWAP_Bands(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators._iVWAP_Bands _iVWAP_Bands()
		{
			return indicator._iVWAP_Bands(Input);
		}

		public Indicators._iVWAP_Bands _iVWAP_Bands(ISeries<double> input )
		{
			return indicator._iVWAP_Bands(input);
		}
	}
}

#endregion
