using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Manager.Data_Structure;
using System.Runtime.Serialization.Formatters.Binary;

namespace Manager.Exstras
{

    class binaryconverter
    {
        
        public static int cout = 0;
       public static void binconv()
        {
            runner(@"C:\Users\Shiri Miller\Google Drive\Garage\HongKong Info\HKEX - 28905377");
        }
        public static void Manager(String path)
        {


            StreamReader reader = new StreamReader(path);
            Dictionary<String, int> headersIDs = new Dictionary<string, int>();
            String line = reader.ReadLine();
            String[] headers = line.Split(',');

            for (int i = 0; i < headers.Length; i++)
            {
                headersIDs.Add(headers[i], i);
            }
            List<RowData> rowdatalist = new List<RowData>();
            RowData r = new RowData();

            while ((line = reader.ReadLine()) != null)
            {

                String[] fields = line.Split(',');

                r.datetime = DateTime.Parse(fields[headersIDs["DateTime"]].ToString());
                r.ask1 = double.Parse(fields[headersIDs["ask1"]].ToString());
                r.bid1 = double.Parse(fields[headersIDs["bid1"]].ToString());
                r.isTrade = int.Parse(fields[headersIDs["isTrade"]].ToString());
                r.askV = int.Parse(fields[headersIDs["ask1volume"]].ToString());
                r.bidV = int.Parse(fields[headersIDs["bid1Volume"]].ToString());
                r.dir = 0;
                rowdatalist.Add(new RowData(r));
            }
            
            string pathsaved = @"C:\Users\Shiri Miller\Desktop\bin\Test" + cout.ToString() + ".bin";
            Stream s = File.Open(pathsaved, FileMode.Create);
            BinaryFormatter bin = new BinaryFormatter();
            bin.Serialize(s, rowdatalist);
            s.Close();
            Stream ss = File.Open(pathsaved, FileMode.Open);
            BinaryFormatter bbiinn = new BinaryFormatter();

            var lr = (List<RowData>)bbiinn.Deserialize(ss);

            Console.WriteLine(lr.Count);
            cout++;


        }
        public static void runner(string path)
        {
            String[] names = Directory.GetFiles(path);
            foreach (var i in names)
            {
                if (-1 != i.IndexOf(".csv"))
                    //Console.WriteLine(i);
                    Manager(i.ToString());
            }
        }
      

    }
}
