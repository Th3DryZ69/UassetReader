using System.Runtime.InteropServices;

namespace Uasset_Reader.Workspace.Other;

public static class Console
{
	private const int SW_HIDE = 0;

	private const int SW_SHOW = 5;

	[DllImport("kernel32.dll")]
	private static extern nint GetConsoleWindow();

	[DllImport("user32.dll")]
	private static extern bool ShowWindow(nint hWnd, int nCmdShow);

	public static void Show()
	{
		ShowWindow(GetConsoleWindow(), 5);
	}

	public static void Hide()
	{
		ShowWindow(GetConsoleWindow(), 0);
	}
}
