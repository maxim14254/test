using System;
using WindowsFormsApp1.ServiceReference1;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        Service1Client client = new Service1Client();// класс для взаимодействия с БД
        public Form1()
        {
            InitializeComponent();
          
            comboBox1.Items.AddRange(UpdateData.Money.Select(c => c.Element("CharCode").Value).OrderBy(s=>s).ToArray());// сохраняем тикеры всех доступный валют в combobox
            comboBox1.Text = comboBox1.Items[0].ToString();
        }

        private void button3_Click(object sender, EventArgs e)// кнопка выхода
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)// кнопка для загрузки данных в нашу БД
        {
            if (dateTimePicker4.Value != null)
            {
                UpdateData.Update(dateTimePicker4.Value);
            }
            else
            {
                MessageBox.Show("Введите дату в поле", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)// кнопка для получения цены в определенную дату
        {

            if (dateTimePicker1.Value != null)
            {
                textBox2.Text = client.SelectPrice(dateTimePicker1.Value, comboBox1.Text);
            }
            else
            {
                MessageBox.Show("Некорректно введены данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        private void button2_Click(object sender, EventArgs e)// кнопка для получения среденей цены за временной промежуток
        {
            if (dateTimePicker2.Value != null && dateTimePicker3.Value != null && dateTimePicker2.Value <= dateTimePicker3.Value)  
            {
                textBox1.Text = client.AveragePrice(dateTimePicker2.Value, dateTimePicker3.Value, comboBox1.Text);
            }
            else
            {
                MessageBox.Show("Некорректно введены данные", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)// кнопка показа графика цен определенной валюты
        {
            Form2 form2 = new Form2(comboBox1.Text, client);
            form2.Show();
        }
    }
}
