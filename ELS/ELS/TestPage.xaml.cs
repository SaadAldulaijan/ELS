using ELS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ELS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage, INotifyPropertyChanged
    {
        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        #region From Json
        public ObservableCollection<District> Districts { get; set; }
        public ObservableCollection<SubDistrict> SubDistricts { get; set; }
        public ObservableCollection<House> Houses { get; set; }


        private District _selectedDistrict;

        public District SelectedDistrict
        {
            get { return _selectedDistrict; }
            set { _selectedDistrict = value; }
        }

        private SubDistrict _selectedSubDistrict;

        public SubDistrict SelectedSubDistrict
        {
            get { return _selectedSubDistrict; }
            set { _selectedSubDistrict = value; }
        }

        private House _selectedHouse;

        public House SelectedHouse
        {
            get { return _selectedHouse; }
            set { _selectedHouse = value; }
        }


        #endregion

        #region Error Handling 
        // TODO: These guys need on property changed event
        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }

        // TODO: this guy is never been used.
        private string _errMessage;

        public string ErrMessage
        {
            get { return _errMessage; }
            set { _errMessage = value; OnPropertyChanged(nameof(ErrMessage)); }
        }

        private bool _canClick;

        public bool CanClick
        {
            get { return _canClick; }
            set { _canClick = value; OnPropertyChanged(nameof(CanClick)); }
        }

        private string _mapUrl;

        public string MapUrl
        {
            get { return _mapUrl; }
            set { _mapUrl = value; }
        }


        #endregion
        public TestPage()
        {
            InitializeComponent();
            BindingContext = this;
            PopulateDistricts();
            CanClick = false;
        }

        private void PopulateDistricts()
        {
            var assembly = typeof(TestPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("ELS.Data.District.json");

            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();

                List<District> districts = JsonConvert.DeserializeObject<List<District>>(json);
                Districts = new ObservableCollection<District>(districts);
                districtPicker.ItemsSource = Districts;
            }
        }

        private void PopulateSubDistricts(int districtId)
        {
            var assembly = typeof(TestPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("ELS.Data.SubDistrict.json");

            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();

                List<SubDistrict> subDistricts = JsonConvert.DeserializeObject<List<SubDistrict>>(json);
                SubDistricts = new ObservableCollection<SubDistrict>(subDistricts);
                subDistrictPicker.ItemsSource = SubDistricts.Where(x => x.DistrictId == districtId).ToList();
            }
        }


        private void PopulateHouses(int subDistrictId)
        {
            var assembly = typeof(TestPage).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("ELS.Data.House.json");

            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();

                List<House> houses = JsonConvert.DeserializeObject<List<House>>(json);
                Houses = new ObservableCollection<House>(houses);
                var dataSource = Houses.Where(x => x.SubDistrictId == subDistrictId).ToList();
                if (dataSource.Count() == 0)
                {
                    Message = $"There is no available houses in {SelectedSubDistrict.Name}";
                }
                else
                {
                    Message = null;
                }
                housePicker.ItemsSource = dataSource;
            }
        }
        private void districtPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateSubDistricts(SelectedDistrict.Id);
        }

        private void subDistrictPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedSubDistrict == null)
            {
                housePicker.ItemsSource = null;
            }
            else
            {
                PopulateHouses(SelectedSubDistrict.Id);
            }
        }

        private void housePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get the link and open google map
            if (SelectedHouse != null)
            {
                CanClick = true;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            MapUrl = SelectedHouse.Location;
            Launcher.OpenAsync(MapUrl).Wait();
        }
    }
}