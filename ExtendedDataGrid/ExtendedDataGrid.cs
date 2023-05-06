using System;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ServiceStack;

namespace Aurora.Wpf
{
    /// <summary>
    /// DataGrid with additional functionality
    ///
    /// - <see cref="ExpandLastColumn"/> property to enable the last column to expand to the remaining space
    /// - <see cref="AutoScroll"/> property to enable autoscroll to the last element in the datagrid
    /// - <see cref="CopyLinesMenu"/> property to enable the copy lines context menu
    /// - <see cref="CopyCellsMenu"/> property to enable the copy cells context menu
    /// - <see cref="MouseWheelFontZoom"/> property to enable zoom via ctrl+mouse wheel
    /// 
    /// </summary>
    public partial class ExtendedDataGrid : DataGrid
    {
        #region LastColumnExpand
        /// <summary>
        /// controlls if the last column in the datagrid is using up all the available space
        /// </summary>
        public bool ExpandLastColumn
        {
            get => (bool)(GetValue(ExpandLastColumnProperty) ?? false);
            set
            {
                if (value != ExpandLastColumn) SetValue(ExpandLastColumnProperty, value);
            }
        }
        /// <summary>
        /// XAML Property to activate ExpandLastColumn
        /// </summary>
        public static readonly DependencyProperty ExpandLastColumnProperty =
            DependencyProperty.Register(nameof(ExpandLastColumn), typeof(bool), typeof(ExtendedDataGrid), new UIPropertyMetadata(default(bool), OnLasColumnExpandChanged));
        private static void OnLasColumnExpandChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            ExtendedDataGrid dataGrid = (ExtendedDataGrid)s;
            if (e.NewValue == e.OldValue)
                return;
            dataGrid.ExpandLastColumn = (bool)e.NewValue;

        }

        #endregion
        #region AutoScroll
        /// <summary>
        /// XAML Property to activate AutoScroll
        /// </summary>
        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.Register(nameof(AutoScroll), typeof(bool), typeof(ExtendedDataGrid), new UIPropertyMetadata(default(bool), OnAutoScrollChanged));

        /// <summary>
        /// controls if auto scroll for the DataGrid is active
        /// </summary>
        public bool AutoScroll
        {
            get { return (bool)(GetValue(AutoScrollProperty) ?? false); }
            set { if (value != AutoScroll) SetValue(AutoScrollProperty, value); }
        }

        private static void OnAutoScrollChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            ExtendedDataGrid dataGrid = (ExtendedDataGrid)s;
            bool newValue = (bool)e.NewValue;
            bool oldValue = (bool)e.OldValue;
            var notifyCollection = dataGrid.Items as INotifyCollectionChanged;


            // Add the event handler in case that the property is set to true
            if (newValue && !oldValue)
                notifyCollection.CollectionChanged += dataGrid.notifyCollection_CollectionChanged;

            // Remove the event handle in case the property is set to false
            if (!newValue && oldValue)
                notifyCollection.CollectionChanged -= dataGrid.notifyCollection_CollectionChanged;
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
        #endregion
        #region ContextMenus
        private MenuItem m_CopyLinesMenu;
        private MenuItem m_CopyCellsMenu;
        private string? m_ContextMenuSelectedColumn;

        /// <summary>
        /// XAML Property to activate the copy lines contextmenu
        /// </summary>
        public static readonly DependencyProperty CopyLinesMenuProperty =
            DependencyProperty.Register(nameof(CopyLinesMenu), typeof(bool), typeof(ExtendedDataGrid), new UIPropertyMetadata(default(bool), OnCopyMenusChanged));

        /// <summary>
        /// controls if the CopyLines context menu should be enablesd
        /// </summary>
        public bool CopyLinesMenu
        {
            get { return (bool)(GetValue(CopyLinesMenuProperty) ?? false); }
            set { if (value != CopyLinesMenu) SetValue(CopyLinesMenuProperty, value); }
        }
        /// <summary>
        /// XAML Property to activate the copy cells contextmenu
        /// </summary>
        public static readonly DependencyProperty CopyCellsMenuProperty =
            DependencyProperty.Register(nameof(CopyCellsMenu), typeof(bool), typeof(ExtendedDataGrid), new UIPropertyMetadata(default(bool), OnCopyMenusChanged));

        /// <summary>
        /// controls if the CopyCells context menu should be enablesd
        /// </summary>
        public bool CopyCellsMenu
        {
            get { return (bool)(GetValue(CopyCellsMenuProperty) ?? false); }
            set { if (value != CopyCellsMenu) SetValue(CopyCellsMenuProperty, value); }
        }

