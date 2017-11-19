﻿using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace AndroidNativeAppWithCSharp
{

    [Activity(Label = "PomodoroClock", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        Button startBtn;
        Button stopBtn;
        TextView pomodoroCntr;
        TextView message;
        TextView three;
        TextView five;
        TextView fifteen;
        TextView thirty;
        CheckBox stopped, running;
        ProgressBar progressBar;


        System.Timers.Timer timer;
        int cntrVal = 10000;
        const int cntrTick = 60000;
        int countMinutes;

        short smallBreak = 3;
        short largeBreak = 15;
        string countdownForWhat = "production";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Initialize components for later use
            message = FindViewById<TextView> (Resource.Id.message);
            message.Text = "Press start to begin countdown";

            startBtn = FindViewById<Button> (Resource.Id.startBtn);
            this.FindViewById<Button>(Resource.Id.startBtn).Click += this.CountdownTime;

            stopBtn = FindViewById<Button> (Resource.Id.stopBtn);

            pomodoroCntr = FindViewById<TextView> (Resource.Id.pomodoroLbl);

            stopped = FindViewById<CheckBox> (Resource.Id.offCheckbox);
            stopped.Checked = true;
            stopped.Enabled = false;

            running = FindViewById<CheckBox> (Resource.Id.runningCheckbox);
            running.Checked = false;
            running.Enabled = false;

            progressBar = FindViewById<ProgressBar> (Resource.Id.progressBar);
            progressBar.Progress = 0;

            three = FindViewById<TextView>(Resource.Id.threeMinLbl);
            this.FindViewById<TextView>(Resource.Id.threeMinLbl).Click += this.UpdateBreakTimePeriods;
            three.SetTextColor(Android.Graphics.Color.DarkOrange);

            five = FindViewById<TextView>(Resource.Id.fiveMinLbl);
            this.FindViewById<TextView>(Resource.Id.fiveMinLbl).Click += this.UpdateBreakTimePeriods;

            fifteen = FindViewById<TextView>(Resource.Id.fifteenMinLbl);
            this.FindViewById<TextView>(Resource.Id.fifteenMinLbl).Click += this.UpdateBreakTimePeriods;
            fifteen.SetTextColor(Android.Graphics.Color.DarkOrange);

            thirty = FindViewById<TextView>(Resource.Id.thirtyMinLbl);
            this.FindViewById<TextView>(Resource.Id.thirtyMinLbl).Click += this.UpdateBreakTimePeriods;
        }

        private void CountdownTime(object sender, EventArgs e)
        {
            timer = new System.Timers.Timer();
            timer.Interval = cntrTick;
            timer.Elapsed += onTimedEvent;
            countMinutes = cntrVal / 10000;
            timer.Enabled = true;
            Toast.MakeText(this, "timer started", ToastLength.Short).Show();
            running.Checked = true;
            stopped.Checked = false;
        }

        /// <summary>
        /// Method called on tick intervals of Timer object.
        /// Reduces the countdown timer and updates the timer view on the layout
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            countMinutes--;

            RunOnUiThread(() =>
            {
                pomodoroCntr.Text = countMinutes.ToString();
            });

            if (countMinutes == 0)
            {
                RunOnUiThread(() =>
                {
                    stopped.Checked = true;
                    running.Checked = false;
                    progressBar.Progress += 1;
                });
                timer.Stop();
                
            }
        }// end of timedEvent()


        /// <summary>
        /// Sets the break time periods for small and large breaks to (3-5) and (15-30) minutes
        /// and updates the ui approprietly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateBreakTimePeriods(object sender, EventArgs e)
        {
            if (countdownForWhat.Equals("break"))
                return;


            if (sender.Equals(three))
            {
                RunOnUiThread(() =>
                {
                    three.SetTextColor(Android.Graphics.Color.DarkOrange);
                    five.SetTextColor(Android.Graphics.Color.Gray);
                });
                smallBreak = 3;
                Toast.MakeText(this, $"Small Break Time is set for {smallBreak} minutes" , ToastLength.Long).Show();
            }
            else if (sender.Equals(five))
            {
                RunOnUiThread(() =>
                {
                    five.SetTextColor(Android.Graphics.Color.DarkOrange);
                    three.SetTextColor(Android.Graphics.Color.Gray);
                });
                smallBreak = 5;
                Toast.MakeText(this, $"Small Break Time is set for {smallBreak} minutes", ToastLength.Long).Show();
            }
            else if (sender.Equals(fifteen))
            {
                RunOnUiThread(() =>
                {
                    fifteen.SetTextColor(Android.Graphics.Color.DarkOrange);
                    thirty.SetTextColor(Android.Graphics.Color.Gray);
                });
                largeBreak = 15;
                Toast.MakeText(this, $"Large Break Time is set for {largeBreak} minutes", ToastLength.Long).Show();
            }
            else if (sender.Equals(thirty))
            {
                RunOnUiThread(() =>
                {
                    thirty.SetTextColor(Android.Graphics.Color.DarkOrange);
                    fifteen.SetTextColor(Android.Graphics.Color.Gray);
                });
                largeBreak = 30;
                Toast.MakeText(this, $"Small Break Time is set for {largeBreak} minutes", ToastLength.Long).Show();
            }

        }// end of updateBreakTimePeriods
    }
}

