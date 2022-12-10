
using Newtonsoft.Json.Linq;
using System.Text;

namespace ConcurrencyUI;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}


	protected override void OnAppearing()
	{
		TitleLabel.ScaleTo(3, 1000);
	}

	private async void OnSumBtnClicked(object sender, EventArgs e)
	{
		try
		{
			double conc = Convert.ToDouble(ConcurrentEntry.Text);
			double synch = Convert.ToDouble(SynchronousEntry.Text);

			await RunSynchRunAsynch(conc, synch);
		}
		catch (FormatException ex)
		{
			await DisplayAlert("Invalid entry", "Please enter a valid number to sum to", "Ok");
		}
	}

	public async Task RunSynchRunAsynch(double conc, double synch)
	{
		var url = "http://localhost:8000/api/concurrent"; // access the login endpoint to receive access token from authorization server
		using var client = new HttpClient();

		client.DefaultRequestHeaders.Add("concurrency", $"{conc}"); // add number to sum to 

		var response = await client.GetAsync(url);
		var result = await response.Content.ReadAsStringAsync();
		TimeSpan concurrentTimeElapsed = TimeSpan.Parse(JObject.Parse(result)["concurrencytime"].ToString());
		double concurrentSum = Convert.ToDouble(JObject.Parse(result)["concurrencysum"].ToString());
		ConcurrentEntry.Text = concurrentSum.ToString();
		ConcurrentTime.Text = concurrentTimeElapsed.ToString();

		client.DefaultRequestHeaders.Remove("concurrency");
		client.DefaultRequestHeaders.Add("synchronous", $"{synch}"); // add number to sum to 


		var newUrl = "http://localhost:8000/api/synchronous"; // get request to user endpoint to receive protected user information and log user in

		var response2 = await client.GetAsync(newUrl);

		var result2 = await response2.Content.ReadAsStringAsync();
		TimeSpan synchtTimeElapsed = TimeSpan.Parse(JObject.Parse(result2)["synchtime"].ToString());
		double synchSum = Convert.ToDouble(JObject.Parse(result2)["synchsum"].ToString());

		SynchronousEntry.Text = synchSum.ToString();
		SynchronousTime.Text = synchtTimeElapsed.ToString();

	} // End method
}

