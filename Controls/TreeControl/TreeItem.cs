///  TreeView Control with ContentItems
///  from https://www.codeproject.com/tips/1096924/tabcontrol-with-treeview-navigation
/// 
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Aurora.Wpf.Controls.TreeControl
{
    [ContentProperty("Items")]
    public class TreeItem : ItemsControl
    {
        public static DependencyProperty IdProperty = DependencyProperty.Register("Id", typeof(Guid), typeof(TreeItem), new FrameworkPropertyMetadata(default(Guid), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Guid Id
        {
            get => (Guid)GetValue(IdProperty);
            set => SetValue(IdProperty, value);
        }

        public static DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded", typeof(bool), typeof(TreeItem), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }

        public static DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(TreeItem), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public static DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(object), typeof(TreeItem), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object Header
        {
            get => (object)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public static DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(TreeItem), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object Content
        {
            get => (object)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<TreeItem>), typeof(TreeItem), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public new ObservableCollection<TreeItem> Items
        {
            get => (ObservableCollection<TreeItem>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public TreeItem() : base()
        {
            this.Id = Guid.NewGuid();
            this.Opacity = 0;
            this.IsExpanded = true;
            this.Items = new ObservableCollection<TreeItem>();
        }
    }
}
