using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace CMD
{
	public static class MouseHook
	{
		private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

		private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

		private struct POINT
		{
			public int x;

			public int y;
		}

		private struct MSLLHOOKSTRUCT
		{
			public POINT pt;

			public uint mouseData;

			public uint flags;

			public uint time;

			public IntPtr dwExtraInfo;
		}

		private enum MouseMessages
		{
			WM_MOUSEMOVE = 0x200,
			WM_LBUTTONDOWN = 513,
			WM_LBUTTONUP = 514,
			WM_RBUTTONDOWN = 516,
			WM_RBUTTONUP = 517,
			WM_MOUSEWHEEL = 522
		}

		private enum GetWindow_Cmd : uint
		{
			GW_HWNDFIRST,
			GW_HWNDLAST,
			GW_HWNDNEXT,
			GW_HWNDPREV,
			GW_OWNER,
			GW_CHILD,
			GW_ENABLEDPOPUP
		}

		private enum FakeMessage
		{
			WM_MOUSEMOVE = 0x200,
			WM_LBUTTONDOWN = 513,
			WM_LBUTTONUP = 514,
			WM_RBUTTONDOWN = 516,
			WM_RBUTTONUP = 517,
			WM_MOUSEWHEEL = 522
		}

		private static LowLevelMouseProc _proc = HookCallbackMouse;

		private static IntPtr _hookID = IntPtr.Zero;

		public static bool isLeftDown = false;

		public static ArrayList nigger = new ArrayList();

		private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private const int WH_MOUSE_LL = 14;

		public static void Start()
		{
			_hookID = SetMouseHook(_proc);
		}

		public static void stop()
		{
			UnhookWindowsHookEx(_hookID);
		}

		private static IntPtr SetMouseHook(LowLevelMouseProc proc)
		{
			using Process process = Process.GetCurrentProcess();
			using ProcessModule processModule = process.MainModule;
			return SetWindowsHookEx(14, proc, GetModuleHandle(processModule.ModuleName), 0u);
		}

		private static IntPtr HookCallbackMouse(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (!HandleMouseInput((MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT)), (MouseMessages)(int)wParam))
			{
				return new IntPtr(1);
			}
			return CallNextHookEx(_hookID, nCode, wParam, lParam);
		}

		public static IntPtr MakeLParam(int LoWord, int HiWord)
		{
			return (IntPtr)((HiWord << 16) | (LoWord & 0xFFFF));
		}

		public static IntPtr GetMinecraftHandle()
		{
			Process[] processesByName = Process.GetProcessesByName("javaw");
			foreach (Process process in processesByName)
			{
				if (process.MainWindowTitle.Contains("Minecraft"))
				{
					return process.MainWindowHandle;
				}
			}
			return IntPtr.Zero;
		}

		private static bool HandleMouseInput(MSLLHOOKSTRUCT strct, MouseMessages mm)
		{
			if (strct.flags != 0 || !Program.isToggled || !(a.minecraft != IntPtr.Zero))
			{
				return true;
			}
			switch (mm)
			{
			case MouseMessages.WM_LBUTTONDOWN:
				isLeftDown = true;
				PostMessage(a.minecraft, 513u, 0, 0);
				break;
			case MouseMessages.WM_LBUTTONUP:
				isLeftDown = false;
				PostMessage(a.minecraft, 514u, 0, 0);
				break;
			case MouseMessages.WM_RBUTTONDOWN:
				PostMessage(a.minecraft, 516u, 0, 0);
				break;
			case MouseMessages.WM_RBUTTONUP:
				PostMessage(a.minecraft, 517u, 0, 0);
				break;
			default:
				return true;
			}
			return false;
		}

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		public static long CurrentTimeMillis()
		{
			return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
		}
	}
}
