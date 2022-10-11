using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Aurora.Wpf
{
    /// <summary>
    /// Add auto scroll functionality to the Listbox Control
    /// </summary>
    public class AutoScrollListBox : ListBox
    {
        /// <summary>
        /// XAML Property to activate Auto scroll
        /// </summary>
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.Register(nameof(AutoScroll), typeof (bool), typeof (AutoScrollListBox), new UIPropertyMetadata(default(bool), OnAutoScrollChanged));
        /// <summary>
        /// activate the auto scroll feature
        /// </summary>
        public bool AutoScroll
        {
            get => (bool)GetValue(AutoScrollProperty);
            set => SetValue(AutoScrollProperty, value);
        }
        /// <summary>
        /// handling a change of the autoScroll property
        /// </summary>
        /// <param name="dependecyObject"></param>
        /// <param name="e"></param>
        public static void OnAutoScrollChanged(DependencyObject dependecyObject, DependencyPropertyChangedEventArgs e)
        {
            AutoScrollListBox thisLb = (AutoScrollListBox) dependecyObject;
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
