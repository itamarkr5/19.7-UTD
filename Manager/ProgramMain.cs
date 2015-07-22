using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Manager.Exstras;
using Manager.Strategies;
namespace Manager
{
    class ProgramMain
    {
        public static RowData[] toGetPointer = new RowData[ProgramMain.number];
        public static RowData row = new RowData();
        public const int number = 10;// parmater for queue length changeble 
        public static bool fileIsOpen = false;
        public static Queue<RowData> rowDataToSave = new Queue<RowData>();
        public static String path = @"C:\Users\urik\Google Drive\Garage\HongKong Info\HKEX - 28905377 bin";
        public static List<RowData> lr = new List<RowData>();

        static void Main(string[] args)
        {
            //binaryconverter.binconv(); //only for new files
            List<Strategy> ls = new List<Strategy>(); //list of strategies
            List<finalAllStratLog> ell = new List<finalAllStratLog>(); //list of config final log

            //creates list
            createStrategyToList(ls);

            RunMain(path, ls); // runs methods

            finalAllStratLog editLog = new finalAllStratLog(ls[0].getLength());

            foreach (var strat in ls) // prints log for pos
            {
                if (strat.getList().Count != 0)
                {
                    strat.writePosLogFile(strat.getList(), path);
                }
                else
                {
                    creatErrorInfoLogTxt("No Transactions and Positions for: " + strat.getName() + " Paramaters: " + strat.getParamatersInfo());
                }
                finalAllStratLog logInfo;
                logInfo = strat.editFinalLogData(path, editLog);
                if (logInfo != null)
                    ell.Add(new finalAllStratLog(logInfo));
            }


            foreach (finalAllStratLog e in ell) // prints sum log for all strategy
            {
                printFinalConfigLog(path, ell);
            }

            //add func to create graph img to top 10 (paramater)

            
        } // Main

        public static void RunMain(String path, List<Strategy> ls)
        {

            String[] names = Directory.GetFiles(path);

            foreach (var i in names)
            {
                if (-1 != i.IndexOf(".bin"))
                {
                    Manager(i.ToString(), ls);

                }
            }
        } // checks all files in folder path and run manager

        public static void Manager(String path, List<Strategy> ls)
        {           
            clearQueue();
            bool prevRowIn = false;
            bool firstWasIn = false;
            RowData rToChange;
            Console.WriteLine("Start file: " + path);
            Stream s = File.Open(path, FileMode.Open);
            BinaryFormatter bin = new BinaryFormatter();
            ProgramMain.lr = (List<RowData>)bin.Deserialize(s);
            s.Close();
            Console.WriteLine("End file: " + path);
            bin = null;
            foreach (RowData r in ProgramMain.lr)
            {
                setQueue(r);

                foreach (var strat in ls)
                {
                    if (prevRowIn)
                    {
                        
                        strat.getInfo(r);
                        
                    }
                    Strategy.prevRowData = new RowData(r);
                    if (!prevRowIn)
                    {
                        prevRowIn = true;
                    }
                }
                rToChange = r;
                rToChange = default(RowData);
            }
          
            foreach (var strat in ls)
            {
                if (strat.getList().Count() > 0 && !strat.getList()[strat.getList().Count() - 1].getPosEnd())
                {
                    strat.mustClosePos(Strategy.prevRowData);
                }

                strat.setDayToList(firstWasIn);
                strat.reset();
            }
           
            //GC.Collect();
            ProgramMain.lr.Clear();
            s.Close();
            firstWasIn = true;
        } // sends each row data to strategy

        public static void printFinalConfigLog(String path, List<finalAllStratLog> ell)
        {
            int i = 0;

            if (!Directory.Exists(path + @"\Logs"))
            { Directory.CreateDirectory(path + @"\Logs"); }

            if (File.Exists(path + @"\Logs\log - Main Run Log.csv"))
                File.WriteAllText(path + @"\Logs\log - Main Run Log.csv", string.Empty);

            path += @"\Logs\log - Main Run Log.csv";

            StreamWriter w = File.AppendText(path);

            String topRow = "ConfigName,";
            for (i = 0; i < ell[0].paramArr.Length; i++)
            {
                topRow += "Paramater " + (i + 1).ToString() + ",";
            }
            topRow += "DateTimeStart,DateTimeEnd,LogPath,numOfPos,avgPosTime,TotalSum,avgPosSum,UpPos,Score";

            w.WriteLine(topRow);
            for (i = 0; i < ell.Count; i++)
            {
                string line = ell[i].printString();
                w.WriteLine(line);

            }
            w.Flush();
            w.Close();

        } // creats final config log