        private static void OnCopyMenusChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            ExtendedDataGrid dataGrid = (ExtendedDataGrid)s;
            if (e.NewValue == e.OldValue)
                return;
            if (e.Property == CopyCellsMenuProperty)
            {
                dataGrid.HandleCellsMenu();
            }
            else if (e.Property == CopyLinesMenuProperty)
            {
                dataGrid.HandleLinesMenu();
            }
        }
        private void CopyCellsMenuOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            string clipBoardText = string.Empty;
            foreach (var line in SelectedItems)
            {
                if (line is DataRowView dataRowView)
                {
                    if (dataRowView.DataView.Table != null)
                    {
                        int columnIndex = dataRowView.DataView.Table.Columns.IndexOf(m_ContextMenuSelectedColumn);
                        clipBoardText += dataRowView[columnIndex] + Environment.NewLine;
                    }
                }
                else
                {
                    PropertyInfo propInfo = line.GetType().GetProperty(m_ContextMenuSelectedColumn, BindingFlags.Public | BindingFlags.Instance);
                    if (propInfo != null)
                        clipBoardText += propInfo.GetValue(line) + Environment.NewLine;
                }
                
            }
            Clipboard.SetDataObject(clipBoardText);
        }

        /// <summary>
        /// Wählt eine Zelle aus, wenn das Kontextmenü geöffnet wird.
        /// </summary>
        /// <param name="e">Das Element, dessen Kontextmenü geöffnet wurde.</param>
        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            HandleCellsMenu();
            HandleLinesMenu();
            base.OnContextMenuOpening(e);
        }

        /// <summary>
        /// Löst das <see cref="E:System.Windows.Controls.DataGrid.CopyingRowClipboardContent"/>-Ereignis aus.
        /// </summary>
        /// <param name="args">Die Daten für das Ereignis.</param>
        protected override void OnCopyingRowClipboardContent(DataGridRowClipboardEventArgs args)
        {
            Clipboard.SetText(args.Item.ToString());
            base.OnCopyingRowClipboardContent(args);
        }

        /// <summary>
        /// Wird aufgerufen, wenn ein nicht behandeltes <see cref="E:System.Windows.UIElement.MouseRightButtonDown"/>-Routingereignis auf seiner Route ein Element erreicht, das von dieser Klasse abgeleitet ist. Implementieren Sie diese Methode, um eine Klassenbehandlung für dieses Ereignis hinzuzufügen.
        /// </summary>
        /// <param name="e">Die Instanz von <see cref="T:System.Windows.Input.MouseButtonEventArgs"/>, die die Ereignisdaten enthält. In den Ereignisdaten wird angegeben, dass die rechte Maustaste gedrückt wurde.</param>
        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            m_ContextMenuSelectedColumn = CurrentCell.Column?.SortMemberPath ?? null;
            DependencyObject? dep = (DependencyObject?)e.OriginalSource;
            while ((dep != null) && dep is not DataGridCell && dep is not DataGridColumnHeader)
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep is DataGridCell cell)
            {
                m_ContextMenuSelectedColumn = cell.Column?.SortMemberPath ?? null;
            }
            if (dep is DataGridColumnHeader header)
            {
                m_ContextMenuSelectedColumn = header.Column?.SortMemberPath ?? null;
            }
            base.OnMouseRightButtonDown(e);
        }

        private void HandleLinesMenu()
        {
            if (m_CopyLinesMenu == null)
                m_CopyLinesMenu = new MenuItem() { Header = "Copy Line(s)", Command = ApplicationCommands.Copy, Visibility = Visibility.Visible };
            if (ContextMenu == null)
                return;
            if (CopyLinesMenu)
            {
                if (!ContextMenu.Items.Contains(m_CopyLinesMenu))
                    ContextMenu.Items.Add(m_CopyLinesMenu);
                else
                    m_CopyLinesMenu.Visibility = Visibility.Visible;
            }
            else
            {
                if (ContextMenu.Items.Contains(m_CopyLinesMenu))
                    m_CopyLinesMenu.Visibility = Visibility.Hidden;
            }
            m_CopyLinesMenu.IsEnabled = SelectedItems.Count > 0;
        }
        private void HandleCellsMenu()
        {
            if (ContextMenu == null)
                return;
            if (m_CopyCellsMenu == null)
            {
                m_CopyCellsMenu = new MenuItem() { Header = "Copy Cell(s)", Visibility = Visibility.Visible };
                m_CopyCellsMenu.Click += CopyCellsMenuOnClick;
            }
            if (CopyCellsMenu)
            {
                if (!ContextMenu.Items.Contains(m_CopyCellsMenu))
                    ContextMenu.Items.Add(m_CopyCellsMenu);
                else
                    m_CopyCellsMenu.Visibility = Visibility.Visible;
            }
            else
            {
                if (ContextMenu.Items.Contains(m_CopyCellsMenu))
                    m_CopyCellsMenu.Visibility = Visibility.Hidden;
            }
            m_CopyCellsMenu.IsEnabled = SelectedItems.Count > 0;
        }

        #endregion
        #region MouseWheelZoom
        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {

            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            // We'll need a DoubleAnimation object to drive
            // the ScaleX and ScaleY properties.
            if (MouseWheelFontZoom)
            {
                if (e.Delta > 0)
                    FontSize++;
                else
                    FontSize = Math.Max(FontSize - 1, 1);
            }

        }
        /// <summary>
        /// XAML Property to activate the zoom on mousewheel feature
        /// </summary>
        public static readonly DependencyProperty MouseWheelFontZoomProperty =
            DependencyProperty.Register(nameof(MouseWheelFontZoom), typeof(bool), typeof(ExtendedDataGrid), new UIPropertyMetadata(false));
        
        /// <summary>
        /// controls if the Ctrl+mousewheel zoom feature is enabled
        /// </summary>
        public bool MouseWheelFontZoom
        {
            get => (bool)GetValue(MouseWheelFontZoomProperty);
            set => SetValue(MouseWheelFontZoomProperty, value);
        }
        #endregion
        #region To life and Die in Starlight
        /// <summary>
        /// init mousewheel event method and set necessary properties
        /// </summary>
        public ExtendedDataGrid()
        {
            PreviewMouseWheel += OnMouseWheel;
            CanUserAddRows = false;
            GridLinesVisibility = DataGridGridLinesVisibility.None;
        }
        #endregion
    }
}
