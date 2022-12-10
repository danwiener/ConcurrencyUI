namespace ConcurrencyUI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		InitializeComponent();
		var width = 1920;
		var height = 1200;
		Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
		{
#if WINDOWS
			var nativeWindow = handler.PlatformView;
			nativeWindow.Activate();
			IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
			Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
			Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
			//TODO: this is hardcoded stuff for fullhd
			appWindow.MoveAndResize(new Windows.Graphics.RectInt32((1920 / 2) - width / 2, (1200 / 2) - height / 2, width, height));
#endif
		});

		MainPage = new AppShell();
	}
}
