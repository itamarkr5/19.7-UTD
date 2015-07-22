using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Manager
{
    abstract class Strategy
    {
        private List<saveData> logInfo; // position list
        private double sum = 0; // Strategy sum money
        private int pvol = 0; // Volume per entry
        protected String name; // Strategy name
        public static RowData prevRowData;  // Last RowData to save
        protected double[] paramArr; // Array of Paramaters
        private static int arrLength = 7; // Paramater Arr Length
        private List<double> eachDaySum; // List of each day sum + before for scoring

        public Strategy()
        {
            this.sum = 0;
            //this.maxSum = 0;
            this.pvol = 0;
            this.logInfo = new List<saveData>();
            this.paramArr = new double[arrLength];
            this.eachDaySum = new List<double>();
            setArr();
            
        } //Constructor

        public int getLength()
        {
            return arrLength;
        }
        public int getPVol()
        {
            return this.pvol;
        } //get current volume in hold
        public List<saveData> getList()
        {
            return this.logInfo;
        } //Get List - logInfo
        public String getName()
        {
            return this.name;
        }
        public String getParamatersInfo()
        {
            String s = "";
            for (int i = 0; i < arrLength; i++)
            {
                if (this.paramArr[i] != -999)
                    s += "Paramater: " + i.ToString() + this.paramArr[i].ToString();
            }
            return s;
        }
      
        public void setDayToList(bool ok)
        {
            if (ok)
                this.eachDaySum.Add(this.eachDaySum[this.eachDaySum.Count() - 1] + this.sum);
            else
                this.eachDaySum.Add(this.sum);//--------------------------
        } // Adds a total of all sums per day
        
        public void writePosLogFile(List <saveData> lsd,string path) 
        {
            if(!Directory.Exists(path+@"\Logs"))
            {Directory.CreateDirectory(path+@"\Logs");}

            if (File.Exists(path + @"\Logs\log - " + this.name + ".csv"))
            { File.WriteAllText(path + @"\Logs\log - " + this.name + ".csv", string.Empty); }

            path += @"\Logs\log - " +this.name +".csv";
            StreamWriter w = File.AppendText(path);
            w.WriteLine("DateTimeStart,ask1Start,askVolumeStart,bid1Start,bidVolumeStart,dirAtStart,DateTimeEnd,ask1End,askVolumeEnd,bid1End,bidVolumeEnd,dirAtEnd,profit");
            string line;

            for (int i = 0; i < lsd.Count; i++)
            {
                line = lsd[i].printString();
                if (i > 0)
                {
                    if (lsd[i - 1].getEnd().datetime.Day != lsd[i].getFirst().datetime.Day)
                    {
                        w.WriteLine(lsd[i].clearRow());
                    }
                }
                w.WriteLine(line);

            }
            w.Flush();
            w.Close();

        } // writes position - configuration log into file

        public finalAllStratLog editFinalLogData(String path , finalAllStratLog endLog/* , double score*/)
        {
            if (this.logInfo.Count != 0)
            {
                endLog.start = this.logInfo[0].getFirst().datetime;
                endLog.end = this.logInfo[this.logInfo.Count() - 1].getEnd().datetime;
                endLog.name = this.name;
                endLog.pLog = path;
                endLog.numOfPos = this.logInfo.Count();
                endLog.avg = 0;
                foreach (saveData s in this.logInfo)
                {
                    endLog.avg += ProgramMain.deltaMin(s.getFirst().datetime, s.getEnd().datetime);
                }
                // makes avg time for position
                endLog.avg = (endLog.avg / endLog.numOfPos) / 60;
                endLog.totalSum = this.sum;
                foreach (saveData s in this.logInfo)
                {
                    endLog.avgSum += s.getProfit();
                }
                endLog.avgSum = endLog.avgSum / this.logInfo.Count();
                // does avg sum for position
                int totalPos = logInfo.Count();
                int profitablePos = logInfo.Where(x => x.getBoolProfit()).Count();             
                endLog.percentProfit = ((1.0 * profitablePos) / (1.0 * totalPos)) * 100;

                for (int i = 0; i < endLog.paramArr.Length; i++)
                {
                    endLog.paramArr[i] = this.paramArr[i];
                }
                //endLog.Score = (profitablePos / endLog.numOfPos) * 100; // easy score to test
                //copies paramater array to log
                Console.WriteLine("Start Working On Test Class");
                endLog.Score = Scoring.Test2.score(profitablePos, endLog.numOfPos, this.sum, this.eachDaySum);
                return endLog;
            }
            return null;
        } // edits info for final log     

        public void insertPosLogInfo(RowData r)
        {
           if(logInfo.Count() > 0 && !logInfo[logInfo.Count() - 1].getPosEnd())
           {
               double posSum = 0;

               if (logInfo[logInfo.Count() - 1].getDir() == 1)
                   posSum -= logInfo[logInfo.Count() - 1].getFirst().ask1;
               if (logInfo[logInfo.Count() - 1].getDir() == -1)
                   posSum += logInfo[logInfo.Count() - 1].getFirst().bid1;

               if (r.dir == 1)
                   posSum -= r.ask1;
               if (r.dir == -1)
                   posSum += r.bid1;
              
               logInfo[logInfo.Count() - 1].setEndRowData(r);
               logInfo[logInfo.Count() - 1].setPosEndBool(true);
               logInfo[logInfo.Count() - 1].setProfit(posSum);
               logInfo[logInfo.Count() - 1].setTotalSum(this.sum);
           }
            else
           {
               logInfo.Add(new saveData (r));
           }        
        } // decides values for config after buy/sell
       
        public bool buy(RowData r , int vol)
        {
            r.dir = 1;
            if (canProceed(r))
            {
                this.sum -= r.ask1;
                this.pvol += vol;
                insertPosLogInfo(r);
                return true;
            }
            return false;
        } //makes buy
        public bool sell(RowData r , int vol)
        {
            r.dir = -1;
            if (canProceed(r))
            {
                this.sum += r.bid1;
                this.pvol -= vol;
                insertPosLogInfo(r); 
                return true;
            }
            return false;
        } //makes sell

        public bool canProceed(RowData r)
        {
            if (logInfo.Count() > 0 && !logInfo[logInfo.Count() - 1].getPosEnd())
            {
                if (this.logInfo[this.logInfo.Count() - 1].getDir() == r.dir)
                {
                    ProgramMain.creatErrorInfoLogTxt(this.name + " : tried to do same action twice");
                    return false;
                }
            }
                return true;

        } // checks if can proceed after config buy/sell - won't proceed if tries to do same thing again

        public abstract void getInfo(RowData r); // gets RowData from main (in each strategy works as it wants)
        public virtual void mustClosePos(RowData r)
        {
            if (this.logInfo[this.logInfo.Count() - 1].getFirst().dir == 1)
                sell(r, 1);
            if (this.logInfo[this.logInfo.Count() - 1].getFirst().dir == -1)
                buy(r, 1);
        } // closes last position if open at end
        public virtual void setArr()
        {
            for (int i = 0; i < this.paramArr.Length; i++)
            {
                this.paramArr[i] = -999;
            }
        } // Set paramater info arr

        public virtual void reset()
        {
        }
       
        }
    
    }
    

