   M S B A M L   `   `   `  ����D  @WPFClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WPFClient.ViewModelWPFClient.ViewModel  5   	   %clr-namespace:WPFClient	WPFClient  5   	       WPFClient.MainWindow   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0VM!clr-namespace:WPFClient.ViewModel     5   	   )localclr-namespace:WPFClient     5   	     � Title$  Client��5	   	   $	��450��6   $	��800��6%    � DataContext 5
      .   (WPFClient.ViewModel.ChartClientViewModel  5   
   .��5      �  � ColumnDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5       � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      .+�5!   
   �� $��4��6   $��7��6'   5#      �� 5$   
   -   5         ��Name$ MessageTextBox��5$      $��2��6*   $��2��67   $��4��6H   $��2��6X   $��
Input text��6l   �� 5%   
   $��4��6   $��4��6!   $��Connect client to server��6.   ��6Q   �� *ConnectClient+�� 5&   
   $��4��6   $��5��6!   $��SentMessage��6.   ��6D   �� *SendmessageBtn+��6g   ��  �� ElementName$ MessageTextBox�� �� Path$
 Text��� 5'   
   $��4��6   $��6��6!   $��CloseClient��6.   ��6D   �� *CloseClientBtn+5(      5)      