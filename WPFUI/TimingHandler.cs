using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WPFUI
{
    class TimingHandler
    {
        MainWindow Form = App.Current.Windows[0] as MainWindow;
        private bool finished, workRound;
        private int currentTimer, currentRound, totalRounds, roundDuration,
            breakDuration, longBreakDuration, longBreakRound;
        private object button, textBlock;
        private string timeFmt;
        private TimeSpan ts;
        private DispatcherTimer dt;

        public TimingHandler(
            int totalRounds,
            int roundDuration, 
            int breakDuration, 
            int longBreakDuration, 
            int longBreakRound,
            object button,
            object textBlock
            )
        {
            this.totalRounds        = totalRounds;
            this.roundDuration      = roundDuration * 60;
            this.breakDuration      = breakDuration * 60;
            this.longBreakDuration  = longBreakDuration * 60;
            this.longBreakRound     = longBreakRound;
            this.button             = button;
            this.textBlock          = textBlock;
            finished = false;
            workRound = true;
            currentRound = 0;
            currentTimer = this.roundDuration;
            // Update label with work time
            timeFmt = @"mm\:ss";
            ts = new TimeSpan(0, roundDuration, 0);
            Form.timerTextBlock.Text = ts.ToString(timeFmt);
        }

        private void updateLabel()
        {
            Form.timerTextBlock.Text = (new TimeSpan(0, currentTimer / 60, currentTimer % 60)).ToString(timeFmt);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
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
                        workRound = !workRound;
                        currentTimer = (1 + currentRound) % longBreakRound != 0 ? breakDuration : longBreakDuration;
                        Form.workBreakTextBlock.Text = "Break";
                        // Play "break time" sound
                        System.Media.SystemSounds.Asterisk.Play();
                        // Restore window
                        //Form.WindowState = System.Windows.WindowState.Normal;
                    } 
                    else
                    {
                        workRound = !workRound;
                        currentRound++;
                        currentTimer = roundDuration;
                        Form.workBreakTextBlock.Text = "Work";
                        // Play "back to work" sound
                        System.Media.SystemSounds.Asterisk.Play();
                        // Maximize winow to block
                        //Form.Activate();
                        //Form.WindowState = System.Windows.WindowState.Maximized;
                    }
                }

            } 
            else
            {
                // Done
                dt.Stop();
                finished = true;
                // Revert Start button
                Form.submitButton.Content = "Start";
            }
            updateLabel();
        }

        public bool isFinished()
        {
            return finished;
        }

        public void Start()
        {
            if (dt == null)
            {
                dt = new System.Windows.Threading.DispatcherTimer();
                dt.Tick += new EventHandler(dispatcherTimer_Tick);
                dt.Interval = TimeSpan.FromSeconds(1);
            }
            dt.Start();
            finished = false;
            //Form.Activate();
            //Form.WindowState = System.Windows.WindowState.Maximized;
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
