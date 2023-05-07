using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPFUI
{
    class TimingHandler
    {
        MainWindow Form = App.Current.Windows[0] as MainWindow;
        private bool finished, workRound;
        private int currentTimer, currentRound, totalRounds, roundDuration,
            breakDuration, longBreakDuration, longBreakRound;
        private string timeFmt;
        private TimeSpan ts;
        private DispatcherTimer dt;

        public TimingHandler(
            int totalRounds,
            int roundDuration, 
            int breakDuration, 
            int longBreakDuration, 
            int longBreakRound
            )
        {
            this.totalRounds        = totalRounds;
            this.roundDuration      = roundDuration * 60;
            this.breakDuration      = breakDuration * 60;
            this.longBreakDuration  = longBreakDuration * 60;
            this.longBreakRound     = longBreakRound;
            finished = false;
            workRound = true;
            currentRound = 0;
            currentTimer = this.roundDuration;
            // Update label with work time
            timeFmt = @"mm\:ss";
            ts = new TimeSpan(0, roundDuration, 0);
            Form.timerTextBlock.Text = ts.ToString(timeFmt);
        }

        private void UpdateLabel()
        {
            // TODO: Decouple from the Form.
            Form.timerTextBlock.Text = (new TimeSpan(0, currentTimer / 60, currentTimer % 60)).ToString(timeFmt);
            CommandManager.InvalidateRequerySuggested();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            // TODO: Use the finished boolean here; alternatively refactor to use method IsFinished. 
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
                dt.Stop();
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
            if (dt == null)
            {
                dt = new System.Windows.Threading.DispatcherTimer();
                dt.Tick += new EventHandler(DispatcherTimer_Tick);
                dt.Interval = TimeSpan.FromSeconds(1.0d);
            }
            dt.Start();
            finished = false;
        }

        public void Pause()
        {
            dt.Stop();
        }

        public void Stop()
        {
            dt.Stop();
            dt = null;
            finished = true;
        }
    }
}
