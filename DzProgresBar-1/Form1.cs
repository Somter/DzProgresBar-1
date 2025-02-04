using System;
using System.Threading;
using System.Windows.Forms;

namespace DzProgresBar_1
{
    public partial class Form1 : Form
    {
        private ManualResetEvent event_for_suspend1 = new ManualResetEvent(true);
        private ManualResetEvent event_for_stop1 = new ManualResetEvent(false);
        private Thread firstThread;

        private ManualResetEvent event_for_suspend2 = new ManualResetEvent(true);
        private ManualResetEvent event_for_stop2 = new ManualResetEvent(false);
        private Thread secondThread;

        private ManualResetEvent event_for_suspend3 = new ManualResetEvent(true);
        private ManualResetEvent event_for_stop3 = new ManualResetEvent(false);
        private Thread thirdThread;

        public SynchronizationContext uiContext;
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            uiContext = SynchronizationContext.Current;

            checkBox1.Text = "Запустить 1-й поток";
            checkBox2.Text = "Приостановить 1-й поток";

            checkBox4.Text = "Запустить 2-й поток";
            checkBox3.Text = "Приостановить 2-й поток";

            checkBox6.Text = "Запустить 3-й поток";
            checkBox5.Text = "Приостановить 3-й поток";

            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            checkBox5.Enabled = false;
        }

        private void ThreadFunc()
        {
            uiContext.Send(d => progressBar1.Minimum = 0, null);
            uiContext.Send(d => progressBar1.Maximum = 230, null);
            uiContext.Send(d => progressBar1.Value = 0, null);

            while (true)
            {
                if (event_for_stop1.WaitOne(0)) break;

                if (event_for_suspend1.WaitOne(0))
                {
                    int randomValue = random.Next(0, 231);
                    uiContext.Send(d => progressBar1.Value = randomValue, null);
                }

                Thread.Sleep(100);
            }

            uiContext.Send(d => progressBar1.Value = 0, null);
        }

        private void ThreadFunc2()
        {
            uiContext.Send(d => progressBar2.Minimum = 0, null);
            uiContext.Send(d => progressBar2.Maximum = 230, null);
            uiContext.Send(d => progressBar2.Value = 0, null);

            while (true)
            {
                if (event_for_stop2.WaitOne(0)) break;

                if (event_for_suspend2.WaitOne(0))
                {
                    int randomValue = random.Next(0, 231);
                    uiContext.Send(d => progressBar2.Value = randomValue, null);
                }

                Thread.Sleep(100);
            }

            uiContext.Send(d => progressBar2.Value = 0, null);
        }

        private void ThreadFunc3()
        {
            uiContext.Send(d => progressBar3.Minimum = 0, null);
            uiContext.Send(d => progressBar3.Maximum = 230, null);
            uiContext.Send(d => progressBar3.Value = 0, null);

            while (true)
            {
                if (event_for_stop3.WaitOne(0)) break;

                if (event_for_suspend3.WaitOne(0))
                {
                    int randomValue = random.Next(0, 231);
                    uiContext.Send(d => progressBar3.Value = randomValue, null);
                }

                Thread.Sleep(100);
            }

            uiContext.Send(d => progressBar3.Value = 0, null);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                event_for_stop1.Reset();

                if (firstThread == null || !firstThread.IsAlive)
                {
                    firstThread = new Thread(ThreadFunc);
                    firstThread.IsBackground = true;
                    firstThread.Start();
                }

                checkBox1.Text = "Остановить 1-й поток";
                checkBox2.Enabled = true;
            }
            else
            {
                event_for_stop1.Set();
                event_for_suspend1.Set();

                checkBox1.Text = "Запустить 1-й поток";
                checkBox2.Checked = false;
                checkBox2.Text = "Приостановить 1-й поток";
                checkBox2.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                event_for_suspend1.Reset();
                checkBox2.Text = "Возобновить 1-й поток";
            }
            else
            {
                event_for_suspend1.Set();
                checkBox2.Text = "Приостановить 1-й поток";
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                event_for_stop2.Reset();

                if (secondThread == null || !secondThread.IsAlive)
                {
                    secondThread = new Thread(ThreadFunc2);
                    secondThread.IsBackground = true;
                    secondThread.Start();
                }

                checkBox4.Text = "Остановить 2-й поток";
                checkBox3.Enabled = true;
            }
            else
            {
                event_for_stop2.Set();
                event_for_suspend2.Set();

                checkBox4.Text = "Запустить 2-й поток";
                checkBox3.Checked = false;
                checkBox3.Text = "Приостановить 2-й поток";
                checkBox3.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                event_for_suspend2.Reset();
                checkBox3.Text = "Возобновить 2-й поток";
            }
            else
            {
                event_for_suspend2.Set();
                checkBox3.Text = "Приостановить 2-й поток";
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                event_for_stop3.Reset();

                if (thirdThread == null || !thirdThread.IsAlive)
                {
                    thirdThread = new Thread(ThreadFunc3);
                    thirdThread.IsBackground = true;
                    thirdThread.Start();
                }

                checkBox6.Text = "Остановить 3-й поток";
                checkBox5.Enabled = true;
            }
            else
            {
                event_for_stop3.Set();
                event_for_suspend3.Set();

                checkBox6.Text = "Запустить 3-й поток";
                checkBox5.Checked = false;
                checkBox5.Text = "Приостановить 3-й поток";
                checkBox5.Enabled = false;
            }
        }

    
        private void checkBox5_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox5.Checked)
            {
                event_for_suspend3.Reset();
                checkBox5.Text = "Возобновить 3-й поток";
            }
            else
            {
                event_for_suspend3.Set();
                checkBox5.Text = "Приостановить 3-й поток";
            }
        }
    }
}
