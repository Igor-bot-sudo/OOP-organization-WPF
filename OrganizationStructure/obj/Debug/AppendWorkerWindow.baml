   M S B A M L   `   `   `  ����P  LOrganizationStructure, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:OrganizationStructureOrganizationStructure  5   	   .    (OrganizationStructure.AppendWorkerWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:OrganizationStructure     5   	     � Title$/  )Добавление сотрудника��5   	   $��315.6��6'   $	��250��66    � WindowStyle$ 
ToolWindow=�6B    � WindowStartupLocation$ CenterScreen=�6[   .��5	      � .+�5
   
   �� $��Имя:��6   $��10q�5      $��25��6   $��Center=�6(   $	��Top=�5      $	��220��6(   �� 5   
   -   5         ��Name$ tbxFirstName��5      $��10,40,10,10q�6(   $��25��6=   $��Center=�5      $	��Top=�65   $	��220��5       J� RenderTransformOrigin$ 0.5,0.5&�6   �� 5   
   $��Фамилия:��6   $��10,65,10,10q�5      $��25��6%   $��Center=�61   $	��Top=�5      $	��220��6(   �� 5   
   -   5        $ tbxLastName��5      $��10,90,10,10q�6'   $��25��6<   $��Center=�5      $	��Top=�65   $	��220��5      $ 0.5,0.5&�6   �� 5   
   $��Возраст:��6   $��10,120,10,10q�5      $��25��6&   $��Center=�62   $	��Top=�5      $	��220��6(   �� 5   
   -   5        $ tbxAge��5      $��10,145,10,10q�6"   $��25��68   $��Center=�5      $	��Top=�65   $	��220��5      $ 0.5,0.5&�6   �� 5   
   -   5         ��Name$
 lbem��5      $��Должность:��6   $��10,175,10,10q�5      $��25��6&   $��Center=�62   $	��Top=�5      $	��220��6(   �� 5   
   -   5         ��Name$ 
cbCategory��5      $��10,200,10,10q�6'   $��25��6=   $��Center=�5       $	��Top=�65   $	��220��5!      $ 0.5,0.5&�6   .@�5"      ��  �� 
IsSelected . .��6-   
Кодер64   �� 5#      .��6   Интерн6#   5$      �� 5%   
   -   5         ��Name$ btnAdd��5%      $+��%Добавить сотрудника��6!   $��10,240,10,0q�6?   $��25��5&      $	��Top=�6   65   5'      5(      