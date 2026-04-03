using Avalonia.Controls;


namespace DoomMapGuessr.Views
{

	public partial class HomePage : UserControl
	{

		public HomePage() => InitializeComponent();

		private void UserControl_SizeChanged(object? sender, SizeChangedEventArgs e)
		{

			if (e is null)
				return;

			if (e.NewSize.Width < 1000)
			{

				DashboardControl.IsEnabled = false;
				DashboardControl.IsVisible = false;

				NoSpaceControl.IsEnabled = true;
				NoSpaceControl.IsVisible = true;

				return;

			}

			DashboardControl.IsEnabled = true;
			DashboardControl.IsVisible = true;

			NoSpaceControl.IsEnabled = false;
			NoSpaceControl.IsVisible = false;

		}

	}

}
