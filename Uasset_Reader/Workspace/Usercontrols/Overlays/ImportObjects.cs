using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Galaxy_Swapper_v2.Workspace.CUE4Parse;
using Serilog;
using Uasset_Reader.Workspace.Utilities;

namespace Uasset_Reader.Workspace.Usercontrols.Overlays;

public partial class ImportObjects : UserControl, IComponentConnector
{
	public ImportObjects()
	{
		InitializeComponent();
	}

	private void Close_Click(object sender, MouseButtonEventArgs e)
	{
		Memory.MainView.RemoveOverLay();
	}

	private void ImportObject_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			string text = ObjectBox.Text;
			if (!CProvider.Save(text))
			{
				Log.Error("Could not export (" + text + "). Wrong path?");
				MessageBox.Show(text + "\nFailed to export! Please ensure you pasted in the correct object path.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
			text = FormatObject(text);
			Memory.Asset.ImportObjects(text, CProvider.Export);
			Memory.MainView.RefreshObjects();
			Close_Click(Close, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(Memory.MainView, "Failed to import object strings! " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private string FormatObject(string Path)
	{
		return (Path.Contains('.') ? Path.Split('.').First() : Path).Replace("FortniteGame/Content/", "/Game/").Replace("FortniteGame/Plugins/GameFeatures/BRCosmetics/Content/", "/BRCosmetics/");
	}
}
