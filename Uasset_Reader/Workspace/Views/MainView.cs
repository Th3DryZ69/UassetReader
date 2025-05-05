using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using Galaxy_Swapper_v2.Workspace.CUE4Parse;
using Newtonsoft.Json.Linq;
using Serilog;
using Uasset_Reader.Workspace.Usercontrols.Overlays;
using Uasset_Reader.Workspace.Utilities;

namespace Uasset_Reader.Workspace.Views;

public partial class MainView : Window, IComponentConnector
{
	private UserControl LastOverLay { get; set; }

	public MainView()
	{
		InitializeComponent();
		Memory.MainView = this;
		Presence.Update("Viewing: Dashboard");
	}

	public void SetOverLay(UserControl UserControl)
	{
		if (LastOverLay != null)
		{
			Base.Children.Remove(LastOverLay);
		}
		Main.IsEnabled = false;
		ObjectList.Visibility = Visibility.Hidden;
		LastOverLay = UserControl;
		Interface.SetBlur(Main);
		Base.Children.Add(UserControl);
		Interface.SetElementAnimations(new Interface.BaseAnim
		{
			Element = UserControl,
			Property = new PropertyPath(UIElement.OpacityProperty),
			ElementAnim = new DoubleAnimation
			{
				From = 0.0,
				To = 1.0,
				Duration = new TimeSpan(0, 0, 0, 0, 400)
			}
		}).Begin();
	}

	public void RemoveOverLay()
	{
		Main.Effect = null;
		Storyboard storyboard = Interface.SetElementAnimations(new Interface.BaseAnim
		{
			Element = LastOverLay,
			Property = new PropertyPath(UIElement.OpacityProperty),
			ElementAnim = new DoubleAnimation
			{
				From = 1.0,
				To = 0.0,
				Duration = new TimeSpan(0, 0, 0, 0, 400)
			}
		});
		storyboard.Completed += delegate
		{
			Main.IsEnabled = true;
			ObjectList.Visibility = Visibility.Visible;
			Base.Children.Remove(LastOverLay);
			LastOverLay = null;
		};
		storyboard.Begin();
	}

