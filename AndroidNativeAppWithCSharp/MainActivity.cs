using Android.App;
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
        CheckBox stopped, running;


        System.Timers.Timer timer;
        int cntrVal = 250000;
        const int cntrTick = 60000;
        int countMinutes;

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
            running = FindViewById<CheckBox> (Resource.Id.runningCheckbox);
            running.Checked = false;
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
            pomodoroCntr.Text = countMinutes.ToString();

            if (countMinutes == 0)
            {
                timer.Stop();
            }
        }
    }
}

