using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SaaMedW
{
    public class ScrollingDataGrid : DataGrid
    {
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                int newItemCount = e.NewItems.Count;

                if (newItemCount > 0)
                    this.ScrollIntoView(e.NewItems[newItemCount - 1]);
            }
            base.OnItemsChanged(e);
        }
        //protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    int newItemCount = e.NewItems.Count;

        //    if (newItemCount > 0)
        //        this.ScrollIntoView(e.NewItems[newItemCount - 1]);

        //    base.OnItemsChanged(e);
        //}
    }
}