        public static void createStrategyToList(List<Strategy> ls)
        {
            /*
            for (int sec = 30; sec <= 420; sec += 30)
            {
                for (int cell = 5; cell <= 7; cell++)
                {
                    for (double diff = 0.99; diff < 1.0; diff += 0.002)
                    {
                        for (double exit = 0.4; exit < 1; exit += 0.2)
                        {
                            for (double lost = 0.4; lost < 1; lost += 0.2)
                            {
                                for (double percentToExit = 0.2; percentToExit < 1; percentToExit += 0.1)
                                {
                                    ls.Add(new Dan(sec, cell, diff, cell / 4, exit, lost, percentToExit));
                                }
                            }
                        }
                    }
                }
            }
           */

            ls.Add(new Dan(60,7,0.99,3,0.6,1,0.4));
            //ls.Add(new NotFail(7, 2, 1));

            //ls.Add(new NotFail(7, 2, 1));
            
        } // creates strategy config list

        public static void creatErrorInfoLogTxt(String st)
        {
            if (!Directory.Exists(ProgramMain.path + @"\Logs"))
            {
                Directory.CreateDirectory(ProgramMain.path + @"\Logs");
            }

            if (File.Exists(ProgramMain.path + @"\Logs\log - mainErrorInfoLog.txt") && (!fileIsOpen))
            {
                File.WriteAllText(ProgramMain.path + @"\Logs\log - mainErrorInfoLog.txt", string.Empty);
                fileIsOpen = true;
            }
            
            StreamWriter w = File.AppendText(path + @"\Logs\log - mainErrorInfoLog.txt");
            w.WriteLine(st);

            w.Flush();
            w.Close();

        } // creates error / info log after run if exists

        /*public static int deltaMin(DateTime start, DateTime end)
        {
            int deltaHour, deltaMinutes;

            if (start.Hour > end.Hour)
            {
                deltaHour = (start.Hour - end.Hour) * 60;
                deltaMinutes = start.Minute - end.Minute;
            }
            else
            {
                if (start.Hour < end.Hour)
                {
                    deltaHour = (end.Hour - start.Hour) * 60;
                    deltaMinutes = end.Minute - start.Minute;
                }
                else
                {
                    deltaHour = 0;
                    deltaMinutes = Math.Abs(start.Minute - end.Minute);
                }
            }
            return deltaMinutes + deltaHour;
        }
        */
        public static void clearQueue()
        {
            if (ProgramMain.rowDataToSave.Count != 0)
            {
                RowData first = ProgramMain.rowDataToSave.Peek();
                ProgramMain.rowDataToSave.Enqueue(ProgramMain.rowDataToSave.Dequeue());
                while (!first.Equals(ProgramMain.rowDataToSave.Peek()))
                {
                    ProgramMain.rowDataToSave.Enqueue(ProgramMain.rowDataToSave.Dequeue());
                }

                System.GC.Collect();
                ProgramMain.rowDataToSave.Clear();
            }
        } // clear queue for manager

        public static void setQueue(RowData r)
        {
            if (ProgramMain.rowDataToSave.Count < number)
            {
                ProgramMain.rowDataToSave.Enqueue(new RowData(r));
            }
            else
            {
                ProgramMain.rowDataToSave.Enqueue(new RowData(r));
                row.ToChange((ProgramMain.rowDataToSave.Dequeue()));
            }
            ProgramMain.toGetPointer = ProgramMain.rowDataToSave.ToArray();
        } // set queue according to paramaters

        public static int deltaMin(DateTime start, DateTime end)
        {
            if (start > end)
            {
                return deltaMinOrg(end, start);
            }
            else
            {
                return deltaMinOrg(start, end);
            }
        }
        private static int deltaMinOrg(DateTime start, DateTime end)
        {
            int hour = end.Hour - start.Hour;
            int min = end.Minute - start.Minute;
            int sec = end.Second - start.Second;
            int milisec = end.Millisecond - start.Millisecond;
            return ((hour * 60 + min) * 60 + sec) ;
        }
        
    }
}
    



 