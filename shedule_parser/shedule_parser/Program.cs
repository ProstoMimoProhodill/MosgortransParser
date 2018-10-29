using System;
using Gtk;

using System.Net;
using System.IO;
using System.Text;

using CsQuery;
using Newtonsoft.Json;

namespace shedule_parser
{
    public class Parser
    {
        private class Data
        {
            public string Station;
            public string Hour { get; set; }
            public string Minutes { get; set; }
        }

        private string URL = "";
        private string html = "";
        private Data[] data { get; set; }

        public Parser(string url)
        {
            URL = url;
        }

        public string createJSON_station()
        {
            //connect to website
            HttpWebRequest proxy_request = (HttpWebRequest)WebRequest.Create(URL);
            proxy_request.Method = "GET";
            HttpWebResponse resp = proxy_request.GetResponse() as HttpWebResponse;
            using (StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding("Windows-1251"))) html = sr.ReadToEnd();

            //create html file
            CQ cq = CQ.Create(html);

            //search name of station
            CQ stations = cq.Find("h2");
            int station_amount = 0;

            foreach (IDomObject obj in stations)
            {
                if (System.Net.WebUtility.HtmlDecode(obj.InnerText) != "Уважаемые пассажиры!")
                    station_amount++;
            }

            string[] station = new string[station_amount];
            station_amount = 0;
            foreach (IDomObject obj in stations)
            {
                if (System.Net.WebUtility.HtmlDecode(obj.InnerText) != "Уважаемые пассажиры!")
                {
                    station[station_amount] = System.Net.WebUtility.HtmlDecode(obj.InnerText);
                    station_amount++;
                }     
            }

            //search data about time
            CQ objects = cq.Find("span");
            int count = 0;
            int hour_amount = 0;
            bool next_is_hour = false;
            string hour = "", hour_prev = "", minutes = "";

            foreach (IDomObject obj in objects)
            {
                if ((obj.GetAttribute("class") == "hour") || (obj.GetAttribute("class") == "grayhour"))
                    hour_amount++;
            }

            Data[] data = new Data[hour_amount];

            foreach (IDomObject obj in objects)
            {
                if (obj.GetAttribute("class") == "hour")
                {
                    hour = obj.InnerText;
                    next_is_hour = true;
                }
                else if (obj.GetAttribute("class") == "minutes")
                {
                    if (obj.InnerText != "")
                        minutes = minutes + obj.InnerText + ",";
                }
                else if (obj.GetAttribute("class") == "grayhour")
                {
                    data[count - 1] = new Data()
                    {
                        Station = station[count/24],
                        Hour = obj.InnerText,
                        Minutes = ""
                    };

                    count++;
                }

                if (next_is_hour)
                {
                    if (count == 0)
                    {
                        hour_prev = hour;
                    }
                    else
                    {
                        data[count - 1] = new Data()
                        {
                            Station = station[(count - 1)/24],
                            Hour = hour_prev,
                            Minutes = minutes.Remove(minutes.Length - 1)
                        };

                        hour_prev = hour;
                    }

                    next_is_hour = false;
                    minutes = "";
                    count++;
                }

                if (count == hour_amount)
                {
                    data[count - 1] = new Data()
                    {
                        Station = station[count/24-1],
                        Hour = hour_prev,
                        Minutes = minutes
                    };

                    if (((data[count - 1].Minutes).Length) != 0)
                        (data[count - 1].Minutes) = (data[count - 1].Minutes).Remove((data[count - 1].Minutes).Length - 1);

                }

            }

            //create json file with data
            string serialized = JsonConvert.SerializeObject(data);
            return serialized;


            //it's necessary to make sort for time and names
        }


    }

    class MainClass
    {
        private static void Main(string[] args)
        {
        
            Application.Init();
            MainWindow win = new MainWindow();

            win.Show();
            Application.Run();

            //string URL = "http://www.mosgortrans.org/pass3/shedule.php?type=avto&way=895&date=0000011&direction=AB&waypoint=all";

            //Parser parser = new Parser(URL);
            //Console.WriteLine(parser.createJSON_station());
            
        }

        public static void shedule_open(string data)
        {
            Shedule shedule_win = new Shedule(data);
            shedule_win.Show();
        }

        public static string parse(string Type, string Route, string Days, string Direction, string Waypoints)
        {
            string base_url = "http://www.mosgortrans.org/pass3/shedule.php?";
            //string URL = "http://www.mosgortrans.org/pass3/shedule.php?type=avto&way=895&date=0000011&direction=AB&waypoint=all";
            string URL = (base_url + "type=" + Type + "&way=" + Route + "&date=" + Days +"&direction=" + Direction + "&waypoint=" + Waypoints);

            Parser parser = new Parser(URL);

            Console.WriteLine(URL);
            Console.WriteLine(parser.createJSON_station());

            return parser.createJSON_station();
        }
    }
}

