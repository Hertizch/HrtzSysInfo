using System.Windows;

namespace HrtzSysInfo.Controls
{
    /// <summary>
    /// Interaction logic for ValueContainer.xaml
    /// </summary>
    public partial class ValueContainer
    {
        public ValueContainer()
        {
            InitializeComponent();
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double ProgressValue
        {
            get { return (double)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        public double ProgressMaxValue
        {
            get { return (double)GetValue(ProgressMaxValueProperty); }
            set { SetValue(ProgressMaxValueProperty, value); }
        }

        public bool ShowProgressBar
        {
            get { return (bool)GetValue(ShowProgressBarProperty); }
            set { SetValue(ShowProgressBarProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(ValueContainer), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(ValueContainer), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ProgressValueProperty = DependencyProperty.Register("ProgressValue", typeof(double), typeof(ValueContainer), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ProgressMaxValueProperty = DependencyProperty.Register("ProgressMaxValue", typeof(double), typeof(ValueContainer), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ShowProgressBarProperty = DependencyProperty.Register("ShowProgressBar", typeof(bool), typeof(ValueContainer), new PropertyMetadata(true));
    }
}
