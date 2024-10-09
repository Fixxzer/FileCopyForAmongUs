using System.ComponentModel;
using System.Diagnostics;

namespace FileCopyForAmongUs
{
    public partial class Form1 : Form
    {
        private static readonly Random Rand = new();

        private const int MaxValue = 100;
        private const int Fast = 100;
        private const int Regular = 200;
        private const int Slow = 1500;
        
        private int randomError;
        private int randomErrorVal;
        private int currRandom;
        private bool hitRandom;
        private int errorTicks;
        private int slowRandom;
        private int slowRandomVal;
        private int fastRandom;
        private int fastRandomVal;

        public Form1()
        {
            InitializeComponent();

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;

            label2.Text = string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            randomError = Rand.Next(0, MaxValue);
            randomErrorVal = Rand.Next(0, 5);
            slowRandom = Rand.Next(0, (MaxValue / 2) - 10);
            slowRandomVal = Rand.Next(1, 10);
            fastRandom = Rand.Next(MaxValue / 2, MaxValue);
            fastRandomVal = Rand.Next(0, 30);
            hitRandom = false;

            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateProgressBar1Safe(0);

            for (var i = 0; i < MaxValue; i++)
            {
                Debug.WriteLine($"Iteration: {i}");
                if (i == randomError && !hitRandom)
                {
                    Debug.WriteLine($"Error: {randomError}");
                    errorTicks = 0;
                    UpdateLabel2Safe("Error!!!");

                    // Reset the current count
                    i = Rand.Next(5, MaxValue - 10);

                    if (currRandom == randomErrorVal)
                    {
                        hitRandom = true;
                    }

                    randomError = Rand.Next(0, MaxValue);
                    currRandom++;
                }

                int timeDelay;
                if (i >= slowRandom && i <= slowRandom + slowRandomVal)
                {
                    Debug.WriteLine($"Slow: {slowRandom} - {slowRandom + slowRandomVal}");
                    timeDelay = Slow;
                }
                else if (i >= fastRandom && i <= fastRandom + fastRandomVal)
                {
                    Debug.WriteLine($"Fast: {fastRandom} - {fastRandom + fastRandomVal}");
                    timeDelay = Fast;
                }
                else
                {
                    Debug.WriteLine("Regular");
                    timeDelay = Regular;
                }

                // Remove "Error!!!" text
                if (errorTicks >= 5)
                {
                    UpdateLabel2Safe(string.Empty);
                    errorTicks = 0;
                }

                UpdateLabel1Safe($@"{i}%");
                UpdateProgressBar1Safe(i);
                errorTicks++;
                Thread.Sleep(timeDelay);
            }

            UpdateProgressBar1Safe(100);
            UpdateLabel1Safe("100%");
            MessageBox.Show("Download Complete!");
        }

        private void UpdateLabel1Safe(string text)
        {
            if (label1.InvokeRequired)
            {
                label1.Invoke(new MethodInvoker(delegate { label1.Text = text; }));
            }
            else
            {
                label1.Text = text;
            }
        }

        private void UpdateLabel2Safe(string text)
        {
            if (label2.InvokeRequired)
            {
                label2.Invoke(new MethodInvoker(delegate { label2.Text = text; }));
            }
            else
            {
                label2.Text = text;
            }
        }

        private void UpdateProgressBar1Safe(int value)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.Invoke(new MethodInvoker(delegate { progressBar1.Value = value; }));
            }
            else
            {
                progressBar1.Value = value;
            }
        }
    }
}
