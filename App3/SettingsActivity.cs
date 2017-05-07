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

namespace generator
{
    [Activity(Label = "Настройки", MainLauncher = true, Icon = "@drawable/icon")]
    class SettingsActivity : Activity
    {
        public static Type Type()
        {
            SettingsActivity test=new SettingsActivity();
            return test.GetType();
        }
        private readonly static string STORE_NAME = "AppStore";
        public readonly static string KEY_YEAR = "Year";
        public readonly static string KEY_MOUNTH = "Mounth";
        public readonly static string KEY_NUMBER = "Number";
        public readonly static string KEY_LASTNAME = "Lastname";

        ISharedPreferences settings;

        TextView yearText;
        TextView numberText;
        TextView lastNameText;
        RadioButton februarRB;
        RadioButton septemberRB;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Settings);

            settings = GetSharedPreferences(STORE_NAME, FileCreationMode.Private);

            ((Button)FindViewById(Resource.Id.saveButton)).Click += saveButton_Click;

            yearText = (TextView)FindViewById(Resource.Id.yearText);
            numberText = (TextView)FindViewById(Resource.Id.numberText);
            lastNameText = (TextView)FindViewById(Resource.Id.lastnameEditText);
            februarRB = (RadioButton)FindViewById(Resource.Id.februaryRadioButton);
            septemberRB = (RadioButton)FindViewById(Resource.Id.septemberRadioButton);
        }

        protected override void OnStart()
        {
            base.OnStart();
            yearText.Text = settings.GetString(KEY_YEAR, DateTime.Now.Year.ToString());
            numberText.Text = settings.GetString(KEY_NUMBER, "1");
            lastNameText.Text = settings.GetString(KEY_LASTNAME, "");
            bool isFebruaryStream = (DateTime.Now.Month >= 2 && DateTime.Now.Month <= 9); // если февральский поток стартанул
            februarRB.Checked = settings.GetBoolean(KEY_MOUNTH, isFebruaryStream);
            septemberRB.Checked = !februarRB.Checked;
        }
        private void saveButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                Mounth mounth = (februarRB.Checked) ? (Mounth.February) : (Mounth.September);
                var editor = settings.Edit();
                editor.PutBoolean(KEY_MOUNTH, februarRB.Checked);
                editor.PutString(KEY_YEAR, yearText.Text);
                editor.PutString(KEY_NUMBER, numberText.Text);
                editor.PutString(KEY_LASTNAME, lastNameText.Text);
                editor.Commit();
                Toast.MakeText(this, Resource.String.Saved, ToastLength.Short).Show();
                this.Finish();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }
    }
}