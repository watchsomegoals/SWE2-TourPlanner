   M S B A M L   `   `   `  ����J  FSWE2TourPlanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:SWE2TourPlanner.ViewModelsSWE2TourPlanner.ViewModels  5   	   '    !SWE2TourPlanner.View.LogAddWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ?
viewmodels(clr-namespace:SWE2TourPlanner.ViewModels     5   	     � Title$  LogAddWindow��5   	   $	��370��6   $	��800��6+    � 	Resources 5	         DefaultTextBoxStyle �� 
TargetType �� Property �� Value " CustomErrorControlOnErrorStyle �� BasedOn ��Name �� ElementName �� Path��%�  &	        &	 u     7   �� 5
   
   " ��6,   .�5      ��  � ��$ 0,5,0,5q�6'   �� 5       � ��$ Center=�62   5      �� 5   
   " ��67   8   .�5      ��  �@=�ErrorTemplate 5      �� 5      .R�5      �� .+�5      �� $ placeholder��68   �� 5      $��11(�6(   $��Italic$�66   
���  ����5   (   �� $ placeholder��$5 /AdornedElement.(Validation.Errors)/ErrorContent�5      5      5      5      5      .��5      � 	 � ColumnDefinitions	 5   
   �� 5      $��1* �6   �� 5       $��2* �6   �� 5!      $��2* �6   �� 5"      $��1* �6   
 � RowDefinitions
 5$   
   �� 5%      $��5* �6   �� 5&      $��1* �6   .+�5)   
   �� $��1��6   $��0,0,15,0q�6%   .+�5*      �� $��Enter Date/Time:��6   �� 5+      #��[ 5,      ��5+      �� *
DateTime+ �� UpdateSourceTrigger$ PropertyChanged=�" �� ValidatesOnNotifyDataErrors$
 True��5,      �� 5.      $��Enter Report:��6   �� 5/      #��[ 50      ��5/      �� *Report+$ PropertyChanged=�$
 True��50      �� 52      $��Enter Distance (in km):��6   �� 53      #��[ 54      ��53      �� *
Distance+$ PropertyChanged=�$
 True��54      �� 56      $��Enter Total Time:��6   �� 57      #��[ 58      ��57      �� *	TotalTime+$ PropertyChanged=�$
 True��58      �� 5:      $��Enter Rating:��6   �� 5;      #��[ 5<      ��5;      �� *Rating+$ PropertyChanged=�$
 True��5<      5=      �� 5?   
   $��2��6   $��15,0,0,0q�6%   .+�5@      �� $$��Enter Average Speed (in km/h):��6   �� 5A      #��[ 5B      ��5A      �� *
AvgSpeed+$ PropertyChanged=�$
 True��5B      �� 5D      $%��Enter Inclination (in degrees):��6   �� 5E      #��[ 5F      ��5E      �� *Inclination+$ PropertyChanged=�$
 True��5F      �� 5H      $ ��Enter Top Speed (in km/h):��6   �� 5I      #��[ 5J      ��5I      �� *
TopSpeed+$ PropertyChanged=�$
 True��5J      �� 5L      $#��Enter Max Height (in metres):��6   �� 5M      #��[ 5N      ��5M      �� *	MaxHeight+$ PropertyChanged=�$
 True��5N      �� 5P      $#��Enter Min Height (in metres):��6   �� 5Q      #��[ 5R      ��5Q      �� *	MinHeight+$ PropertyChanged=�$
 True��5R      5S      � 5U   
   $��1��6   $��1��6   $��2��6,   	 5V      �� 5W      $��3* �6#   �� 5X      $��1* �6#   �� 5Y      $��3* �6#   .+�5\      �� $��1��6   $	��Add��5]      $��	0,15,0,15q�5_   D   ��5]   #   �� *AddLogCommand+��5^      ��  �� RelativeSource ��  �� AncestorType" �5_   D   5`      5c      5d      