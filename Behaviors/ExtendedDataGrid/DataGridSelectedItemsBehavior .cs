using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#if NET48
using System.Windows.Interactivity;
#else
using Microsoft.Xaml.Behaviors;
#endif

namespace Aurora.Wpf.Behaviors.ExtendedDataGrid
{
    public class DataGridSelectedItemsBehavior : Behavior<DataGrid>
    {
        private bool _isUpdating;

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(IList), typeof(DataGridSelectedItemsBehavior), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsChanged));

        public IList SelectedItems
        {
            get => (IList)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGridSelectedItemsBehavior behavior && !behavior._isUpdating)
            {
                behavior._isUpdating = true;
                if (e.NewValue != null)
                {
                    behavior.AssociatedObject.SelectedItems.Clear();
                    foreach (var item in (IEnumerable)e.NewValue)
                    {
                        behavior.AssociatedObject.SelectedItems.Add(item);
                    }
                }
                behavior._isUpdating = false;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isUpdating)
            {
                _isUpdating = true;
                SelectedItems = AssociatedObject.SelectedItems.Cast<object>().ToList();
                _isUpdating = false;
            }
        }
    }

}
