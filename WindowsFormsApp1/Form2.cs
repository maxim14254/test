using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsFormsApp1.ServiceReference1;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2(string name, Service1Client client)
        {
            InitializeComponent();
            
            SeriesCollection seriesViews = new SeriesCollection();

            ChartValues<decimal> price = new ChartValues<decimal>();

            List<string> dates = new List<string>();

            string[][] Data = new string[2][];
           
            Data = client.AllPriceAndDates(name);

            for (int i = 0; i < Data[0].Length; i++)
            {
                price.Add(decimal.Parse(Data[0][i].ToString()));
                dates.Add(Data[1][i]);
            }
            cartesianChart1.AxisX.Clear();
            cartesianChart1.AxisX.Add(new Axis { Labels = dates });

            LineSeries line = new LineSeries();
            line.Title = "Цена";
            line.Values = price;
            seriesViews.Add(line);
            cartesianChart1.Series = seriesViews;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            cartesianChart1.LegendLocation = LegendLocation.Bottom;
        }
    }
}

