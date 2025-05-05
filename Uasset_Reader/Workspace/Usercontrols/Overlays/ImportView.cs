using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Galaxy_Swapper_v2.Workspace.CUE4Parse;
using Serilog;
using Uasset_Reader.Workspace.Swapping;
using Uasset_Reader.Workspace.Utilities;

namespace Uasset_Reader.Workspace.Usercontrols.Overlays;

public partial class ImportView : UserControl, IComponentConnector
{
	public ImportView()
	{
		InitializeComponent();
	}

	private void Close_Click(object sender, MouseButtonEventArgs e)
	{
		Memory.MainView.RemoveOverLay();
	}

	private void ImportObject_Click(object sender, RoutedEventArgs e)
	{
		if (ObjectBox.Text.Length == 0)
		{
			MessageBox.Show(Memory.MainView, "Object box is empty! Please paste the object path in the box above.", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}
		try
		{
			string text = ObjectBox.Text;
			if (!CProvider.Save(text))
			{
				Log.Error("Could not export (" + text + "). Wrong path?");
				MessageBox.Show(Memory.MainView, text + "\nFailed to export! Please ensure you pasted in the correct object path.", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
				return;
			}
			text = FormatObject(text);
			Memory.Asset = new Asset(text, CProvider.Export);
			Memory.MainView.RefreshObjects();
			Close_Click(Close, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(Memory.MainView, "Failed to import object! " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private string FormatObject(string Path)
	{
		return (Path.Contains('.') ? Path.Split('.').First() : Path).Replace("FortniteGame/Content/", "/Game/").Replace("FortniteGame/Plugins/GameFeatures/BRCosmetics/Content/", "/BRCosmetics/");
	}
}
