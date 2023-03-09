using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9 ]+"); //regex that matches disallowed text
        private TimingHandler th;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
                
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Console.WriteLine(e.Key);
            e.Handled 
                =  e.Key.ToString() == "Space" 
                || e.Key.ToString() == "LeftCtrl" 
                || e.Key.ToString() == "RightCtrl" 
                || e.Key.ToString() == "V"
                || e.Key.ToString() == "Insert";
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (th == null || th.isFinished())
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
                th = new TimingHandler(int.Parse(totalRounds.Text),
                    int.Parse(roundTime.Text), int.Parse(breakTime.Text), int.Parse(longBreakTime.Text),
                    int.Parse(longBreakRound.Text), sender, timerTextBlock);
            }
            string pauseStateStr = ((Button)sender).Content.ToString();
            if (pauseStateStr == "Start")
            {
                ((Button)sender).Content = "Pause";
                th.Start();
            }
            else
            {
                ((Button)sender).Content = "Start";
                th.Pause();
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            submitButton.Content = "Start";
            workBreakTextBlock.Text = "Work";
            th?.Stop();
            totalRounds.Text = "12";
            roundTime.Text = "25";
            breakTime.Text = "5";
            longBreakTime.Text = "20";
            longBreakRound.Text = "4";
            timerTextBlock.Text = (new TimeSpan(0, 25, 0)).ToString(@"mm\:ss");
            th = null;
        }
    }
}
