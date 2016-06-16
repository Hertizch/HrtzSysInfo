using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HrtzSysInfo.Extensions;
using HrtzSysInfo.Properties;
using HrtzSysInfo.ViewModels;

namespace HrtzSysInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            //NetworkExtensions.CalculateMaxTranserSpeed();
        }

        // this is the offset of the mouse cursor from the top left corner of the window
        private Point _offset;

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // capturing the mouse here will redirect all events to this window, even if
            // the mouse cursor should leave the window area
            Mouse.Capture(this, CaptureMode.Element);

            var cursorPos = PointToScreen(Mouse.GetPosition(this));
            var windowPos = new Point(Left, Top);
            _offset = (Point)(cursorPos - windowPos);
        }

        private void MainWindow_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!Equals(Mouse.Captured, this) || Mouse.LeftButton != MouseButtonState.Pressed) return;

            var cursorPos = PointToScreen(Mouse.GetPosition(this));
            var newLeft = cursorPos.X - _offset.X;
            var newTop = cursorPos.Y - _offset.Y;

            const int snappingMargin = 10;

            if (Math.Abs(SystemParameters.WorkArea.Left - newLeft) < snappingMargin)
                newLeft = SystemParameters.WorkArea.Left;
            else if (Math.Abs(newLeft + ActualWidth - SystemParameters.WorkArea.Left - SystemParameters.WorkArea.Width) < snappingMargin)
                newLeft = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width - ActualWidth;

            if (Math.Abs(SystemParameters.WorkArea.Top - newTop) < snappingMargin)
                newTop = SystemParameters.WorkArea.Top;
            else if (Math.Abs(newTop + ActualHeight - SystemParameters.WorkArea.Top - SystemParameters.WorkArea.Height) < snappingMargin)
                newTop = SystemParameters.WorkArea.Top + SystemParameters.WorkArea.Height - ActualHeight;

            Left = newLeft;
            Top = newTop;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            // Set registry value
            RegistryExtensions.RegisterInStartup("HrtzSysInfo", Settings.Default.Ui_ExecuteAtStartup);

            // Save global settings
            if (GlobalSettingsVm.Instance.CmdSaveSettings.CanExecute(null))
                GlobalSettingsVm.Instance.CmdSaveSettings.Execute(null);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
           /* var bindingExpression = ((Window)sender).GetBindingExpression(LeftProperty);
            bindingExpression?.UpdateTarget();*/
        }
    }
}
