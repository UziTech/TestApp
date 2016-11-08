using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            Encription en = new Encription(textBox1.Text);
            textBox1.Text = en.EncryptedText;
        }
        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            Encription en = new Encription(textBox1.Text);
            textBox1.Text = en.DecryptedText;
        }
    }
}
