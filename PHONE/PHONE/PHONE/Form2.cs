using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
namespace PHONE
{
    public partial class Form2 : Form
    {
        public static Form1 f1 = null;
        public Form2(Form1 f11)
        {
            InitializeComponent();
            f1 = f11;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddPhoneNumber(sender, e);
            Close();
        }

        private void AddPhoneNumber(object sender, EventArgs e)
        {
            if (ValidatePhoneNumber(sender, e, textBox1.Text))
            {
                if (!(CheckPhoneNumberExists(sender, e, textBox1.Text)))
                {
                    int FirstEmpty = FindEmptyPhoneNumber(sender, e);
                    if (FirstEmpty != -1)
                    {
                        Form1.PhoneNumbers[FirstEmpty] = new Form1.CPHONE_NUMBER();
                        Form1.PhoneNumbers[FirstEmpty].PhoneNumber = textBox1.Text;
                        Form1.PhoneNumbers[FirstEmpty].ip = IPAddress.Loopback;
                    }
                    else
                    {
                        MessageBox.Show("No Phone numbers awailable.", "PHONE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Phone number already exists", "PHONE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid phone number.", "PHONE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CheckPhoneNumberExists(object sender, EventArgs e, String phonenumber)
        { 
            for (int i = 0; i < Form1.MAX; i++)
            {
                if (Form1.PhoneNumbers != null)
                {
                    if (Form1.PhoneNumbers[i] != null)
                    {
                        if (Form1.PhoneNumbers[i].PhoneNumber == phonenumber)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private int FindEmptyPhoneNumber(object sender, EventArgs e)
        {
            int FirstEmpty = -1;
            for (int i = 0; i < Form1.MAX; i++)
            {
                if (Form1.PhoneNumbers != null)
                {
                    if (Form1.PhoneNumbers[i] == null)
                    {
                        FirstEmpty = i;
                        break;
                    }
                }
            }
            return FirstEmpty;
        }

        private bool ValidatePhoneNumber(object sender, EventArgs e, String phonenumber)
        {
            if (!(phonenumber.Length == 13))
            {
                return false;
            }
            try
            {
                ulong.Parse(phonenumber);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
