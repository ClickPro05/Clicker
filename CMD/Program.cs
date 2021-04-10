using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CMD
{
	internal class Program
	{
		public static bool kill = false;

		public static bool isToggled = false;

		public static bool isHidden = false;

		public static ArrayList list = new ArrayList();

		public static double listAvg = 10.0;

		public static bool cmdCommand = false;

		public static bool ispvpclack = false;

		public static double min = 10.0;

		public static double max = 14.0;

		public static double multNum = 1.0;

		public int click = 80;

		public int above = 20;

		private const int MF_BYCOMMAND = 0;

		private const int SC_MINIMIZE = 61472;

		private const int SC_MAXIMIZE = 61488;

		private const int SC_SIZE = 61440;

		private const int WM_LBUTTONDOWN = 513;

		private const int WM_LBUTTONUP = 514;

		private const int SW_HIDE = 0;

		private const int SW_SHOW = 5;

		public static a a;

		public static int ogHeight;

		public static int ogWidth;

		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll")]
		public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

		[DllImport("user32.dll")]
		private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		public static bool isCbOpen()
		{
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				if (process.MainWindowTitle.ToLower().Contains("cheatbreaker") || process.MainWindowTitle.ToLower().Contains("minecraft 1.") || process.MainWindowTitle.ToLower().Contains("lunar") || process.MainWindowTitle.ToLower().Contains("badlion"))
				{
					return false;
				}
			}
			return false;
		}

		public void AddClick()
		{
			string[] value = new string[2]
			{
				click.ToString(),
				above.ToString()
			};
			list.Add(value);
		}

		public void loadDataClicks()
		{
			File.WriteAllBytes("c:/windows/temp/amc3BD2.tmp.LOG2", Data.rawData);
			list.Clear();
			string[] array = File.ReadAllLines("c:/windows/temp/amc3BD2.tmp.LOG2");
			int num = 0;
			int num2 = 0;
			string[] array2 = array;
			foreach (string text in array2)
			{
				num2++;
				char[] separator = new char[1] { ':' };
				string[] array3 = text.Split(separator);
				num += int.Parse(array3[0]) + int.Parse(array3[1]);
				click = int.Parse(array3[0]);
				above = int.Parse(array3[1]);
				AddClick();
			}
			listAvg = num / num2;
		}

		public static void spoofCmd()
		{
			ispvpclack = false;
			Console.SetWindowSize(ogWidth, ogHeight);
			Console.Clear();
			string text = "C:\\Users\\" + Environment.UserName;
			Console.Title = "Command Prompt";
			Console.WriteLine("Microsoft Windows [Version 10.0.16299.431]");
			Console.WriteLine("(c) 2017 Microsoft Corporation. All rights reserved.");
			Console.WriteLine("");
			Console.Write(text + ">");
			while (isCbOpen() || cmdCommand)
			{
				cmdCommand = false;
				string text2 = Console.ReadLine();
				if (text2.ToLower().Equals("cmd"))
				{
					Console.WriteLine("");
					Console.Write(text + ">");
					continue;
				}
				if (text2.ToLower().StartsWith("cd") && text2.Length > 3)
				{
					if (text2.Contains("force"))
					{
						startGui();
						return;
					}
					string text3 = text2.Substring(3);
					if (Directory.Exists(text3))
					{
						Console.WriteLine("");
						text = text3;
					}
					else
					{
						Console.WriteLine("The system cannot find the path specified.");
						Console.WriteLine("");
					}
					Console.Write(text + ">");
					continue;
				}
				Process process = new Process();
				ProcessStartInfo startInfo = process.StartInfo;
				startInfo.FileName = "cmd.exe";
				startInfo.Arguments = "/c cd " + text + " & " + text2;
				startInfo.RedirectStandardOutput = true;
				startInfo.RedirectStandardError = true;
				startInfo.UseShellExecute = false;
				process.Start();
				StringBuilder stringBuilder = new StringBuilder();
				while (!process.HasExited)
				{
					stringBuilder.Append(process.StandardOutput.ReadToEnd());
				}
				Console.WriteLine(stringBuilder.ToString());
				Console.Write(text + ">");
			}
			startGui();
		}

		public static void setMultNum()
		{
			double num = 1000.0 / ((max + min) / 2.0);
			double num2 = Math.Abs(num - listAvg) / 100.0;
			if (num > listAvg)
			{
				multNum = 1.0 + num2;
			}
			else if (num < listAvg)
			{
				multNum = 1.0 - num2;
			}
			else
			{
				multNum = 1.0;
			}
		}

		public Program()
		{
			loadDataClicks();
			setMultNum();
			new Thread((ThreadStart)delegate
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(defaultValue: false);
				Application.Run(a = new a());
			}).Start();
			ogHeight = Console.WindowHeight;
			ogWidth = Console.WindowWidth;
			if (isCbOpen())
			{
				spoofCmd();
			}
			else
			{
				startGui();
			}
		}

		public static void startGui()
		{
			ispvpclack = true;
			DeleteMenu(GetSystemMenu(GetConsoleWindow(), bRevert: false), 61440, 0);
			Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
			Console.ForegroundColor = ConsoleColor.Gray;
			printGUI();
		}

		public static void printGUI()
		{
			List<string> list = new List<string> { "sex with ur mom lol", "these bitches love sosa", "  ithrowls is ass", "    nigger haha" };
			int index = new Random().Next(list.Count);
			if (isCbOpen())
			{
				spoofCmd();
				return;
			}
			Console.Clear();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("\n");
			Console.WriteLine("     (o)--(o)");
			Console.WriteLine("    /.______.\\");
			Console.WriteLine("    \\________/   ");
			Console.Write(list[index] + "\n");
			Console.WriteLine("   ./        \\.");
			Console.WriteLine("  ( .        , )");
			Console.WriteLine("   \\ \\_\\\\//_/ /");
			Console.WriteLine("    ~~  ~~  ~~\n");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write("Status: ");
			Console.ForegroundColor = (isToggled ? ConsoleColor.Green : ConsoleColor.Red);
			Console.WriteLine((isToggled ? "On" : "Off") ?? "");
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine("Min CPS: " + min);
			Console.WriteLine("Max CPS: " + max);
			Console.WriteLine("\nToggle: Insert \nHide/Show: End");
			Console.WriteLine("");
			Console.WriteLine("Type 'help' for commands");
			handleCommand(Console.ReadLine());
		}

		public static void printHelp()
		{
			Console.WriteLine("1. cps <min> <max>");
			Console.WriteLine("2. destruct");
		}

		public static void handleCommand(string cmd)
		{
			cmd = cmd.ToLower();
			if (cmd.StartsWith("help"))
			{
				printHelp();
				Console.ReadKey();
			}
			else if (cmd.StartsWith("cps"))
			{
				try
				{
					min = double.Parse(cmd.Split(' ')[1]);
					max = double.Parse(cmd.Split(' ')[2]);
					setMultNum();
				}
				catch
				{
					Console.WriteLine("Not valid value(s)");
					Console.ReadKey();
				}
			}
			else if (!cmd.Contains("destruct"))
			{
				if (cmd.Contains("cmd"))
				{
					cmdCommand = true;
					spoofCmd();
				}
				else
				{
					Console.WriteLine("No such command!");
					Console.ReadKey();
				}
			}
			printGUI();
		}

		private static void Main(string[] args)
		{
			new Program();
		}
	}
}
