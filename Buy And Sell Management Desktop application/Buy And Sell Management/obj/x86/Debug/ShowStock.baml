   M S B A M L   `   `   `  ����  Buy And Sell Management'    !Buy_And_Sell_Management.ShowStock   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  	ShowStock��5   	   $	��327��6   $	��339��6(    � WindowStartupLocation$ CenterScreen=�6b   .��5      �  � ColumnDefinitions 5   
   �� 5      $
��108* �6   6.   �� 5      $
��108* �6   6.    � RowDefinitions 5
   
   �� 5      $	��27* �6   6+   �� 5      $	��27* �6   6+   .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��showdata��5   `   $��3q�6   $��0��6   $��0��6+   $��2��6;   $��2��6L      AutoGenerateColumns .     EnableRowVirtualization . ��.     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5         FontSize$ 24Q�6%   ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 63   	   Header$
	 Name��6@   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6O   �� *Name+6l     5      $ 24Q�6%   $
 Auto 63   $	 Stock��6@   
 6P   �� *Stock+6n   5      5      5      