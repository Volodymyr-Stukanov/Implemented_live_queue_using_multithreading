using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.ServiceProcess;
using System.Timers;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private TextBox textBox1 = new TextBox();
        private bool pause = false;
        MyQueue<int> queue1 = new MyQueue<int>();

        public Form1()
        {
            InitializeComponent();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            pause = false;
            Controls.Add(textBox1);
            await Task.Run(() => startQueue());
        }

        private void print()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(print));
            }
            else
            {
                textBox1.Text = "Queue:" + Environment.NewLine + "num   time" + Environment.NewLine;
                Node<int> temp = queue1.head;
                for (int i = 0; i < queue1.count; i++)
                {
                    textBox1.Text += "   " + i.ToString() + "      " + temp.Data.ToString() + Environment.NewLine;
                    temp = temp.Next;
                }
                textBox1.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            queue1 = new MyQueue<int>();
            pause = true;
        }

        private void startQueue()
        {
            int counterNew = 0;
            Random rand = new Random();

            while (!pause)
            {
                if (counterNew == 0 || queue1.count == 0)
                {
                    queue1.Add(rand.Next(5, 13));
                    counterNew = rand.Next(3, 11);
                }
                if (queue1.head.Data == 0)
                    queue1.Remove();
                if (queue1.head != null)
                    queue1.head.Data--;
                counterNew--;
                Thread.Sleep(200);
                print();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pause = true;
        }

        void ClosingForm(object sender, FormClosingEventArgs answer)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }

    public class MyQueue<T> : IEnumerable<T>
    {
        public Node<T> head;
        public Node<T> tail;
        public int count;

        public void Add(T data)
        {
            Node<T> node = new Node<T>(data);

            if (head == null)
                head = node;
            else
                tail.Next = node;
            tail = node;

            count++;
        }

        public bool Remove()
        {
            if (count > 0)
            {
                head = head.Next;
                count--;
                return true;
            }
            else
            {
                return false;
            }
        }

            IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Node<T> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }

    public class Node<T>
    {
        public Node(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public Node<T> Next { get; set; }
    }
}
