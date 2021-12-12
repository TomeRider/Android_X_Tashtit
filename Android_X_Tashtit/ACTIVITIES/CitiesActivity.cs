using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using MODEL;
using HELPER;
using Java.Lang;
using AndroidX.RecyclerView.Widget;

using Android_X_Tashtit.ADAPTERS;
using Google.Android.Material.FloatingActionButton;

namespace Android_X_Tashtit.ACTIVITIES
{
    [Activity(Label = "Cities")]
    public class CitiesActivity : BaseActivity
    {
        private RecyclerView       lvCities;
        private EditText           etCity;
        private ImageButton        btnOk;
        private ImageButton        btnCancel;
        private TextView           txtHeader;
        private LinearLayout         llData;
        private FloatingActionButton fabAdd;

        private Cities             cities;
        private City               oldCity;
        private CitiesAdapter      adapter;

        int position = -1;
        
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.cities_layout);

            InitializeViews();

            txtHeader.Text = "Cities list";
            etCity.Hint    = "New City";

            cities = new Cities();
            ProgressDialog progress = Global.SetProgress(this);
            cities = await cities.SelectAll();
            SetupRecyclerView();
            progress.Dismiss();

            Keyboard.HideKeyboard(this, true);

            /*
             cities.FetchAndListen();
            adapter.NotifyDataSetChanged();
            */
        }

        protected override void InitializeViews()
        {
            lvCities  = FindViewById<RecyclerView>(Resource.Id.lvCities);
            etCity    = FindViewById<EditText>(Resource.Id.etCity);
            btnOk     = FindViewById<ImageButton>(Resource.Id.btnOk);
            btnCancel = FindViewById<ImageButton>(Resource.Id.btnCancel);
            txtHeader = FindViewById<TextView>(Resource.Id.txtHeader);
            fabAdd    = FindViewById<FloatingActionButton>(Resource.Id.fabAdd);
            llData    = FindViewById<LinearLayout>(Resource.Id.llData);

            btnOk.Click     += BtnOk_Click;
            btnCancel.Click += BtnCancel_Click;

            fabAdd.Click += delegate { llData.Visibility = ViewStates.Visible; };
        }

        private void SetupRecyclerView()
        {
            adapter = new CitiesAdapter(lvCities, cities, Resource.Layout.single_city_layout);
            lvCities.SetAdapter(adapter);
            lvCities.SetLayoutManager(new LinearLayoutManager(this));
            lvCities.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Horizontal));

            adapter.ItemSelected     += Adapter_ItemSelected;
            adapter.LongItemSelected += Adapter_LongItemSelected;
        }

        private void Adapter_LongItemSelected(object sender, City e)
        {
            position = cities.FindIndex(item => item.Name == e.Name);
            Global.YesNoAlertDialog(this, 
                                    "Confim Delete", 
                                    "Once '" + cities[position].Name + "' deleted the move cannot be undone", 
                                    "Yes", 
                                    "No",
                                    Delete);
        }

        private async void Delete(bool obj)
        {
            if (obj)
            {
                City city = cities[position];
                cities.Delete(city);
                await cities.Save();

                adapter.NotifyDataSetChanged();
            }
        }

        private void Adapter_ItemSelected(object sender, City e)
        {
            position    = cities.FindIndex(item => item.Name == e.Name);
            oldCity     = e;
            etCity.Text = e.Name;
            llData.Visibility = ViewStates.Visible;

            Keyboard.HideKeyboard(this, false);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            bool isNew = position == -1;
            bool dataSetChanged = false;

            llData.Visibility = ViewStates.Visible;
            Keyboard.HideKeyboard(this);

            if (etCity.Text != "")
            {
                City city = new City(etCity.Text);

                if (city.Validate())
                {
                    if (isNew)
                    {
                        if (cities.Add(city))
                            dataSetChanged = true;
                    }
                    else
                    {
                        if (cities.Modify(oldCity, city))
                            dataSetChanged = true;
                    }

                    if (dataSetChanged)
                    {
                        etCity.Text = "";
                        position = -1;

                        adapter.NotifyDataSetChanged();

                        llData.Visibility = ViewStates.Gone;
                        Keyboard.HideKeyboard(this, true);
                    }
                    else
                    {
                        AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);

                        alertDiag.SetTitle("Exists");
                        alertDiag.SetMessage(city.Name + " already exists");

                        alertDiag.SetCancelable(true);

                        alertDiag.SetPositiveButton("OK", (senderAlert, args)
                        =>
                        {
                            alertDiag.Dispose();
                        });

                        Dialog diag = alertDiag.Create();
                        diag.Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, "City not validates", ToastLength.Short).Show();
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            etCity.Text = "";
            position    = -1;
            oldCity     = null;

            llData.Visibility = ViewStates.Gone;
            Keyboard.HideKeyboard(this, true);
        }

        protected override async void OnStop()
        {
            base.OnStop();
            await cities.Save();
        }
    }
}