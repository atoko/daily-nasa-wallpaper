   M S B A M L   `   `   `  ����D  @SpaceADay, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:SpaceADay	SpaceADay  5          SpaceADay.Carousel   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -	SpaceADayclr-namespace:SpaceADay     5        � 	Resources  5	       
  Brush2 �� Color  CalendarDayButtonStyle1 �� 
TargetType:  4System.Windows.Controls.Primitives.CalendarDayButton �� Property �� Value �� 
TargetType � 	Resources%   SpaceADay.RedLetterDayConverter  Key'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Transitions%  System.Windows.VisualTransition	   GeneratedDuration
   States   System.Windows.VisualState   
Storyboard g� Duration	 W� To �� TargetProperty �� 
TargetName R� KeyTime R� Value  System.Windows.Visibility	 �� To ��Name J� Opacity �� RadiusY �� RadiusX J� IsHitTestVisible ��Name � ToolTip F�Name J� RenderTransformOrigin �� Stretch �� Triggers �� Binding  	converter �� 	Converter  �� Value! �� 
TargetName��%�  &	        &	 #     7  �� 5
   
   $ 	#FF5D39D3��6)   �� 5   
   "  60   .�5      ��  � ��$ 5��6)   �� 5       � ��$ 5��6*   �� 5       � ��$ 10(�6)   �� 5       � ��$ Center=�6;   �� 5       � ��$ Center=�69   �� 5       � �� 5      �� 5      "  6&    5      ��  5      	converter 6R   5      .R�5      �  5        5   "   CommonStates�� 5   &     5   *   $	 0:0:0.1D�6;   .
 5   &     
Normal��62     5   &   	MouseOver��. 5   *   �� .��5    .   W� $ 0D�6>   $	 0.5X�6K   $ Opacity�6T   $ HighlightBackground��6x   5!   +   5"   '     5#   &   Pressed��. 5$   *   �� .��5%   .   W� $ 0D�6>   $	 0.5X�6K   $ Opacity�6T   $ HighlightBackground��6x   5&   +   5'   '     5(   &   Disabled��. 5)   *   �� .��5*   .   W� $ 0D�6>   $ 0X�6K   $ Opacity�6R   $ HighlightBackground��6v   W� 5+   .   $ 0D�6>   $	 .35X�6K   $ Opacity�6T   $ 
NormalText��6x   5,   +   5-   '   5.   #     5/   "   SelectionStates�� 50   &     51   *   $	 0D�6;   .
 53   &     
Unselected��62     54   &   Selected��. 55   *   �� .��56   .   W� $ 0D�6>   $	 .75X�6K   $ Opacity�6T   $ SelectedBackground��6x   57   +   58   '   59   #     5:   "   CalendarButtonFocusStates�� 5;   &     5<   *   $	 0D�6;   .
 5>   &     CalendarButtonFocused��. 5?   *   �� .��5@   .   T� $ 0D�6L   $ 
Visibility�6Y   $ DayButtonFocusVisual��6�   ..�5A   2   i� $ 0��6I    5B   6    5C   :   Visible=�6N   5E   3   5F   /   5G   +   5H   '     5I   &   CalendarButtonUnfocused��. 5J   *   �� .��5K   .   T� $ 0D�6L   $ 
Visibility�6Y   $ DayButtonFocusVisual��6�   ..�5L   2   i� $ 0��6I    5M   6    5N   :   	Collapsed=�6P   5P   3   5Q   /   5R   +   5S   '   5T   #     5U   "   ActiveStates�� 5V   &     5W   *   $	 0D�6;   .
 5Y   &     
Active��62     5Z   &   Inactive��. 5[   *   �� .��5\   .   �� $ 0D�6=   $ 	#FF777777X�6J   $6 0(TextElement.Foreground).(SolidColorBrush.Color)�6Y   $ 
NormalText��6�   5]   +   5^   '   5_   #     5`   "   	DayStates�� 5a   &     5b   *   $	 0D�6;   .
 5d   &     
RegularDay��62     5e   &   	Today��. 5f   *   �� .��5g   .   W� $ 0D�6>   $ 1X�6K   $ Opacity�6R   $ TodayBackground��6v   �� 5h   .   $ 0D�6=   $ 	#FFFFFFFFX�6J   $6 0(TextElement.Foreground).(SolidColorBrush.Color)�6Y   $ 
NormalText��6�   5i   +   5j   '   5k   #     5l   "   BlackoutDayStates�� 5m   &     5n   *   $	 0D�6;   .
 5p   &     	NormalDay��62     5q   &   BlackoutDay��. 5r   *   �� .��5s   .   W� $ 0D�6>   $ .2X�6K   $ Opacity�6S   $ Blackout��6w   5t   +   5u   '   5v   #   .+�5x      �� $ TodayBackground��6(   ���	#FFAAAAAA$ 0Q�6R   $ 1��6^   $ 1��6j   �� 5y      $ SelectedBackground��6(   ���	#FFBADDE9$ 0Q�6U   $ 1��6a   $ 1��6m   �� 5z      #��z��6%   #��z��6Q   #��z��6�   �� 5{      $ HighlightBackground��6(   ���	#FFBADDE9$ 0Q�6V   $ 1��6b   $ 1��6n   �� 5|      $ RedLetterDayBackground��6(    .  ���	#801877906a   �� 5~      $ 
NormalText��6/   ���	#FF333333#��z��6f   $��5,1,5,1q�6�   #��z��6�   $ He��6�   F� 5      $ Blackout��6#   }�����5 �Z @�J� �Q2� �J� �̉� iV� �A�� �J� �W�� �J� ѷ�� #�� �>8� 7T�K�� 7T�� � ��� ћ�� 7T����G @7T��� �D� 	���	#FF000000$��Stretch=�6J  $��3q�6h  $ 0Q�6s  $ 0.5,0.5&�6  $
 Fill=�6�  $��Stretch=�6�  �� 5�      $ DayButtonFocusVisual��6(    .  $ 1��6_   $ 1��6k   ���	#FF45D6FA$y�	Collapsed=�6�   5�       5�      �� 5�       6*   �� 8     6c   Z� .a�5�   "   �� $! RedLetterDayBackground��6)    � y�$ Hidden=�6c   5�      5�      5�      5�      5�      .��5�      �� -   5        " ��Name$" mainGrid��5�      $��0q�6#   # �� Orientation$# 
Horizontal=�6.   
�������$
��Left=�6Y   $	��Top=�6t   .+�5�   
   �� $# Vertical=�6   $	��179��6,   .+�5�      &   System.Windows.Controls.Calendar  -   5        $  Name$$ calendar��5�      %   SelectionMode$% SingleRange=�6)   $��Center=�6E   &   CalendarDayButtonStyle#& �  6`   �� 5�      -   5        ' ��Name$' btnLoad��5�      $��Load ��6&   $
��10,0q�6L   #���   6Z   
�������$��16(�6�   $��40��6�   ��5�      �� 5�      # � ��6&   5�      � 5�      -   5        ( �Name$( progress��5�      $��24��6,   $
��10,0q�68   ) � SmallChange$) 1Q�6F   * � Value$* 50Q�6V   5�      �� 5�   
   -   5        + ��Name$
+ list��5�      $��0q�6!   $	��255��6,   $, �� IsSynchronizedWithCurrentItem$, FalseY�69   - �� SelectionMode$- Single=�6�   ��6_   �� *
IndexFiles+6�   �� 5�   
   $	��213��6   $
��Fill=�6E   $y�	Collapsed=�6T   ��6   �� *	Display+. �� Mode$. OneWay=�6T   5�      5�      