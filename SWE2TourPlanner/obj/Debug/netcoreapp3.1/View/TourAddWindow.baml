   M S B A M L   `   `   `  ����J  FSWE2TourPlanner, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:SWE2TourPlanner.ViewModelsSWE2TourPlanner.ViewModels  5   	   (    "SWE2TourPlanner.View.TourAddWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ?
viewmodels(clr-namespace:SWE2TourPlanner.ViewModels     5   	     � Title$  TourAddWindow��5   	   $	��440��6   $	��600��6,    � 	Resources 5	         DefaultTextBoxStyle �� 
TargetType �� Property �� Value  DefaultComboBoxStyle " CustomErrorControlOnErrorStyle �� BasedOn ��Name �� ElementName �� Path * &CustomErrorControlOnErrorStyleComboBox��%�  &	        &	 u     &	 �     7   &	 9    7  �� 5
   
   " ��6,   .�5      ��  � ��$ 0,5,0,5q�6'   �� 5       � ��$ Center=�62   5      �� 5   
   " ��6-   .�5      ��  � ��$ 0,5,0,5q�6'   �� 5       � ��$ Center=�62   5      �� 5   
   " ��67   8   .�5      ��  �@=�ErrorTemplate 5      �� 5      .R�5      �� .+�5      �� $ placeholder��68   �� 5      $��11(�6(   $��Italic$�66   
���  ����5   (   �� $ placeholder��$5 /AdornedElement.(Validation.Errors)/ErrorContent�5      5      5      5      �� 5   
   " ��6?   8   .�5       ��  �@=�ErrorTemplate 5!      �� 5"      .R�5#      �� .+�5$      �� $ placeholder��68   �� 5%      $��11(�6(   $��Italic$�66   
���  ����5&   (   �� $ placeholder��$5 /AdornedElement.(Validation.Errors)/ErrorContent�5'      5(      5*      5+      5-      .��5.      � 	 � ColumnDefinitions	 50   
   �� 51      $��1* �6   �� 52      $��2* �6   �� 53      $��1* �6   .+�56   
   �� $��1��6   .+�57      �� $��Enter Tour Name:��6   �� 58      #��[ 59      ��58      �� *NewTourText+
 �� Mode$
 TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�" �� ValidatesOnNotifyDataErrors$
 True��59      �� 5;      $��Enter Starting Point:��6   �� 5<      #��[ 5=      ��5<      �� *From+$
 TwoWay=� �� ValidatesOnExceptions$
 True��$ PropertyChanged=�$
 True��5=      �� 5?      $��Enter Destination:��6   �� 5@      #��[ 5A      ��5@      �� *To+$
 TwoWay=�$
 True��$ PropertyChanged=�$
 True��5A      �� 5C      $��Enter Tour Description:��6   �� 5D      $	��100��6   $
��Auto��5E       �� TextWrapping$
 Wrap=�5F       �� AcceptsReturn$
 True��5G      $ �� HorizontalScrollBarVisibility$ Disabled=�5H      " �� VerticalScrollBarVisibility$
 Auto=�5I      #��[ 5K      ��5J      �� *Description+$
 TwoWay=�$
 True��$ PropertyChanged=�$
 True��5K      �� 5M      $��Choose Route Type:��6   � 5N      	 5O      �� 5P      $��1* �6'   �� 5Q      $��1* �6'   .+�5S      ��  �� DisplayMemberPath$ Display��5U      #��[ 5V      ��5S      �� $ 
RouteItems� �� SelectedItem 5T      �� $ CurrentRoute�$
 TwoWay=�$
 True��$
 True��5V      5W      �� 5Y      $	��Add��6   $��120,20,120,0q�5[   D   ��5Y   #   �� *AddTourCommand+��5Z      ��  �� RelativeSource ��  �� AncestorType" �5[   D   5\      5^      5_      