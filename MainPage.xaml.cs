namespace DrawingViewLeak;

public partial class MainPage : ContentPage
{
	WeakReference<Page>? pageReference;
	WeakReference<object>? pViewReference;
    public MainPage()
    {
        InitializeComponent();
    }

    private async void ShowPageWithLeak(object sender, EventArgs e)
    {
		var leakPage = new LeakPage();
		checkButton.IsEnabled = false;
		pageReference = new WeakReference<Page>(leakPage);
		await Navigation.PushAsync(leakPage);
		checkButton.IsEnabled = true;
		var pView = leakPage.GetPlatformView();
		pViewReference = null;
		if (pView != null)
			pViewReference = new WeakReference<object>(pView);
    }
	void CheckLeak(object sender, EventArgs e)
	{
		GC.Collect();
		GC.WaitForPendingFinalizers();
		GC.Collect();
		string pViewStatusMessage = "can't detect PlatformView";
		if (pViewReference != null) {
			if (pViewReference.TryGetTarget(out var pView))
			{
				pViewStatusMessage = $"{pView.GetType().Name} is still alive";
			} else {
				pViewStatusMessage = $"PlatformView is gone";
			}
		}
		if (pageReference?.TryGetTarget(out var page) ?? false)
		{
			DisplayAlert("Leak", $"LeakPage is still alive ({pViewStatusMessage})", "OK");
		}
		else
		{
			DisplayAlert("No Leak", $"LeakPage is gone ({pViewStatusMessage})", "OK");
		}
	}
}