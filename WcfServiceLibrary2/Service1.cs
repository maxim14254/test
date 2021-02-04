using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary2
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    public class Service1 : IService1, IDisposable
    {
        private readonly SqlConnection connection;
        public Service1()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;//Строка подключения App.config
            connection = new SqlConnection(connectionString);
            connection.Open();// открываем подключение
        }
        public void SaveData(float Price, DateTime Date, string Name)// метод сохранения в БД информациио валюте
        {
            try
            {
                string query = $"CREATE TABLE [dbo].[{Name}] ([Id] INT NOT NULL PRIMARY KEY IDENTITY, [Date] DATETIME NOT NULL, [Price] FLOAT NOT NULL)";// запрос SQL на создание таблиц данных о каждой валюте
                SqlCommand myCommand = new SqlCommand(query, connection);
                myCommand.ExecuteNonQuery();// выполнение запроса
            }
            catch (System.Data.SqlClient.SqlException) { }// срабатывает когда таблицы уже существуют

            string query2 = $"INSERT INTO {Name} (Date, Price) VALUES ('{Date.Year}.{Date.Month}.{Date.Day}', '{Price.ToString().Replace(',', '.')}')";// запрос на запись в таблицу
            string query3 = $"With duplicates As (Select *, ROW_NUMBER() Over (PARTITION by Date Order by Date) as Duplicate From {Name}) delete From duplicates Where Duplicate > 1 ;";// запрос на удаление одинаковых дат в таблице
            SqlCommand myCommand2 = new SqlCommand(query2, connection);
            SqlCommand myCommand3 = new SqlCommand(query3, connection);
            myCommand2.ExecuteNonQuery();
            myCommand3.ExecuteNonQuery();// выполенние запроса
        }
        public string SelectPrice(DateTime Date, string Name)// метод получения цены в опрделенную дату
        {
            string rezult;
            try
            {
                string query = $"SELECT Price FROM {Name} WHERE Date='{Date.Year}.{Date.Month}.{Date.Day}'";// запрос на получение цены на заданную дату
                SqlCommand myCommand = new SqlCommand(query, connection);
                string a = myCommand.ExecuteScalar().ToString();// выполенние запроса
                rezult = a;
            }
            catch (Exception ex) when (ex is System.NullReferenceException)// срабатывает когда данные на заданную лату отсутстуют
            {
                rezult = "";
            }
            return rezult;
        }

        public string AveragePrice(DateTime DateStart, DateTime DateFinish, string Name)// метод получения средней цены за временной промежуток 
        {
            string rezult;
            try
            {
                string query = $"SELECT AVG(Price) FROM {Name} WHERE Date>='{DateStart.Year}.{DateStart.Month}.{DateStart.Day}' AND Date<='{DateFinish.Year}.{DateFinish.Month}.{DateFinish.Day}' ";// запрос на получение среденей цены на заданную дату
                SqlCommand myCommand = new SqlCommand(query, connection);
                string a = myCommand.ExecuteScalar().ToString();// выполенение запроса
                rezult = a;
            }
            catch (Exception ex) when (ex is System.NullReferenceException)// срабатывает когда данные на заданную лату отсутстуют
            {
                rezult = "";
            }
            return rezult;
        }


        public List<List<string>> AllPriceAndDates(string Name)// метод получения всей информации о валюте
        {
            List<List<string>> rezult = new List<List<string>>();// массив для хранения результата
            rezult.Add(new List<string>());
            rezult.Add(new List<string>());

            string query = $"SELECT Price, Date FROM {Name} ORDER BY Date";// запрос на получение всей информации для заданной валюты
            SqlCommand myCommand = new SqlCommand(query, connection);
            SqlDataReader reader = myCommand.ExecuteReader();// выполенение запроса
            
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read()) // построчно считываем данные
                {
                    rezult[0].Add(reader["Price"].ToString());// сохраняем данные цены в массив
                    rezult[1].Add(reader["Date"].ToString());// сохраняем данные даты в массив
                }
                reader.Close();
            }
            return rezult;
        }
        public void Dispose()
        {
            connection.Close();//закрываем подключение
        }

    }
}
