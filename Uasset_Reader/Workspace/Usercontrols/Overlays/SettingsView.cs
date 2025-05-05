using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using Newtonsoft.Json.Linq;
using Serilog;
using Uasset_Reader.Workspace.Other;
using Uasset_Reader.Workspace.Utilities;

namespace Uasset_Reader.Workspace.Usercontrols.Overlays;

public partial class SettingsView : System.Windows.Controls.UserControl, IComponentConnector
{
	public SettingsView()
	{
		InitializeComponent();
	}

	private void Close_Click(object sender, MouseButtonEventArgs e)
	{
		if (Memory.Asset == null)
		{
			Uasset_Reader.Workspace.Utilities.Presence.Update("Viewing: Dashboard");
		}
		else
		{
			Uasset_Reader.Workspace.Utilities.Presence.Update("Viewing: " + Path.GetFileNameWithoutExtension(Memory.Asset.ObjectPath));
		}
		Memory.MainView.RemoveOverLay();
	}

	private void SettingsView_Loaded(object sender, RoutedEventArgs e)
	{
		Installation.Text = Settings.Read(Settings.Type.Installtion).Value<string>();
		if (Settings.Read(Settings.Type.UtocModification).Value<bool>())
		{
			UtocModification.IsChecked = true;
		}
		if (Settings.Read(Settings.Type.Console).Value<bool>())
		{
			ShowConsole.IsChecked = true;
		}
		if (Settings.Read(Settings.Type.FormatSwap).Value<bool>())
		{
			FormatHexSwap.IsChecked = true;
		}
		if (Settings.Read(Settings.Type.Presence).Value<bool>())
		{
			Presence.IsChecked = true;
		}
		Uasset_Reader.Workspace.Utilities.Presence.Update("Viewing: Settings");
	}

	private void UtocModification_Click(object sender, RoutedEventArgs e)
	{
		Settings.Edit(Settings.Type.UtocModification, UtocModification.IsChecked.ToString());
	}

	private void FormatHexSwap_Click(object sender, RoutedEventArgs e)
	{
		Settings.Edit(Settings.Type.FormatSwap, FormatHexSwap.IsChecked.ToString());
	}

	private void Presence_Click(object sender, RoutedEventArgs e)
	{
		Settings.Edit(Settings.Type.Presence, Presence.IsChecked.ToString());
		if (Presence.IsChecked == true)
		{
			Uasset_Reader.Workspace.Utilities.Presence.Initialize();
			Uasset_Reader.Workspace.Utilities.Presence.Update("Viewing: Settings");
		}
		else
		{
			Uasset_Reader.Workspace.Utilities.Presence.Client.ClearPresence();
		}
	}

	private void ShowConsole_Click(object sender, RoutedEventArgs e)
	{
		if (ShowConsole.IsChecked == true)
		{
			Console.Show();
		}
		else
		{
			Console.Hide();
		}
		Settings.Edit(Settings.Type.Console, ShowConsole.IsChecked.ToString());
	}

	private void EditPath_Click(object sender, RoutedEventArgs e)
	{
		using FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
		{
			Description = "Please select the 'FortniteGame' folder!",
			UseDescriptionForTitle = true
		};
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			string text = folderBrowserDialog.SelectedPath;
			if (text.EndsWith("\\FortniteGame\\Content\\Paks"))
			{
				text = text.Replace("\\FortniteGame\\Content\\Paks", string.Empty);
			}
			else if (text.EndsWith("\\FortniteGame\\Content"))
			{
				text = text.Replace("\\FortniteGame\\Content", string.Empty);
			}
			else if (text.EndsWith("\\FortniteGame"))
			{
				text = text.Replace("\\FortniteGame", string.Empty);
			}
			else if (!text.EndsWith("\\Fortnite"))
			{
				Log.Error(text + "\nDoes not contain 'FortniteGame'");
				System.Windows.MessageBox.Show(Memory.MainView, text + " Does not contain 'FortniteGame'!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				return;
			}
			Settings.Edit(Settings.Type.Installtion, text);
			Installation.Text = text;
		}
	}
}
