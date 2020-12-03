using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_03B_5153_4372
{
    public static class RestartBuses
    {
        public static void Restart10Buses(ObservableCollection<Bus> buses)
        {
            buses.Add(new Bus("12345678", new DateTime(2018, 12, 1), 10000, 1200, new DateTime(2020, 12,1 ), 8001));
            buses.Add(new Bus("1524897", new DateTime(2017, 12, 1), 10000, 1, new DateTime(2020, 12, 1), 9500));//need refuel
            buses.Add(new Bus("45698725", new DateTime(2019, 12, 1), 10000, 1000, new DateTime(2019, 12, 1), 9700));//need treatment-date
            buses.Add(new Bus("47589646", new DateTime(2019, 11, 2), 10000, 800, new DateTime(2020, 9,2), 9600));
            buses.Add(new Bus("1456982", new DateTime(2016, 11, 2), 10000, 800, new DateTime(2020, 10, 1), 9600));
            buses.Add(new Bus("1458795", new DateTime(2015, 11, 3), 20000, 1200, new DateTime(2020, 8, 21),19000));
            buses.Add(new Bus("65984758", new DateTime(2019, 8, 2), 30000, 800, new DateTime(2020, 5, 15), 10001));//need treatment-20000
            buses.Add(new Bus("4569821", new DateTime(2014, 11, 20), 10000, 800, new DateTime(2020,6, 1), 9600));
            buses.Add(new Bus("2564875", new DateTime(2013, 11, 2), 50000, 800, new DateTime(2020, 10, 1), 49700));
            buses.Add(new Bus("42650314", new DateTime(2019, 1, 20), 10000, 1200, new DateTime(2020, 7, 1), 9900));
        }
    }
}
