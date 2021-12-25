using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        
        //static StreamWriter sr = new StreamWriter("log.txt");
        public static void LogException(Exception ex)
        {
            // TODO: Create log file named log.txt to log exception details in it
            FileStream fs = new FileStream("log.txt", FileMode.OpenOrCreate);
            StreamWriter sr = new StreamWriter(fs);
            //Datetime:
            //message:
            // for each exception write its details associated with datetime 
            sr.WriteLine(DateTime.Now.ToString() + ":" + ex.Message);
            sr.Close();
        }
    }
}
