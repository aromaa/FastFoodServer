﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastFoodServer.Core
{
    public class ConfigurationData
    {
        private Dictionary<string, string> data;

        public ConfigurationData(string filePath, bool throwExceptionIfFileNotFound = false)
        {
            this.data = new Dictionary<string, string>();
            if (!File.Exists(filePath))
            {
                if (!throwExceptionIfFileNotFound)
                {
                    Logging.WriteLine("Unable to locate configuration file at '" + filePath + "'!", ConsoleColor.Red);
                    Logging.WriteBlank();
                    Logging.WriteLine("Press any key to shut down ...", ConsoleColor.Blue);
                    Console.ReadKey(true);
                    Program.Destroy();
                }
                else
                {
                    throw new Exception("Unable to locate configuration file at '" + filePath + "'!");
                }
            }
            try
            {
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    string str;
                    while ((str = streamReader.ReadLine()) != null)
                    {
                        if (str.Length >= 1 && !str.StartsWith("#"))
                        {
                            int num = str.IndexOf('=');
                            if (num != -1)
                            {
                                string key = str.Substring(0, num);
                                string value = str.Substring(num + 1);

                                this.data.Add(key, value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Could not process configuration file: " + ex.Message);
            }
        }

        public string this[string key]
        {
            get
            {
                if (this.data.ContainsKey(key))
                {
                    return this.data[key];
                }
                else
                {
                    throw new Exception("Unable to find configuration file data: " + key);
                }
            }
        }
    }
}
