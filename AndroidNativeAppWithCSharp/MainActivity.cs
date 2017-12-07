using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Media;
using Android.Content.Res;

namespace AndroidNativeAppWithCSharp
{

    [Activity(Label = "PomodoroClock", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        Button startBtn;
        Button stopBtn;
        TextView pomodoroCntr;
        TextView message;
        TextView progressMessage;
        TextView three;
        TextView five;
        TextView fifteen;
        TextView thirty;
        CheckBox stopped, running;
        ProgressBar progressBar;
        MediaPlayer mediaPlayer;


        System.Timers.Timer timer;
        int cntrVal = 20000;
        const int cntrTick = 60000;
        int countMinutes;
        int tempTime = -1;

        short smallBreak = 3;
        short largeBreak = 15;
        string countdownForWhat = "production";

        private enum btnText
        {
            START = 1,
            STOP = 2,
            CONTINUE = 3,
            RESET = 4
        };

        private const string startSound = "beep.mp3";
        private const string beginCountdownSound = "arpeggio.mp3";
        private const string endOfCountdownSound = "what-friends-are-for.mp3";


        private const string INITIAL_MSG = "Press start to begin countdown";
        private const string PRODUCTION_COUNTDOWN_MSG = "Counting down production time";
        private const string SMALLBREAK_COUNTDOWN_MSG = "Counting down small break time";
        private const string LARGEBREAK_COUNTDOWN_MSG = "Counting down large break time";
        private const string BEFORE_SMALLBREAK_COUNTDOWN_MSG = "Press start to begin small break countdown";
        private const string BEFORE_LARGEBREAK_COUNTDOWN_MSG = "Press start to begin large break countdown";
        private const string PROGRESS_DEFAULT_MSG = "No productive periods passed yet";
        private const string PROGRESS_ALL_MSG = "Finally it is time for the large break!";
        private const string STOP_MESSAGE = "Coutdown has stopped";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            // Initialize components for later use
            message = FindViewById<TextView> (Resource.Id.message);
            message.Text = INITIAL_MSG;

            progressMessage = FindViewById<TextView>(Resource.Id.progressMessage);
            progressMessage.Text = PROGRESS_DEFAULT_MSG;
            progressMessage.SetTextColor(Android.Graphics.Color.LightGreen);

            startBtn = FindViewById<Button> (Resource.Id.startBtn);
            this.FindViewById<Button>(Resource.Id.startBtn).Click += this.CountdownTime;

            stopBtn = FindViewById<Button> (Resource.Id.stopBtn);
            this.FindViewById<Button>(Resource.Id.stopBtn).Click += this.StopCountdown;

            pomodoroCntr = FindViewById<TextView> (Resource.Id.pomodoroLbl);
            pomodoroCntr.Text = (cntrVal / 10000).ToString();

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

            mediaPlayer = new MediaPlayer();
        }

        private void CountdownTime(object sender, EventArgs e)
        {

            RunOnUiThread(() =>
            {
                stopBtn.Text = btnText.STOP.ToString();
                stopBtn.Enabled = true;
                startBtn.Enabled = false;
                playAudioFile(beginCountdownSound);
            });

            tempTime = -1;
            timer = new System.Timers.Timer();
            timer.Interval = cntrTick;
            timer.Elapsed += onTimedEvent;

            if (countdownForWhat.Equals("production"))
            {
                RunOnUiThread(() =>
                {
                    message.Text = PRODUCTION_COUNTDOWN_MSG;
                });

                if (tempTime == -1)
                {
                    countMinutes = cntrVal / 10000;

                    RunOnUiThread(() =>
                    {
                        pomodoroCntr.Text = countMinutes.ToString();
                    });
                }
                
                timer.Enabled = true;
                Toast.MakeText(this, "timer for production started", ToastLength.Short).Show();
                running.Checked = true;
                stopped.Checked = false;
            }
            else if(countdownForWhat.Equals("smallBreak"))
            {
                RunOnUiThread(() =>
                {
                    message.Text = SMALLBREAK_COUNTDOWN_MSG;
                });

                if (tempTime == -1)
                {
                    countMinutes = (smallBreak * 10000) / 10000;

                    RunOnUiThread(() =>
                    {
                        pomodoroCntr.Text = countMinutes.ToString();
                    });
                }

                timer.Enabled = true;
                Toast.MakeText(this, "timer for small break started", ToastLength.Short).Show();
                running.Checked = true;
                stopped.Checked = false;
            }
            else
            {
                RunOnUiThread(() =>
                {
                    message.Text = LARGEBREAK_COUNTDOWN_MSG;
                });

                if (tempTime == -1)
                {
                    countMinutes = (largeBreak * 10000) / 10000;

                    RunOnUiThread(() =>
                    {
                        pomodoroCntr.Text = countMinutes.ToString();
                    });
                }

                timer.Enabled = true;
                Toast.MakeText(this, "timer for large break started", ToastLength.Short).Show();
                running.Checked = true;
                stopped.Checked = false;
            }
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
                playAudioFile(startSound);
            });

