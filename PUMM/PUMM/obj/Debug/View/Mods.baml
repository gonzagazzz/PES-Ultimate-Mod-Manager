   M S B A M L   `   `   `  ����?  ;PUMM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:PUMM.View	PUMM.View  5          PUMM.View.Mods   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      #localclr-namespace:PUMM.View  5      .��5
      � ���#EDF1F2  � ColumnDefinitions  5   
   �� 5      �� 5      $	��.4* �6    � RowDefinitions 5   
   �� 5      �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ listMods��5      $��	12 12 0 0q�6B   $��0��6U   $��0��6b   ��6%   �� *Mods+ �� View 5      �� 5      P�5      �� 5       �� Width$ 32��6)    �� CellTemplate 5      �� 5   "   .R�5   &   ��  d� 	IsChecked 6/   �� *
Selected+ �� Mode$ TwoWay=���6[   ��  �� ElementName$ listMods��	 �� Path$"	 DataContext.SelectionChanged���6�   �� 5   #   5      �� 5      $	��CPK��6)    5       �� 5!   "   .R�5"   &   �� ��60   �� *
Filename+5#   #   5%      5'      5)      �� 5+   
   $��
12 12 12 0q�67   $��0��6K   $��1��6X   ��6   �� *CheckedMods+ 5,      �� 5-      P�5.      �� 5/      $	��CPK��6)    50      �� 51   "   .R�52   &   �� ��60   �� 6D   53   #   55      57      59      �� 5;   
      PrimaryButton#��[  64   $��
16 12 8 12q�6[   $��0��6o   $��1��6   ��6   �� *GenerateBinary+.��6�   Generate DpFileList.bin6�   �� 5<   
   #��[  61   $��
8 12 16 12q�6X   $��1��6l   $��1��6|   ��6   �� *SaveModpack+.��6�   Save Modpack6�   5=      5>      