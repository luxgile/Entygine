using System;
using System.IO;

namespace Entygine.DevTools
{
    internal class ConsoleLoggerFile : IConsoleLogger
    {
        public ConsoleLoggerFile()
        {

            try
            {
                string txt = "";
                using (FileStream readStream = new FileStream(TextLocation, FileMode.OpenOrCreate))
                {
                    using (StreamReader reader = new StreamReader(readStream))
                        txt = reader.ReadToEnd();
                }

                using (FileStream writeStream = new FileStream(PrevTextLocation, FileMode.OpenOrCreate))
                {
                    using (StreamWriter writer = new StreamWriter(writeStream))
                        writer.Write(txt);
                }

                using FileStream s = File.Create(TextLocation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Log(object log)
        {
            try
            {
                using FileStream stream = new FileStream(TextLocation, FileMode.Append);
                using StreamWriter writer = new StreamWriter(stream);
                writer.Write(log.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void Clear() { }

        private string TextLocation => AppEngineInfo.GetApplicationDataPath() + @"\dev_log.txt";
        private string PrevTextLocation => AppEngineInfo.GetApplicationDataPath() + @"\prev_dev_log.txt";
    }
}
