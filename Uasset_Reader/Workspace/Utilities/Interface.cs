using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Uasset_Reader.Workspace.Utilities;

public static class Interface
{
	public class ThicknessAnim
	{
		public dynamic Element { get; set; }

		public ThicknessAnimation ElementAnim { get; set; }
	}

	public class BaseAnim
	{
		public dynamic Element { get; set; }

		public DoubleAnimationBase ElementAnim { get; set; }

		public PropertyPath Property { get; set; }
	}

	public static Storyboard SetThicknessAnimations(params ThicknessAnim[] Elements)
	{
		Storyboard storyboard = new Storyboard();
		foreach (ThicknessAnim thicknessAnim in Elements)
		{
			ThicknessAnimation elementAnim = thicknessAnim.ElementAnim;
			elementAnim.SetValue(Storyboard.TargetProperty, thicknessAnim.Element);
			Storyboard.SetTargetProperty(elementAnim, new PropertyPath(FrameworkElement.MarginProperty));
			storyboard.Children.Add(elementAnim);
		}
		return storyboard;
	}

	public static Storyboard SetElementAnimations(params BaseAnim[] Elements)
	{
		Storyboard storyboard = new Storyboard();
		foreach (BaseAnim baseAnim in Elements)
		{
			DoubleAnimationBase elementAnim = baseAnim.ElementAnim;
			elementAnim.SetValue(Storyboard.TargetProperty, baseAnim.Element);
			Storyboard.SetTargetProperty(elementAnim, baseAnim.Property);
			storyboard.Children.Add(elementAnim);
		}
		return storyboard;
	}

	public static void SetBlur(params dynamic[] decorators)
	{
		foreach (dynamic val in decorators)
		{
			BlurEffect blurEffect = new BlurEffect
			{
				Radius = 10.0
			};
			val.Effect = blurEffect;
		}
	}
}
