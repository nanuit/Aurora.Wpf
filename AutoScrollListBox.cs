using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Aurora.Wpf
{
    public class AutoScrollListBox : ListBox
    {
        // Using a DependencyProperty as the backing store for AutoScoll.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.Register("AutoScroll", typeof (bool), typeof (AutoScrollListBox), new UIPropertyMetadata(default(bool), OnAutoScrollChanged));

        public bool AutoScroll
        {
            get => (bool)GetValue(AutoScrollProperty);
            set => SetValue(AutoScrollProperty, value);
        }

        public static void OnAutoScrollChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            AutoScrollListBox thisLb = (AutoScrollListBox) s;
            bool newValue = (bool)e.NewValue;
            bool oldValue = (bool)e.OldValue;
            INotifyCollectionChanged notifyCollection = thisLb.Items as INotifyCollectionChanged;

            // Add the event handler in case that the property is set to true
            if (newValue && !oldValue)
                notifyCollection.CollectionChanged += thisLb.notifyCollection_CollectionChanged;
            
            // Remove the event handle in case the property is set to false
            if (!newValue && oldValue)
                notifyCollection.CollectionChanged -= thisLb.notifyCollection_CollectionChanged;
        }

        private void notifyCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var itemCollection = sender as ItemCollection;
            //Scroll into the last item
            if (itemCollection?.Count > 1)
            {
                this.ScrollIntoView(itemCollection[itemCollection.Count - 1]);
                UnselectAll();
            }
        }
    }
}
