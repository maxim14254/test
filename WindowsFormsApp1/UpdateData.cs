using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WindowsFormsApp1.ServiceReference1;

namespace WindowsFormsApp1
{
    public static class UpdateData// класс для загрузки и сохранения в БД информации о валютах
    {
       public static List<XElement> Money = new List<XElement>();// массив всех данных
       public static void Update(DateTime date)
        {

            XDocument xdoc = XDocument.Load($"http://www.cbr.ru/scripts/XML_daily.asp?date_req={date.ToShortDateString()}");// загружаем данные с сайта ЦБ

            Money = xdoc.Element("ValCurs").Elements("Valute").ToList();//получаем массив с данными на каждую валюту

            for (int i = 0; i < Money.Count; i++)// перебираем
            {
                float price = float.Parse(Money[i].Element("Value").Value);// получаем цену
                
                string name = Money[i].Element("CharCode").Value;// получаем тикер

                var client = new Service1Client();// класс для взаимодействия с БД

                client.SaveData(price, date, name);// сохраняем данные в БД определенной валюты
   
            }
        }
    }
}
