using System;

namespace FutCalc
{
	partial class Program
	{
		
		public class Product
		{
			public string ProductName { get; set; }
			public ProductType ProductType { get; set; }
			public double Entry { get; set; }
			public double StopLoss { get; set; }
			public double TakeProfit { get; set; }
			public double MinimumTickFluctuation { get; set; }
			public double TickValue { get; set; }
			public double RewardTicks
			{
				get
				{
					return Math.Abs(Entry - TakeProfit) / MinimumTickFluctuation;
				}
			}
			public double Reward
			{
				get
				{
					return RewardTicks * TickValue;
				}
			}
			public double RiskTicks 
			{ 
				get 
				{ 
					return Math.Abs(Entry - StopLoss) / MinimumTickFluctuation; 
				} 
			}
			public double Risk
			{
				get
				{
					return RiskTicks * TickValue;
				}
			}

			public Product(ConsoleKey inputKey)
			{
				switch (inputKey)
				{
					case ConsoleKey.D1:
						ProductName = "MICRO E-MINI S&P 500";
						ProductType = ProductType.SP;
						MinimumTickFluctuation = 0.25f;
						TickValue = 1.25f;
						break;
					case ConsoleKey.D2:
						ProductName = "MICRO E-MINI NASDAQ-100";
						ProductType = ProductType.NASDAQ;
						MinimumTickFluctuation = 0.25f;
						TickValue = 0.50f;
						break;
					case ConsoleKey.D3:
						ProductName = "MICRO E-MINI RUSSELL 2000";
						ProductType = ProductType.RUSSELL;
						MinimumTickFluctuation = 0.10f;
						TickValue = 0.50f;
						break;
					case ConsoleKey.D4:
						ProductName = "MICRO E-MINI DOW";
						ProductType = ProductType.DOW;
						MinimumTickFluctuation = 1f;
						TickValue = 0.50f;
						break;
				}
			}
		}
	}
}
