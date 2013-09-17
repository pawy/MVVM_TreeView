using System.Windows;
using System.Windows.Controls;

namespace MVVM_TreeView.Core.View
{
    public class CustomTreeView : TreeView
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CustomTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is CustomTreeViewItem);
        }
    }

    public class CustomTreeViewItem : TreeViewItem
    {
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            this.IsSelected = true;
            this.RaiseEvent(e);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CustomTreeViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is CustomTreeViewItem);
        }
    }
}