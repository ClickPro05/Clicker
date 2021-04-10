using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace CMD
{
	public class a : Form
	{
		private enum KeyModifier
		{
			None = 0,
			Alt = 1,
			Control = 2,
			Shift = 4,
			WinKey = 8
		}

		public static IntPtr minecraft = IntPtr.Zero;

		public Random rand = new Random();

		public int current = 2;

		private const int SW_HIDE = 0;

		private const int SW_SHOW = 5;

		private const int WM_MOUSEWHEEL = 522;

		private const int WM_MOUSEMOVE = 512;

		private const int WM_LBUTTONDOWN = 513;

		private const int WM_LBUTTONUP = 514;

		private const int WM_RBUTTONDOWN = 516;

		private const int WM_RBUTTONUP = 517;

		public bool wasDown;

		private IContainer components;

		private System.Windows.Forms.Timer timer;

		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		[DllImport("user32.dll")]
		public static extern bool RegisterHotKey(IntPtr windowHandle, int hotkeyId, uint modifierKeys, uint virtualKey);

		[DllImport("user32.dll")]
		public static extern bool UnregisterHotKey(IntPtr windowHandle, int hotkeyId);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		public static IntPtr GetMinecraftHandle()
		{
			Process[] processesByName = Process.GetProcessesByName("javaw");
			foreach (Process process in processesByName)
			{
				if (process.MainWindowTitle.Contains("Minecraft") || process.MainWindowTitle.Contains("Client"))
				{
					return process.MainWindowHandle;
				}
			}
			return IntPtr.Zero;
		}

		public a()
		{
			InitializeComponent();
			int num = 0;
			RegisterHotKey(base.Handle, num, 0u, 45u);
			RegisterHotKey(base.Handle, num + 1, 0u, 35u);
			timer.Start();
			MouseHook.Start();
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			minecraft = GetMinecraftHandle();
			if (minecraft == IntPtr.Zero)
			{
				return;
			}
			if (Program.isToggled && MouseHook.isLeftDown)
			{
				if (current >= Program.list.Count - 2)
				{
					current = 0;
				}
				else
				{
					current++;
				}
				string[] array = (string[])Program.list[current];
				int down = Convert.ToInt32((double)int.Parse(array[0]) * Program.multNum);
				timer.Interval = Convert.ToInt32((double)int.Parse(array[1]) * Program.multNum) + down;
				new Thread((ThreadStart)delegate
				{
					sendClick(down);
				}).Start();
				wasDown = true;
			}
			else if (wasDown)
			{
				timer.Interval = 40;
				wasDown = false;
				PostMessage(minecraft, 514u, 0, 0);
			}
		}

		public void sendClick(int down)
		{
			PostMessage(minecraft, 514u, 0, 0);
			Thread.Sleep(down);
			if (MouseHook.isLeftDown)
			{
				PostMessage(minecraft, 513u, 0, 0);
			}
		}

		private void a_Paint(object sender, PaintEventArgs e)
		{
			Hide();
		}

		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			if (m.Msg != 786)
			{
				return;
			}
			int num = ((int)m.LParam >> 16) & 0xFFFF;
			_ = (int)m.LParam;
			m.WParam.ToInt32();
			int num2 = 45;
			if (num == num2)
			{
				Program.isToggled = !Program.isToggled;
				if (Program.ispvpclack)
				{
					new Thread((ThreadStart)delegate
					{
						Program.printGUI();
					}).Start();
				}
			}
			else
			{
				IntPtr consoleWindow = GetConsoleWindow();
				if (Program.isHidden)
				{
					ShowWindow(consoleWindow, 5);
				}
				else
				{
					ShowWindow(consoleWindow, 0);
				}
				Program.isHidden = !Program.isHidden;
			}
		}

		private void a_Load(object sender, EventArgs e)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			timer = new System.Windows.Forms.Timer(components);
			SuspendLayout();
			timer.Enabled = true;
			timer.Interval = 40;
			timer.Tick += new System.EventHandler(timer_Tick);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(120, 0);
			base.Name = "a";
			base.ShowIcon = false;
			Text = "a";
			base.Load += new System.EventHandler(a_Load);
			base.Paint += new System.Windows.Forms.PaintEventHandler(a_Paint);
			ResumeLayout(false);
		}
	}
}