	public void RefreshObjects()
	{
		ObjectList.Items.Clear();
		foreach (string Object in Memory.Asset.Read())
		{
			ListBoxItem listBoxItem = new ListBoxItem
			{
				Content = Object,
				Cursor = Cursors.Hand
			};
			listBoxItem.MouseDoubleClick += delegate
			{
				SearchBox.Text = Object;
			};
			listBoxItem.KeyDown += delegate(object sender, KeyEventArgs e)
			{
				if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.C)
				{
					Clipboard.SetText(Object);
				}
				else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.I)
				{
					string text = SearchBox.Text;
					string text2 = ReplaceBox.Text;
					SearchBox.Text = Object;
					ReplaceBox.Text = "/";
					ReplaceObject_Click(null, null);
					SearchBox.Text = text;
					ReplaceBox.Text = text2;
				}
			};
			ObjectList.Items.Add(listBoxItem);
		}
	}

	private void ImportObject_Click(object sender, RoutedEventArgs e)
	{
		SetOverLay(new ImportView());
	}

	private void ImportObjects_Click(object sender, RoutedEventArgs e)
	{
		if (Memory.Asset != null)
		{
			SetOverLay(new ImportObjects());
		}
	}

	private void ReplaceHex_Click(object sender, RoutedEventArgs e)
	{
		if (Memory.Asset == null)
		{
			return;
		}
		try
		{
			Memory.Asset.ReplaceHex(SearchBox.Text, ReplaceBox.Text);
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private void ReplaceObject_Click(object sender, RoutedEventArgs e)
	{
		if (Memory.Asset == null)
		{
			return;
		}
		try
		{
			string text = SearchBox.Text;
			string text2 = ReplaceBox.Text;
			if (text.Contains('.') && text.CountChars('.') == 1)
			{
				if (text2.Contains('.') && text.CountChars('.') == 1)
				{
					Memory.Asset.ReplaceNameMap(text.Split('.').First(), text2.Split('.').First());
					Memory.Asset.ReplaceNameMap(text.Split('.').Last(), text2.Split('.').Last());
				}
				else
				{
					Memory.Asset.ReplaceNameMap(text.Split('.').First(), text2);
					Memory.Asset.ReplaceNameMap(text.Split('.').Last(), " ");
				}
			}
			else
			{
				Memory.Asset.ReplaceNameMap(text, text2);
			}
			RefreshObjects();
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private void SaveAsset_Click(object sender, RoutedEventArgs e)
	{
		try
		{
			if (Memory.Asset == null || !Memory.Asset.SaveAsset(out byte[] Uncompressed, out byte[] Compressed))
			{
				return;
			}
			string text = Settings.Path + "\\Exports\\" + Path.GetDirectoryName(Memory.Asset.ObjectPath);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Memory.Asset.ObjectPath);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			File.WriteAllBytes(text + "\\" + fileNameWithoutExtension + ".uasset", Uncompressed);
			File.WriteAllBytes(text + "\\" + fileNameWithoutExtension + "_Compressed.uasset", Compressed);
			CProvider.ExportData export = Memory.Asset.Export;
			string ucas = Memory.Asset.Export.Ucas + ".ucas";
			string utoc = Memory.Asset.Export.Utoc + ".utoc";
			JObject jObject = JObject.FromObject(new
			{
				Ucas = ucas,
				Utoc = utoc,
				Offset = export.Offset,
				Search = export.CompressedBuffer.Take(Compressed.Length).ToArray().ByteToHex()
			});
			if (Settings.Read(Settings.Type.UtocModification).Value<bool>())
			{
				byte[] array = Miscellaneous.CompressionBlock(export.CompressionBlock.Buffer, (uint)Compressed.Length, (uint)Uncompressed.Length, IsCompressed: true);
				byte[] array2 = Miscellaneous.ChunkOffsetLengths(export.ChunkOffsetLengths.Buffer, Uncompressed.Length);
				JObject value = JObject.FromObject(new
				{
					Offset = export.CompressionBlock.Position,
					Search = export.CompressionBlock.Buffer.ByteToHex(),
					Replace = array.ByteToHex()
				});
				jObject.Add("CompressionBlock", value);
				JObject value2 = JObject.FromObject(new
				{
					Offset = export.ChunkOffsetLengths.Position,
					Search = export.ChunkOffsetLengths.Buffer.ByteToHex(),
					Replace = array2.ByteToHex()
				});
				jObject.Add("ChunkOffsetLengths", value2);
				if (Settings.Read(Settings.Type.FormatSwap).Value<bool>())
				{
					string contents = Miscellaneous.FormatHexSwap(ucas, utoc, export.CompressedBuffer.Take(Compressed.Length).ToArray(), Compressed, export.CompressionBlock.Buffer, array, export.ChunkOffsetLengths.Buffer, array2);
					File.WriteAllText(text + "\\" + fileNameWithoutExtension + ".txt", contents);
				}
			}
			else if (Settings.Read(Settings.Type.FormatSwap).Value<bool>())
			{
				string contents2 = Miscellaneous.FormatHexSwap(ucas, utoc, export.CompressedBuffer.Take(Compressed.Length).ToArray(), Compressed, null, null, null, null);
				File.WriteAllText(text + "\\" + fileNameWithoutExtension + ".txt", contents2);
			}
			File.WriteAllText(text + "\\" + fileNameWithoutExtension + ".json", jObject.ToString());
			text.UrlStart();
		}
		catch (Exception value3)
		{
			MessageBox.Show(this, $"Failed to save asset! {value3}", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private void Settings_Click(object sender, RoutedEventArgs e)
	{
		SetOverLay(new SettingsView());
	}

	private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		SearchLength.Text = SearchBox.Text.Length.ToString();
	}

	private void ReplaceBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		ReplaceLength.Text = ReplaceBox.Text.Length.ToString();
	}

	private void SearchBox_KeyDown(object sender, KeyEventArgs e)
	{
		if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.F)
		{
			string text = SearchBox.Text;
			if (text.StartsWith("FortniteGame/Content/"))
			{
				text = text.Replace("FortniteGame/Content/", "/Game/");
			}
			if (text.EndsWith(".uasset"))
			{
				text = text.Replace(".uasset", string.Empty);
			}
			if (text.EndsWith(".ubulk"))
			{
				text = text.Replace(".ubulk", string.Empty);
			}
			Log.Information("Formated from " + SearchBox.Text + " To " + text);
			SearchBox.Text = text;
		}
		else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.D)
		{
			string text2 = SearchBox.Text + "." + Path.GetFileNameWithoutExtension(SearchBox.Text);
			Log.Information("Formated from " + SearchBox.Text + " To " + text2);
			SearchBox.Text = text2;
		}
	}

	private void ReplaceBox_KeyDown(object sender, KeyEventArgs e)
	{
		if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.F)
		{
			string text = ReplaceBox.Text;
			if (text.StartsWith("FortniteGame/Content/"))
			{
				text = text.Replace("FortniteGame/Content/", "/Game/");
			}
			if (text.EndsWith(".uasset"))
			{
				text = text.Replace(".uasset", string.Empty);
			}
			if (text.EndsWith(".ubulk"))
			{
				text = text.Replace(".ubulk", string.Empty);
			}
			Log.Information("Formated from " + ReplaceBox.Text + " To " + text);
			ReplaceBox.Text = text;
		}
		else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.D)
		{
			string text2 = ReplaceBox.Text + "." + Path.GetFileNameWithoutExtension(ReplaceBox.Text);
			Log.Information("Formated from " + ReplaceBox.Text + " To " + text2);
			ReplaceBox.Text = text2;
		}
	}

	private void MainView_Loaded(object sender, RoutedEventArgs e)
	{
		// SetOverLay(new SplashView());
	}
}
