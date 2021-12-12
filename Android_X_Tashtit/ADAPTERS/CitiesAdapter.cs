using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MODEL;
using AndroidX.RecyclerView.Widget;

namespace Android_X_Tashtit.ADAPTERS
{
    public class CitiesAdapter : BaseRecyclerAdapter<City>
    {
        public CitiesAdapter(RecyclerView recyclerView, List<City> items, int? layoutId = null)
            : base(recyclerView, items, layoutId)
        { }

        protected override void OnLookupViewItems(View layout, BaseViewHolder viewHolder)
        {
            TextView name = layout.FindViewById<TextView>(Resource.Id.txtName);

            viewHolder.AddView(name, "name");
        }

        protected override void OnUpdateView(BaseViewHolder viewHolder, City item)
        {
            viewHolder.GetView<TextView>("name").Text = item.Name;
        }
    }
}