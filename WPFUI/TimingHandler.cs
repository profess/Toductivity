using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace Toductivity
{
    class TimingHandler : IDisposable
    {
        // TODO: stop using Form directly
        MainWindow Form = System.Windows.Application.Current.Windows[0] as MainWindow;
        private bool finished, workRound;
        private int currentTimer, currentRound, totalRounds, roundDuration,
            breakDuration, longBreakDuration, longBreakRound;
        private readonly string timeFmt;
        private TimeSpan MyTimeSpan;
        private DispatcherTimer MyDispatcherTimer;

        public int CurrentTimer { get => currentTimer; set => currentTimer = value; }
        public int CurrentRound { get => currentRound; set => currentRound = value; }
        public int TotalRounds { get => totalRounds; set => totalRounds = value; }
        public int RoundDuration { get => roundDuration; set => roundDuration = value; }
        public int BreakDuration { get => breakDuration; set => breakDuration = value; }
        public int LongBreakDuration { get => longBreakDuration; set => longBreakDuration = value; }
        public int LongBreakRound { get => longBreakRound; set => longBreakRound = value; }

        public TimingHandler(
            int totalRounds,
            int roundDuration,
            int breakDuration,
            int longBreakDuration,
            int longBreakRound
            )
        {
<<<<<<< HEAD
            this.totalRounds = totalRounds;
            this.roundDuration = roundDuration * 60;
            this.breakDuration = breakDuration * 60;
            this.longBreakDuration = longBreakDuration * 60;
            this.longBreakRound = longBreakRound;
=======
            this.totalRounds        = totalRounds;
            this.roundDuration      = roundDuration * 60;
            this.breakDuration      = breakDuration * 60;
            this.longBreakDuration  = longBreakDuration * 60;
            this.longBreakRound     = longBreakRound;

>>>>>>> Added disposer
            finished = false;
            workRound = true;
            currentRound = 0;
            currentTimer = this.roundDuration;
            // Update label with work time
            timeFmt = @"mm\:ss";
            MyTimeSpan = new TimeSpan(0, roundDuration, 0);
            Form.timerTextBlock.Text = MyTimeSpan.ToString(timeFmt);
        }

        private void UpdateLabel()
        {
            // TODO: Decouple from the Form.
            Form.timerTextBlock.Text = (new TimeSpan(0, currentTimer / 60, currentTimer % 60)).ToString(timeFmt);
            CommandManager.InvalidateRequerySuggested();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            // TODO: Use the 'finished' boolean here; alternatively refactor to use method IsFinished. 
            if (currentRound < totalRounds)
            {
                if (currentTimer > 0)
                {
                    currentTimer--;
                }
                else
                {
                    if (workRound)
                    {
                        currentTimer = (1 + currentRound) % longBreakRound != 0 ? breakDuration : longBreakDuration;
                        Form.workBreakTextBlock.Text = "Break";
                    }
                    else
                    {
                        currentRound++;
                        currentTimer = roundDuration;
                        Form.workBreakTextBlock.Text = "Work";
                    }
                    workRound = !workRound;
                    System.Media.SystemSounds.Asterisk.Play();
                }
            }
            else
            {
                // Done
                MyDispatcherTimer.Stop();
                finished = true;
                // TODO: Move this to MainWindow.cs
                // Revert Start button after everything is done.
                Form.submitButton.Content = "Start";
            }
            UpdateLabel();
        }

        public bool IsFinished()
        {
            return finished;
        }

        public void Start()
        {
            if (MyDispatcherTimer == null)
            {
                MyDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                MyDispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
                MyDispatcherTimer.Interval = TimeSpan.FromSeconds(1.0d);
            }
            MyDispatcherTimer.Start();
            finished = false;
        }

        public void Pause()
        {
            MyDispatcherTimer.Stop();
        }

        public void Stop()
        {
            MyDispatcherTimer.Stop();
            MyDispatcherTimer = null;
            finished = true;
        }

        public void Dispose()
        {
            MyDispatcherTimer.Stop();
            MyDispatcherTimer = null;
        }
    }
}
