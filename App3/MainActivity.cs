using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using Android.Runtime;

namespace generator
{
    [Activity(Label = "Генератор вариантов", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private readonly static string STORE_NAME = "AppStore";
        private readonly static string KEY_LABWORK = "LabWork";
        private readonly static string KEY_SUBJECT = "Subject";
        ISharedPreferences settings;


        TextView lastName;
        TextView numberInList;
        TextView dayOfInclude;

        TextView resultVariant;
        RadioGroup LRRadioGroup;
        RadioGroup subjectRadioGroup;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            ((TextView)FindViewById(Resource.Id.TLastNameTextView)).Click += settingsButton_Click;
            ((TextView)FindViewById(Resource.Id.TDayOfIncludeTextView)).Click += settingsButton_Click;
            ((TextView)FindViewById(Resource.Id.TNumberTextView)).Click += settingsButton_Click;

            //((Button)FindViewById(Resource.Id.GenerateButton)).Click += calculateButton_Click;

            settings = GetSharedPreferences(STORE_NAME, FileCreationMode.Private);

            lastName = (TextView)FindViewById(Resource.Id.TLastNameTextView);
            numberInList = (TextView)FindViewById(Resource.Id.TNumberTextView);
            dayOfInclude = (TextView)FindViewById(Resource.Id.TDayOfIncludeTextView);

            resultVariant = (TextView)FindViewById(Resource.Id.resultVariantTextView);
            LRRadioGroup = (RadioGroup)FindViewById(Resource.Id.LRGroup);
            subjectRadioGroup = (RadioGroup)FindViewById(Resource.Id.subjectGroup);
            ((RadioButton)FindViewById(Resource.Id.Sub1RadioButton)).Click += calculateButton_Click;
            ((RadioButton)FindViewById(Resource.Id.Sub2RadioButton)).Click += calculateButton_Click;
            ((RadioButton)FindViewById(Resource.Id.Sub3RadioButton)).Click += calculateButton_Click;
            ((RadioButton)FindViewById(Resource.Id.Sub4RadioButton)).Click += calculateButton_Click;
            ((RadioButton)FindViewById(Resource.Id.LR1RadioButton)).Click += calculateButton_Click;
            ((RadioButton)FindViewById(Resource.Id.LR2RadioButton)).Click += calculateButton_Click;
            ((RadioButton)FindViewById(Resource.Id.LR3RadioButton)).Click += calculateButton_Click;
            ((RadioButton)FindViewById(Resource.Id.LR4RadioButton)).Click += calculateButton_Click;
            LRRadioGroup.Click += calculateButton_Click;
            subjectRadioGroup.Click += calculateButton_Click;
        }
        protected override void OnStart()
        {
            base.OnStart();
            try
            {
                subjectRadioGroup.ClearCheck();
                subjectRadioGroup.Check(int.Parse(settings.GetInt(KEY_SUBJECT, Resource.Id.Sub1RadioButton).ToString()));
                LRRadioGroup.ClearCheck();
                LRRadioGroup.Check(int.Parse(settings.GetInt(KEY_LABWORK, Resource.Id.LR1RadioButton).ToString()));
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                settingsButton_Click(this, null);
            }
            
        }
        protected override void OnResume()
        {
            try
            {
                base.OnResume();
                calculateButton_Click(null, null);
                lastName.Text = GetString(Resource.String.TLastName) + " " + settings.GetString(SettingsActivity.KEY_LASTNAME, "");
                int mounthOfInclude = settings.GetBoolean(SettingsActivity.KEY_MOUNTH, false) ? 2 : 9;
                string yearOfInclude = settings.GetString(SettingsActivity.KEY_YEAR, "");
                if (yearOfInclude.CompareTo("") == 0)
                    throw new Exception();
                dayOfInclude.Text = GetString(Resource.String.TDayOfInclude) + " " + mounthOfInclude + "." + yearOfInclude;
                numberInList.Text = GetString(Resource.String.TNumber) + " " + settings.GetString(SettingsActivity.KEY_NUMBER, "");
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
                settingsButton_Click(this, null);
            }
        }
        protected override void OnStop()
        {
            base.OnStop();     
            var editor = settings.Edit();
            editor.PutInt(KEY_SUBJECT, subjectRadioGroup.CheckedRadioButtonId);
            editor.PutInt(KEY_LABWORK, LRRadioGroup.CheckedRadioButtonId);
            editor.Commit();
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            calculateButton_Click(null, null);
        }

        private void settingsButton_Click(object sender, System.EventArgs e)
        {
            Intent test = new Intent(this, SettingsActivity.Type());
            //StartActivity(test);
            StartActivityForResult(test, 0);
        }
        private void calculateButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                string variant = GetString(Resource.String.Variant);
                switch (subjectRadioGroup.CheckedRadioButtonId)
                {
                    case Resource.Id.Sub1RadioButton:
                    case Resource.Id.Sub2RadioButton:
                        Mounth mounth = (settings.GetBoolean(SettingsActivity.KEY_MOUNTH,false)) ? (Mounth.February) : (Mounth.September);
                        int year = int.Parse(settings.GetString(SettingsActivity.KEY_YEAR,"0"));
                        int numberInList = int.Parse(settings.GetString(SettingsActivity.KEY_NUMBER,"0"));
                        if (subjectRadioGroup.CheckedRadioButtonId == Resource.Id.Sub2RadioButton)
                            numberInList += 30;
                        int numberLabWork=0;
                        switch(LRRadioGroup.CheckedRadioButtonId)
                        {
                            case Resource.Id.LR1RadioButton:
                                numberLabWork = 1;
                                break;
                            case Resource.Id.LR2RadioButton:
                                numberLabWork = 2;
                                break;
                            case Resource.Id.LR3RadioButton:
                                numberLabWork = 3;
                                break;
                            case Resource.Id.LR4RadioButton:
                                numberLabWork = 4;
                                break;
                        }
                        variant+=Varianter.GetVariantForInformatic(mounth, year, numberInList, numberLabWork).ToString();
                        break;
                    case Resource.Id.Sub3RadioButton:
                    case Resource.Id.Sub4RadioButton:
                        variant += Varianter.GetVariantForOther(settings.GetString(SettingsActivity.KEY_LASTNAME, ""));
                        break;

                }
                resultVariant.Text = variant;
            }
            catch (Exception ex)
            {
                resultVariant.Text = GetString(Resource.String.Error);
                Toast.MakeText(this, ex.Message, ToastLength.Short).Show();
            }
        }
    }
}

