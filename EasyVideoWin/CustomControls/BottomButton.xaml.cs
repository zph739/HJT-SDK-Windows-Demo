﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EasyVideoWin.CustomControls
{
    /// <summary>
    /// Interaction logic for BottomButton.xaml
    /// </summary>
    public partial class BottomButton : UserControl
    {
        public BottomButton()
        {
            InitializeComponent();
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(BottomButton), new UIPropertyMetadata(null));

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(BottomButton), new UIPropertyMetadata(null));
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public event RoutedEventHandler Click;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }

        public ImageSource NormalImage
        {
            get { return (ImageSource)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }
        public static readonly DependencyProperty NormalImageProperty =
            DependencyProperty.Register("NormalImage", typeof(ImageSource), typeof(BottomButton), new UIPropertyMetadata(null));

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }
        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(BottomButton), new UIPropertyMetadata(null));

        public Visibility LeftRectangleVisibility
        {
            get { return (Visibility)GetValue(LeftRectangleVisibilityProperty); }
            set { SetValue(LeftRectangleVisibilityProperty, value); }
        }
        public static readonly DependencyProperty LeftRectangleVisibilityProperty =
            DependencyProperty.Register("LeftRectangleVisibility", typeof(Visibility), typeof(BottomButton), new UIPropertyMetadata(null));

        public Visibility RightRectangleVisibility
        {
            get { return (Visibility)GetValue(RightRectangleVisibilityProperty); }
            set { SetValue(RightRectangleVisibilityProperty, value); }
        }
        public static readonly DependencyProperty RightRectangleVisibilityProperty =
            DependencyProperty.Register("RightRectangleVisibility", typeof(Visibility), typeof(BottomButton), new UIPropertyMetadata(null));

    }
}
