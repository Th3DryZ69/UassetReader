   M S B A M L   `   `   `  ���� H  DUasset Reader, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Uasset_ReaderUasset_Reader      Uasset_Reader.App   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -localclr-namespace:Uasset_Reader       �� ShutdownMode$  OnExplicitShutdown=� �� 	Resources    ScrollThumbs �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property	 P� Value
 �� 
TargetName � RowDefinitions ]� Thumb ]� IncreaseRepeatButton J� Opacity ]� DecreaseRepeatButton �� Angle 	 Close �� Triggers  Control �� BasedOn � ShowGridLines � ColumnDefinitions K� ZIndex �� Stretch �� RadiusX �� RadiusY � Cursor J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   ShadowDepth   Opacity  SettingBorder�� %6  &	        '�� t    &	 	    &	 0    'd� U    7d�'�� 
    &	 +    '_�     �� " n�.���  �@�Cursor$
 Hand����  � �� �� " n�.R�� $
��Grid��.+��� $��Stretch=�$��Stretch=�$
��Auto��$
��Auto�����	#00FFFFFF�� $��
Rectangle1��$ 5��$��Stretch=�$��Stretch=�$
��Auto��$
��Auto��#��z�� P�  �@�Tag$	 
Horizontal��. ��� $
 
Rectangle1�� � ��$
 Auto���� $
 
Rectangle1�� � ��$ 7���� " ��.���  �@��IsFlicksEnabled$ false����  � �� �#8C8C8C��  � �� �	#00FFFFFF��  � ��$ 8����  � �� �� " ��.R�� $��GridRoot��$��8��#��z�� �� $��0.00001* �.+�]� $��
PART_Track��$��0��$
��true��$|�false�� n� $��Thumb��#��z��#���    �� $��PageUp��$��ScrollBar.PageDownCommand��$ 0Q�$|�false�� �� $��PageDown��$��ScrollBar.PageUpCommand��$ 0Q�$|�false�� P�  � {�$	 false��. ��� $
 Thumb�� � y�$ 	Collapsed=�P�  �@��Orientation$	 
Horizontal=�. ��� $
 GridRoot�� �@�LayoutTransform �� $	 -90Q��� $
 
PART_Track�� �@�LayoutTransform �� $	 -90Q���  � ��$
 Auto����  � ��$ 8���� $
 Thumb�� �@�Tag$ 
Horizontal���� $
 PageDown�� � ��$ ScrollBar.PageLeftCommand���� $
 PageUp�� � ��$  ScrollBar.PageRightCommand���� " ��.���  � �� �	#00FFFFFF��  �@��CornerRadius$ 2����  �@�Cursor$
 Hand����  � ��$ 30����  � ��$ 40�� P�  �@J�IsMouseOver$
	 True��. ���  � �� �	#FFFF0000P�  �@J�IsMouseOver$	 False��. ���  � �� �	#00FFFFFF�� " ��.���  � �� �	#00FFFFFF��  �@��CornerRadius$ 2����  �@�Cursor$
 Hand����  � ��$ 30����  � ��$ 40�� P�  �@J�IsMouseOver$
	 True��. ���  � �� �#0D1118P�  �@J�IsMouseOver$	 False��. ���  � �� �	#00FFFFFF�� 8   " d�.���  �@�Cursor$
 Hand����  � ��$	 100����  � �� �� " d�.R�(� #��z��.��� $��96��.+�� $��96��$��Center=�$ False�� �� $��1* ��� $��1* �.+�F� $��0��$��18��$��Center=�$��Center=�$ 2��$ Uniform=��� $��
BackGround��$��0��$��2��$��96��$��52��$��Stretch=�$��Center=�$ 0��#��z��$ 26��$ 26��F� $��1��$��21��$��0,0,2,3q�$��Center=�$��Center=�$ 2��$ Uniform=�A� $
��Pill��$��50��$��50��$��1,0,1,0q�$
��Left=�$��Center=�$ 1��#��z��$y�Visible=� P�  �@d�	IsChecked$
	 TrueY�. ��� $

 Pill�� � ��$ Right=��� $
 
BackGround�� � �� �#0091E6�� " ��.���  � ��$ Center=���  � ��$ Center=���  � ��$ 1q���  � �� �#D3D3D6��  � �� �� " ��.R�� $
��grid��.+��� $��border��$ 8�����#050608$��1q����#0B0F16$
 Hand��   $ 3Q�$	 0.2Q�._��� $��Center=�$��Center=�$��SemiBold"� P�  � ��$
	 True��. ��� $
 border�� � �� �#0091E6P�  �@J�IsMouseOver$
	 True��. ��� $
 border�� � �� �#454B56P�  � {�$	 False��. ��� $

 grid�� �@J�Opacity$
 0.25Q��� " ��.���  � �� �#0B0F16��  � �� �#050608��  � ��$ 1q���  �@��CornerRadius$ 10����  � ��$ 70����  � ��$	 820����  � ��$ 10q���  �@J�Effect   $ 3Q�$	 0.2Q��� " _�.���  � �� �#0B0F16��  � �� �#050608��  � ��$ 1q���  � �� �#D3D3D6��  �@^�	Placement$ Right=���  �@J�Effect   $ 3Q�$	 0.2Q