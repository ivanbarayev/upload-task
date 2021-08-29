using System;
using System.IO;
using static System.Environment;
using Newtonsoft.Json;
using upload_task.Models;
namespace upload_task
{
    public class Common
    {
        private static string current_path = Directory.GetCurrentDirectory();
        private static string local_storage = Path.Combine(current_path, "LocalStorage");
        private static string settings_file = Path.Combine(local_storage, "settings.json");
        private static string upload_dir = Path.Combine(local_storage, "Uploads");
        private static string log_dir = Path.Combine(current_path, "Logs");
        private static string log_file = Path.Combine(log_dir, "log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".json");
        private int END = S.SUCCESS;
        private string DIR;
        private bool chk_variable;




        #region Check Directory
        /*
         * END = 1 > Success
         * END = 2 > Failed
         * END = 3 > Exist
         */
        public int ChkDir(string dir_name)
        {
            DIR = Path.Combine(current_path, dir_name);
            if (!Directory.Exists(DIR))
            {
                try
                {
                    Directory.CreateDirectory(DIR);
                    END = S.SUCCESS;
                }
                catch
                {
                    END = S.ERROR;
                }
            }
            else
            {
                END = S.EXIST;
            }

            return END;
        }
        #endregion


        #region Check Directory2
        /*
         * END = 1 > Success
         * END = 2 > Failed
         * END = 3 > Exist
         */
        public int ChkDir2(string dir_name)
        {
            if (!Directory.Exists(dir_name))
            {
                try
                {
                    Directory.CreateDirectory(dir_name);
                    END = S.SUCCESS;
                }
                catch
                {
                    END = S.ERROR;
                }
            }
            else
            {
                END = S.EXIST;
            }

            return END;
        }
        #endregion

        #region Check File
        /*
         * END = 1 > Exist
         * END = 2 > Not Exist
         */
        public int ChkFile(string file_path)
        {
            if (File.Exists(file_path))
            {
                END = S.EXIST;
            }
            else
            {
                END = S.NOTFOUND;
            }

            return END;
        }
        #endregion

        #region Return Responser
        /*
         * status = 1 > Success
         * status = 2 > Failed
         * status = 3 > Custom
         * message= ''> Custom or error message based on status
         */
        public HTTPResponser Responser(int status, string message, dynamic data = null)
        {
            HTTPResponser RES = new HTTPResponser
            {
                status = status,
                message = message,
                data = data
            };

            return RES;
        }

        #endregion

        #region Settings Loader
        /*
         * Load system default settings from JSON file
         */
        public void LoadSettings()
        {
            Settings obj = new SettingsModel().SettingsRead();

            if (obj == null)
            {
                Settings default_settings = new Settings()
                {
                    //_id = 6453,
                    min_file_size = 20,
                    max_file_size = 500,
                    destination_path = upload_dir,
                    allow_mimes = "jpg,png",
                    allow_rename = false,
                    allow_override = true,
                    store_type = "FS",
                };

                new SettingsModel().SettingsInsert(default_settings);
            }

            obj = new SettingsModel().SettingsRead();

            //SetEnvironmentVariable("set_id", obj.min_file_size.ToString());
            SetEnvironmentVariable("min_file_size", obj.min_file_size.ToString());
            SetEnvironmentVariable("max_file_size", obj.max_file_size.ToString());
            SetEnvironmentVariable("destination_path", obj.destination_path.ToString());
            SetEnvironmentVariable("allow_mimes", obj.allow_mimes.ToString());
            SetEnvironmentVariable("allow_rename", obj.allow_rename.ToString());
            SetEnvironmentVariable("allow_override", obj.allow_override.ToString());
            SetEnvironmentVariable("store_type", obj.store_type.ToString());
        }
        #endregion

        #region Settings Writer
        /*
         * Write system default settings to JSON file
         */
        public int WriteToFile(dynamic Datas)
        {
            try
            {
                File.WriteAllText(settings_file, Datas);
                END = S.SUCCESS;
            }
            catch
            {
                END = S.ERROR;
            }

            return END;
        }
        #endregion

        #region Log File Writer
        /*
         * Write system error logs to JSON file
         */
        public void LogWriter(int status, string location, string message)
        {
            END = ChkDir(log_dir);
            if (END == S.SUCCESS || END == S.EXIST)
            {
                LogWrite structer = new LogWrite
                {
                    status = status,
                    location = location,
                    message = message,
                    date = DateTime.Now
                };
                var data = JsonConvert.SerializeObject(structer);
                if (!File.Exists(log_file))
                {
                    using (StreamWriter sw = File.CreateText(log_file))
                    {
                        sw.WriteLine(data);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(log_file))
                    {
                        sw.WriteLine(data);
                    }
                }
            }
        }
        #endregion

        public string FileRenamer(string file_name)
        {
            return Guid.NewGuid().ToString() + "." + file_name.Split(".")[1];
        }

        public bool ChkString(string variable)
        {
            if (String.IsNullOrEmpty(variable) || String.IsNullOrWhiteSpace(variable))
            {
                chk_variable = true;
            }
            else
            {
                chk_variable = false;
            }

            return chk_variable;
        }
    }
}