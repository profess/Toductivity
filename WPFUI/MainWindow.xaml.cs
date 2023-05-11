using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Toductivity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Regex MatchingRegex = new Regex("[0-9]+"); //regex that matches allowed text
        private TimingHandler MyTimingHandler;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            return !MatchingRegex.IsMatch(text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled
                = e.Key.ToString() == "Space"
                || e.Key.ToString() == "LeftCtrl"
                || e.Key.ToString() == "RightCtrl"
                || e.Key.ToString() == "V"
                || e.Key.ToString() == "Insert";
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshTimingHandler();
            string pauseStateStr = ((Button)sender).Content.ToString();
            if (pauseStateStr == "Start")
            {
                ((Button)sender).Content = "Pause";
                MyTimingHandler.Start();
            }
            else
            {
                ((Button)sender).Content = "Start";
                MyTimingHandler.Pause();
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            submitButton.Content = "Start";
            workBreakTextBlock.Text = "Work";
            MyTimingHandler?.Stop();
            totalRounds.Text = "12";
            roundTime.Text = "25";
            breakTime.Text = "5";
            longBreakTime.Text = "20";
            longBreakRound.Text = "4";
            timerTextBlock.Text = (new TimeSpan(0, 25, 0)).ToString(@"mm\:ss");
            MyTimingHandler = null;
        }

        private void LoadDefaultsIfFieldEmpty()
        {
            if (totalRounds.Text == "")
            {
                totalRounds.Text = "12";
            }
            if (roundTime.Text == "")
            {
                roundTime.Text = "25";
            }
            if (breakTime.Text == "")
            {
                breakTime.Text = "5";
            }
            if (longBreakTime.Text == "")
            {
                longBreakTime.Text = "20";
            }
            if (longBreakRound.Text == "")
            {
                longBreakRound.Text = "4";
            }
        }

        private void RefreshTimingHandler()
        {
            if (MyTimingHandler == null || MyTimingHandler.IsFinished())
            {
                LoadDefaultsIfFieldEmpty();
                MyTimingHandler = new TimingHandler(
                    int.Parse(totalRounds.Text),
                    int.Parse(roundTime.Text),
                    int.Parse(breakTime.Text),
                    int.Parse(longBreakTime.Text),
                    int.Parse(longBreakRound.Text)
                    );
            }
        }
    }
}
