using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Newtonsoft.Json.Linq;
using Serilog;
using Uasset_Reader.Workspace.Utilities;

namespace Uasset_Reader.Workspace.Usercontrols.Overlays;

public partial class SplashView : UserControl, IComponentConnector
{
	public SplashView()
	{
		InitializeComponent();
	}

	private void SplashView_Loaded(object sender, RoutedEventArgs e)
	{
		Log.Information("Initializing SplashView");
		JToken jToken = Endpoint.Download(Endpoint.Type.Versions);
		Log.Information("Setting GitHub");
		Global.GitHub = jToken["GitHub"].Value<string>();
		Log.Information("Checking version");
		if (jToken["2.02"]["Update"].Value<bool>())
		{
			Log.Warning("Detected a new update! this application is now unusable.");
			MessageBox.Show(Memory.MainView, "New version of uasset reader is out! Please update from: " + Global.GitHub, "Info", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			Global.GitHub.UrlStart();
			Environment.Exit(0);
		}
		Log.Information("Setting credits");
		Credits.Text = "Cracked by " + jToken["Discord"].Value<string>();
		Log.Information("Setting avatar");
		try
		{
			AvatarSource.LoadImage(jToken["Avatar"].Value<string>());
		}
		catch
		{
			Log.Warning("Failed to download avatar! Going to fallback.");
			AvatarSource.Source = new BitmapImage(new Uri("/Workspace/Assets/FallBack.png", UriKind.Relative));
		}
		Anim1();
	}

	private void Anim1()
	{
		Interface.SetElementAnimations(new Interface.BaseAnim
		{
			Element = Avatar,
			Property = new PropertyPath(UIElement.OpacityProperty),
			ElementAnim = new DoubleAnimation
			{
				From = 0.0,
				To = 1.0,
				Duration = new TimeSpan(0, 0, 0, 0, 100),
				BeginTime = new TimeSpan(0, 0, 0, 0, 600)
			}
		}).Begin();
		Storyboard storyboard = Interface.SetThicknessAnimations(new Interface.ThicknessAnim
		{
			Element = Avatar,
			ElementAnim = new ThicknessAnimation
			{
				To = new Thickness(389.0, 199.0, 389.0, 199.0),
				Duration = new TimeSpan(0, 0, 0, 0, 600),
				BeginTime = new TimeSpan(0, 0, 0, 0, 600)
			}
		});
		storyboard.Completed += delegate
		{
			Anim2();
		};
		storyboard.Begin();
	}

	private void Anim2()
	{
		Interface.SetElementAnimations(new Interface.BaseAnim
		{
			Element = UassetReader,
			Property = new PropertyPath(UIElement.OpacityProperty),
			ElementAnim = new DoubleAnimation
			{
				From = 0.0,
				To = 1.0,
				Duration = new TimeSpan(0, 0, 0, 0, 100),
				BeginTime = new TimeSpan(0, 0, 0, 0, 300)
			}
		}).Begin();
		Storyboard storyboard = Interface.SetThicknessAnimations(new Interface.ThicknessAnim
		{
			Element = Avatar,
			ElementAnim = new ThicknessAnimation
			{
				To = new Thickness(345.0, 199.0, 433.0, 199.0),
				Duration = new TimeSpan(0, 0, 0, 0, 600),
				BeginTime = new TimeSpan(0, 0, 0, 0, 300)
			}
		}, new Interface.ThicknessAnim
		{
			Element = UassetReader,
			ElementAnim = new ThicknessAnimation
			{
				To = new Thickness(439.0, 208.0, 0.0, 250.0),
				Duration = new TimeSpan(0, 0, 0, 0, 600),
				BeginTime = new TimeSpan(0, 0, 0, 0, 300)
			}
		});
		storyboard.Completed += delegate
		{
			Anim3();
		};
		storyboard.Begin();
	}

	private void Anim3()
	{
		Interface.SetElementAnimations(new Interface.BaseAnim
		{
			Element = UassetReader,
			Property = new PropertyPath(UIElement.OpacityProperty),
			ElementAnim = new DoubleAnimation
			{
				From = 1.0,
				To = 0.0,
				Duration = new TimeSpan(0, 0, 0, 0, 620),
				BeginTime = new TimeSpan(0, 0, 0, 0, 800)
			}
		}).Begin();
		Storyboard storyboard = Interface.SetThicknessAnimations(new Interface.ThicknessAnim
		{
			Element = UassetReader,
			ElementAnim = new ThicknessAnimation
			{
				To = new Thickness(769.0, 208.0, 0.0, 250.0),
				Duration = new TimeSpan(0, 0, 0, 0, 600),
				BeginTime = new TimeSpan(0, 0, 0, 0, 800)
			}
		});
		storyboard.Completed += delegate
		{
			Anim4();
		};
		storyboard.Begin();
	}

	private void Anim4()
	{
		Interface.SetElementAnimations(new Interface.BaseAnim
		{
			Element = Credits,
			Property = new PropertyPath(UIElement.OpacityProperty),
			ElementAnim = new DoubleAnimation
			{
				From = 0.0,
				To = 1.0,
				Duration = new TimeSpan(0, 0, 0, 0, 100),
				BeginTime = new TimeSpan(0, 0, 0, 0, 300)
			}
		}).Begin();
		Storyboard storyboard = Interface.SetThicknessAnimations(new Interface.ThicknessAnim
		{
			Element = Credits,
			ElementAnim = new ThicknessAnimation
			{
				To = new Thickness(439.0, 208.0, 0.0, 250.0),
				Duration = new TimeSpan(0, 0, 0, 0, 600),
				BeginTime = new TimeSpan(0, 0, 0, 0, 300)
			}
		});
		storyboard.Completed += delegate
		{
			Anim5();
		};
		storyboard.Begin();
	}

	private void Anim5()
	{
		Interface.SetElementAnimations(new Interface.BaseAnim
		{
			Element = Credits,
			Property = new PropertyPath(UIElement.OpacityProperty),
			ElementAnim = new DoubleAnimation
			{
				From = 1.0,
				To = 0.0,
				Duration = new TimeSpan(0, 0, 0, 0, 620),
				BeginTime = new TimeSpan(0, 0, 0, 0, 800)
			}
		}).Begin();
		Storyboard storyboard = Interface.SetThicknessAnimations(new Interface.ThicknessAnim
		{
			Element = Credits,
			ElementAnim = new ThicknessAnimation
			{
				To = new Thickness(769.0, 208.0, -283.0, 250.0),
				Duration = new TimeSpan(0, 0, 0, 0, 600),
				BeginTime = new TimeSpan(0, 0, 0, 0, 800)
			}
		});
		storyboard.Completed += delegate
		{
			Anim6();
		};
		storyboard.Begin();
	}

	private void Anim6()
	{
		Storyboard storyboard = Interface.SetThicknessAnimations(new Interface.ThicknessAnim
		{
			Element = Avatar,
			ElementAnim = new ThicknessAnimation
			{
				To = new Thickness(389.0, 199.0, 389.0, 199.0),
				Duration = new TimeSpan(0, 0, 0, 0, 600),
				BeginTime = new TimeSpan(0, 0, 0, 0, 600)
			}
		});
		storyboard.Completed += delegate
		{
			if (Settings.Read(Settings.Type.IsNew).Value<bool>() && Global.GitHub != null)
			{
				Global.GitHub.UrlStart();
				Settings.Edit(Settings.Type.IsNew, false);
			}
			Memory.MainView.RemoveOverLay();
		};
		storyboard.Begin();
	}
}
