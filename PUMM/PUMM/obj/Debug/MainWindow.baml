   M S B A M L   `   `   `  ����?  ;PUMM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:PUMMPUMM  5   	   %clr-namespace:PUMM.View	PUMM.View  5   	   /clr-namespace:PUMM.ViewModelPUMM.ViewModel  5	   	       PUMM.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   localclr-namespace:PUMM  5   	   "viewclr-namespace:PUMM.View  5   	   ,	viewModelclr-namespace:PUMM.ViewModel  5	   	   $	��900��5   	   $	��600��6     � WindowStartupLocation$  CenterScreen=�5   	    � Icon$> 8pack://application:,,,/PUMM;component/Resources/logo.ico��5   	    � Title 5   	   �� *Title+.  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Closing��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *SaveSession+5       � 	Resources 5       �� DataType"  PUMM.ViewModel.HomeViewModel   PUMM.View.Home%  PUMM.ViewModel.LibraryViewModel   PUMM.View.Library(	  "PUMM.ViewModel.NewModpackViewModel
   PUMM.View.NewModpack"  PUMM.ViewModel.ModsViewModel   PUMM.View.Mods#  PUMM.ViewModel.AboutViewModel   PUMM.View.About��%w  (��       5      *, +)(�� 2     5      *, +)(�� d     5      *,	 +)(�� �     5#      *, +)(�� �     5'      *, +)�� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   " 	 6   .R�5       
  5!      �� 5#   
   "  6   .R�5$        5%      �� 5'   
   "  6   .R�5(        5)      5*      .��5,      � 	 � ColumnDefinitions	 5-   
   �� 5.      $	��.5* �6   $	��280��6+   �� 5/      
 � RowDefinitions
 51   
   �� 52      $
��Auto �6   �� 53      �� 54      $
��Auto �6   .+�57   
   ��  �� Orientation$ Vertical=�6   $��0��6,   $��0��6<   $��3��6I   ��58      �� 59       �� ImageSource$I Cpack://application:,,,/PUMM;component/Resources/menu_background.png��6    j� Stretch$ UniformToFill=�6o   .+�5;      �� $E��?pack://application:,,,/PUMM;component/Resources/menu_header.png��6   $	��Top=�6]   $��16q�6u    �� BitmapScalingMode$
 Fant=�6�   �� 5<      -   5        $��tabHome��5<      $
��home��6C      
EnabledTab#��[  6q    � Cursor$
 Hand��6�   $��16, 0, 16, 0q�6�   ��6$   �� *
NavCommand+.��5=      ]� ��6   �� *ActualWidth+ �� RelativeSource �� *FindAncestor+ �� AncestorType" ��.+�5>      �� $C��=pack://application:,,,/PUMM;component/Resources/home_icon.png��6   $��22��6c   $��24, 14, 24, 14q�6n   $
 Fant=�6�   6�   �� 5?      $��Center=�6   
�������$��
0, 0, 0, 0q�6J   .��6^   Home6d   5@      5A      �� 5B      -   5        $��tabNewModpack��5B      $��new_modpack��6I     DisabledTab#��[ 6�   $
 Hand��6�   $��16, 5, 16, 0q�6�   ��6*   �� *
NavCommand+.��5C      ]� ��6   �� *ActualWidth+ �� *FindAncestor+" ��.+�5D      �� $B��<pack://application:,,,/PUMM;component/Resources/add_icon.png��6   $��22��6b   $��24, 14, 24, 14q�6m   $
 Fant=�6�   6�   �� 5E      $��Center=�6   
�������$��
0, 0, 0, 0q�6J   .��6^   New Modpack6k   5F      5G      �� 5H      -   5        $��
tabLibrary��5H      $��library��6F   #��[ 6z   $
 Hand��6�   $��16, 5, 16, 0q�6�   ��6'   �� *
NavCommand+.��5I      ]� ��6   �� *ActualWidth+ �� *FindAncestor+" ��.+�5J      �� $F��@pack://application:,,,/PUMM;component/Resources/library_icon.png��6   $��22��6f   $��24, 14, 24, 14q�6q   $
 Fant=�6�   6�   �� 5K      $��Center=�6   
�������$��
0, 0, 0, 0q�6J   .��6^   	Library6g   5L      5M      �� 5N      -   5        $��tabMods��5N      $
��mods��6C   #��[ 6q   $
 Hand��6�   $��16, 5, 16, 0q�6�   ��6$   �� *
NavCommand+.��5O      ]� ��6   �� *ActualWidth+ �� *FindAncestor+" ��.+�5P      �� $C��=pack://application:,,,/PUMM;component/Resources/mods_icon.png��6   $��22��6c   $��24, 14, 24, 14q�6n   $
 Fant=�6�   6�   �� 5Q      $��Center=�6   
�������$��
0, 0, 0, 0q�6J   .��6^   Mods6d   5R      5S      �� 5T      -   5        $��tabAbout��5T      $��about��6D   #��[ 6t   $
 Hand��6�   $��16, 5, 16, 0q�6�   ��6%   �� *
NavCommand+.��5U      ]� ��6   �� *ActualWidth+ �� *FindAncestor+" ��.+�5V      �� $C��=pack://application:,,,/PUMM;component/Resources/info_icon.png��6   $��22��6c   $��24, 14, 24, 14q�6n   $
 Fant=�6�   6�   �� 5W      $��Center=�6   
�������$��
0, 0, 0, 0q�6J   .��6^   About6e   5X      5Y      5Z      � 5\   
   -   5         �Name$ MainContent��5\      $��1��6&   $��1��66   .+�5]      �� ��6   �� *CurrentViewModel+5^      � 5`   
   -   5        $ barActiveModpack��5`      $��1��6)   $��2��69   $y�	Collapsed=�6F   	 5a      �� 5b      $	��.2* �6#   �� 5c      
 5e      �� 5f      .+�5i      �� $
��Left=�6   $��Center=�64   $��28 0 0 0q�6O   $��0��6a   $��0��6q   .+�5j      ��  	 Close#��[ 69   $��36��6X   $��36��6c   ��6   �� *ClearActive+.��5k      �� $D��>pack://application:,,,/PUMM;component/Resources/close_icon.png��6   $
 Fant=�6d   $��6q�6�   5l      �� 5m      $��Center=�6   $��0 4 0 0q�69   Close6Q   5n      �� 5p      $ 
Horizontal=�6   $��Right=�62   $��1��6N   $��0��6^   
�������$��88��6~   .+�5q      �� $ Vertical=�6   $��Bottom=�64   .+�5r      �� -   5        $��labelModpackName��5r      $��Right=�64   $
 Hand��6�   ��6P   �� *Active.Name+6�   �� 5s      -	   5        $��inputModpackName��5s   !   $ 
Horizontal=�69   $y�	Collapsed=�6R   .+�5t      �� -
   5         ��Name$ newModpackName��5t   "   $��Right=�6:     PrimaryInput#��[ 6�   ��6V   �� *PotentialName+ �� Mode$ TwoWay=�6�   �� 5u      -   5        $��buttonSaveModpackName��5u   !     Icon#��[ 6~   $��4 0 4 0q�6�   $��14��6�   ��6b   �� *	SetName+.��5v      �� $D��>pack://application:,,,/PUMM;component/Resources/apply_icon.png��6$   $
 Fant=�6l   6�   5w      5x      �� 5y      $��Right=�6    �� ContentStringFormat$ 	Mods: {0}��6^   ��68   �� *Active.Mods.Count+6^   5z      �� 5{        ThumbnailButton#��[ 6=   $	��128��6f   $��88��6r   ��6   �� *BrowseThumbnail+��5|      �� 5}      $ UniformToFill=�6%   $
 Fant=�6=    6d   �� *Active.Thumbnail+5      5�      5�      5�      5�      