            if (countMinutes == 0)
            {

                RunOnUiThread(() =>
                {
                    stopped.Checked = true;
                    running.Checked = false;
                    startBtn.Enabled = true;
                    stopBtn.Enabled = false;
                    startBtn.Text = btnText.START.ToString();
                    progressBar.Progress += 1;
                    message.Text = (progressBar.Progress < 4) ? BEFORE_SMALLBREAK_COUNTDOWN_MSG : BEFORE_LARGEBREAK_COUNTDOWN_MSG;
                    progressMessage.Text = (progressBar.Progress < 4)
                    ? $"{progressBar.Progress} periods have passed - {4 - progressBar.Progress} remain" : PROGRESS_ALL_MSG;

                    if (countdownForWhat.Equals("production"))
                    {
                        countdownForWhat = (progressBar.Progress < 4) ? "smallBreak" : "largeBreak";
                    }
                    else
                        countdownForWhat = "production";

                    playAudioFile(endOfCountdownSound);

                });


                timer.Stop();
                
            }
        }// end of timedEvent()


        /// <summary>
        /// Method called on stop button click.
        /// If btn text is 'stop', stops the timer, sets the count minutes
        /// and updates the ui appropriately.
        /// If btn text is 'reset', resets tempTime and sets the countMinutes appropriately
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopCountdown(Object sender, EventArgs e)
        {
            if (stopBtn.Text.Equals(btnText.STOP.ToString()))
            {
                timer.Stop();

                tempTime = Convert.ToInt32(pomodoroCntr.Text);

                countMinutes = tempTime * 10000;

                RunOnUiThread(() =>
                {
                    stopped.Checked = true;
                    running.Checked = false;
                    startBtn.Enabled = true;
                    startBtn.Text = btnText.CONTINUE.ToString();
                    stopBtn.Text = btnText.RESET.ToString();
                    message.Text = STOP_MESSAGE;
                });
            }
            else if (stopBtn.Text.Equals(btnText.RESET.ToString()))
            {
                tempTime = -1;
                RunOnUiThread(() =>
                {
                    startBtn.Text = btnText.START.ToString();
                    stopBtn.Text = btnText.STOP.ToString();
                });

                switch (countdownForWhat)
                {
                    case "production":
                        countMinutes = 250000;
                        message.Text = INITIAL_MSG;
                        break;
                    case "smallBreak":
                        countMinutes = smallBreak * 10000;
                        message.Text = BEFORE_SMALLBREAK_COUNTDOWN_MSG;
                        break;
                    case "largeBreak":
                        countMinutes = largeBreak * 10000;
                        message.Text = BEFORE_LARGEBREAK_COUNTDOWN_MSG;
                        break;
                }
            }

        }// end of stopCountdown()


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
                Toast.MakeText(this, $"Small Break Time is set for {smallBreak} minutes" , ToastLength.Short).Show();
            }
            else if (sender.Equals(five))
            {
                RunOnUiThread(() =>
                {
                    five.SetTextColor(Android.Graphics.Color.DarkOrange);
                    three.SetTextColor(Android.Graphics.Color.Gray);
                });
                smallBreak = 5;
                Toast.MakeText(this, $"Small Break Time is set for {smallBreak} minutes", ToastLength.Short).Show();
            }
            else if (sender.Equals(fifteen))
            {
                RunOnUiThread(() =>
                {
                    fifteen.SetTextColor(Android.Graphics.Color.DarkOrange);
                    thirty.SetTextColor(Android.Graphics.Color.Gray);
                });
                largeBreak = 15;
                Toast.MakeText(this, $"Large Break Time is set for {largeBreak} minutes", ToastLength.Short).Show();
            }
            else if (sender.Equals(thirty))
            {
                RunOnUiThread(() =>
                {
                    thirty.SetTextColor(Android.Graphics.Color.DarkOrange);
                    fifteen.SetTextColor(Android.Graphics.Color.Gray);
                });
                largeBreak = 30;
                Toast.MakeText(this, $"Large Break Time is set for {largeBreak} minutes", ToastLength.Short).Show();
            }

        }// end of updateBreakTimePeriods


        /// <summary>
        /// Plays audio files thar are located in assets folder
        /// </summary>
        /// <param name="file">the filename e.g "name.mp3"</param>
        private void playAudioFile(string file)
        {
            AssetFileDescriptor descriptor = Assets.OpenFd(file);
            if (mediaPlayer.IsPlaying == true)
            {
                mediaPlayer.Stop();
                mediaPlayer.Reset();
            }
            else
            {
                mediaPlayer.Reset();
            }

            mediaPlayer.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
            mediaPlayer.Prepare();
            mediaPlayer.Start();
        }

    }// end of main ativity
}

