using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceLibrary2
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        void SaveData(float Price, DateTime Date, string Name);// метод сохранения в БД информациио валюте

        [OperationContract]
        string SelectPrice(DateTime Date, string Name);// метод получения цены в опрделенную дату

        [OperationContract]
        string AveragePrice(DateTime DateStart, DateTime DateFinish, string Name);// метод получения средней цены за временной промежуток 

        [OperationContract]
        List<List<string>> AllPriceAndDates(string Name);// метод получения всей информации о валюте

    }
}
