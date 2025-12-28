using Avalonia.Controls;

using DoomMapGuessr.Services;


namespace DoomMapGuessr.Views
{

	public partial class HomePage : UserControl
	{

		public HomePage()
		{

			InitializeComponent();

			if (ApplicationSettings.Shared.Cache!.SavedRelease?.Body is null)
			{

				this.FindControl<StackPanel>("GitHubReleaseBodyPanel")!.Children.Add(new TextBlock()
				{

					Text = "No release data.",
					FontSize = 24,
					FontWeight = Avalonia.Media.FontWeight.Bold

				});

				return;

			}

			this.FindControl<StackPanel>("GitHubReleaseBodyPanel")!
				.Children
				.AddRange(
                [
                    new TextBlock()
					{

						Text = ApplicationSettings.Shared.Cache.SavedRelease.Name,
						FontSize = 28,
						FontWeight = Avalonia.Media.FontWeight.Black,
						TextWrapping = Avalonia.Media.TextWrapping.Wrap

					},
					new TextBlock()
					{

						Text = ApplicationSettings.Shared.Cache.SavedRelease.Body,
						FontSize = 14,
						FontWeight = Avalonia.Media.FontWeight.Normal,
						TextWrapping = Avalonia.Media.TextWrapping.Wrap

					}
				]);

		}

}

}
