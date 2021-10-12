using System;
using System.Text;

namespace FutCalc
{
	partial class Program
	{
		private static ConsoleKey[] _menuKeys = new ConsoleKey[] { ConsoleKey.D1, ConsoleKey.D2, ConsoleKey.D3, ConsoleKey.D4 };

		static void Main(string[] args)
		{
			Console.Title = "Micro E-mini Futures Risk Calculator";

			var logoStrBldr = new StringBuilder();
			logoStrBldr.Append("\n");
			logoStrBldr.Append("███████╗██╗   ██╗████████╗ ██████╗ █████╗ ██╗      ██████╗\n");
			logoStrBldr.Append("██╔════╝██║   ██║╚══██╔══╝██╔════╝██╔══██╗██║     ██╔════╝\n");
			logoStrBldr.Append("█████╗  ██║   ██║   ██║   ██║     ███████║██║     ██║     \n");
			logoStrBldr.Append("██╔══╝  ██║   ██║   ██║   ██║     ██╔══██║██║     ██║     \n");
			logoStrBldr.Append("██║     ╚██████╔╝   ██║   ╚██████╗██║  ██║███████╗╚██████╗\n");
			logoStrBldr.Append("╚═╝      ╚═════╝    ╚═╝    ╚═════╝╚═╝  ╚═╝╚══════╝ ╚═════╝ v1.0\n");

			Console.Clear();
			Write(logoStrBldr.ToString());

			Run();
		}

		private static void Run()
		{
			var run = true;

			do
			{
				var selectStr = "Select product:\n\t<1> MICRO E-MINI S&P 500\n\t<2> MICRO E-MINI NASDAQ-100\n\t<3> MICRO E-MINI RUSSELL 2000\n\t<4> MICRO E-MINI DOW\n\t<ESC> Exit..";
				Write(selectStr);

				var consoleKeyInfo = Console.ReadKey(true);
				var selectedKey = consoleKeyInfo.Key;

				if (selectedKey == ConsoleKey.Escape)
				{
					run = !run;
				}
				else if (ContainsKey(_menuKeys, selectedKey))
				{
					var model = new Product(selectedKey);

					Console.Clear();
					DisplayLog($"{model.ProductName}", ConsoleColor.DarkGreen);

					if (ValidateUserInput(model))
					{
						var riskStr = $"| Risk: ${String.Format("{0:0.00}", model.Risk)} |";
						var rewardStr = $"| Reward: ${String.Format("{0:0.00}", model.Reward)} |";
						var separatorStr = $"\t{new string('-', riskStr.Length)}\t{new string('-', rewardStr.Length)}\n";

						var stringBldr = new StringBuilder();
						stringBldr.Append("\n");
						stringBldr.Append(separatorStr);
						stringBldr.Append($"\t{riskStr}\t{rewardStr}\n");
						stringBldr.Append(separatorStr);
						stringBldr.Append("\n");

						Write(stringBldr.ToString());

						Write("\tReturn to menu...");
						Console.ReadKey();
						Console.Clear();
					}
				}
				else
				{
					DisplayLog($"{selectedKey} is an invalid selection.", ConsoleColor.DarkRed);
				}

			} while (run);
		}

		static bool ValidateUserInput(Product model) 
		{
			Write("\n\tEntry: ", writeLine: false);
			model.Entry = ParseDouble(Console.ReadLine(), model);
			if(CheckDoubleDefault(model.Entry))
			{
				return false;
			}

			Write("\tStop-Loss: ", writeLine: false);
			model.StopLoss = ParseDouble(Console.ReadLine(), model);
			if (CheckDoubleDefault(model.StopLoss))
			{
				return false;
			}

			if(model.Entry == model.StopLoss)
			{
				DisplayLog("Stop-Loss cannot equal Entry.", ConsoleColor.DarkRed);
				return false;
			}

			Write("\tTake-Profit: ", writeLine: false);
			model.TakeProfit = ParseDouble(Console.ReadLine(), model);
			if (CheckDoubleDefault(model.TakeProfit))
			{
				return false;
			}

			if (model.Entry > model.StopLoss)
			{
				var valid = model.TakeProfit > model.Entry;

				if (!valid)
				{
					DisplayLog("Take-Profit must be > Entry.", ConsoleColor.DarkRed);
				}

				return valid;			
			}
			else if (model.Entry < model.StopLoss)
			{
				var valid = model.TakeProfit < model.Entry;

				if (!valid)
				{
					DisplayLog("Take-Profit must be < Entry.", ConsoleColor.DarkRed);
				}

				return valid;
			}

			return false;
		}

		static bool CheckDoubleDefault(double value) => value == default;

		static bool ContainsKey(ConsoleKey[] menuKeys, ConsoleKey selectedKey)
		{
			for (int i = 0; i < menuKeys.Length; i++)
			{
				if(menuKeys[i] == selectedKey)
				{
					return true;
				}
			}

			return false;
		}

		static double ParseDouble(string input, Product model)
		{
			double value = default;

			try
			{
				value = double.Parse(input);

				if(model.ProductType == ProductType.RUSSELL)
				{
					value = Math.Round(value, 1);
				}
				else if (value % model.MinimumTickFluctuation != 0)
				{					
					DisplayLog($"Invalid {nameof(model.MinimumTickFluctuation)}, must be a minimal increment of {String.Format("{0:0.00}", model.MinimumTickFluctuation)} for {model.ProductName}.", ConsoleColor.DarkRed);
					value = default;
				}
			}
			catch (FormatException ex)
			{
				DisplayLog($"Unable to parse '{input}'. {ex.Message}", ConsoleColor.DarkRed);			
			}

			return value;
		}

		static void Write(string value, bool writeLine = true)
		{
			if (writeLine)
				Console.WriteLine(value);
			else
				Console.Write(value);
		}

		static void DisplayLog(string value, ConsoleColor backgroundColor)
		{
			Console.Clear();	
			Console.BackgroundColor = backgroundColor;
			Console.ForegroundColor = ConsoleColor.White;
			Write(value.PadRight(Console.WindowWidth - 1));
			Console.ResetColor();
		}
	}
}
