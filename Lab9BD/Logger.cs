using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab9BD
{
    public class Logger 
    {
        private static List<string> loggers = new List<string>();
        static public void AddLog(String LogPart)
        {
            loggers.Add(LogPart);
        }
        public static List<String> SaveLog()
        {
            try
            {
                StreamWriter sw = new StreamWriter("E:\\list.txt");
                foreach (var item in loggers)
                {
                    sw.WriteLine(item);
                   // ListingLable.Text = Form1.ListingLable.Text + "\n" + item;

                }
                sw.Close();
                return loggers;

            }
            catch (Exception ex)
            {
                return null;
                //ListingLable.Text = ex.Message;
            }
        }
    }
}
