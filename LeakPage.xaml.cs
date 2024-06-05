namespace DrawingViewLeak;

public partial class LeakPage : ContentPage
{
	public LeakPage()
	{
		InitializeComponent();
	}
	public object? GetPlatformView()
	{
		return this.drawingView?.Handler?.PlatformView;
	}
}