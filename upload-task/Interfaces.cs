using MongoDB.Bson.Serialization.Attributes;
using System;
using static System.Environment;
using J = Newtonsoft.Json.JsonPropertyAttribute;
using N = Newtonsoft.Json.NullValueHandling;

namespace upload_task
{
    public class S
    {
        public static int EXIST = -2;
        public static int ERROR = -1;
        public static int NOTFOUND = 0;
        public static int SUCCESS = 1;
    }
    public class FileInfo
    {
        [J("_id", NullValueHandling = N.Ignore)]
        public MongoDB.Bson.ObjectId? _id { get; set; }
        [J("store_type")]
        public string store_type { get; set; } //FS - DB
        [J("file_name_org")]
        public string file_name_org { get; set; }
        [J("file_name_gen")]
        public string file_name_gen { get; set; }
        [J("file_mime")]
        public string file_mime { get; set; }
        [J("file_size")]
        public long file_size { get; set; }
        [J("file_path_src")]
        public string file_path_src { get; set; }
        [J("file_path_dest")]
        public string file_path_dest { get; set; }
        [J("upload_date")]
        public DateTime upload_date { get; set; }
        [J("ip", NullValueHandling = N.Ignore)]
        public string ip { get; set; }
        [J("image", NullValueHandling = N.Ignore)]
        public string image { get; set; }
    }

    public class UploadRequest
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }

    public class HTTPResponser
    {
        public int status { get; set; }

        public string message { get; set; }
        #nullable disable
        public dynamic? data { get; set; }
    }

    public static class SettingsENV
    {
        public static int _id { get; set; }
        public static int min_file_size { get; set; } = int.Parse(GetEnvironmentVariable("min_file_size"));
        public static int max_file_size { get; set; } = int.Parse(GetEnvironmentVariable("max_file_size"));
        public static string destination_path { get; set; } = GetEnvironmentVariable("destination_path");
        public static string allow_mimes { get; set; } = GetEnvironmentVariable("allow_mimes");
        public static bool allow_rename { get; set; } = bool.Parse(GetEnvironmentVariable("allow_rename"));
        public static bool allow_override { get; set; } = bool.Parse(GetEnvironmentVariable("allow_override"));
        public static string store_type { get; set; } = GetEnvironmentVariable("store_type"); //FS - DB - BOTH
    }
    public class Settings
    {
        [BsonIgnoreIfNull]
        [J("_id", NullValueHandling = N.Ignore)]
        public int _id { get; set; }
        [J("min_file_size", NullValueHandling = N.Ignore)]
        public int min_file_size { get; set; }
        [J("max_file_size", NullValueHandling = N.Ignore)]
        public int max_file_size { get; set; }
        [J("destination_path", NullValueHandling = N.Ignore)]
        public string destination_path { get; set; }
        [J("allow_mimes", NullValueHandling = N.Ignore)]
        public string allow_mimes { get; set; }
        [J("allow_rename", NullValueHandling = N.Ignore)]
        public bool allow_rename { get; set; }
        [J("allow_override", NullValueHandling = N.Ignore)]
        public bool allow_override { get; set; }
        [J("store_type", NullValueHandling = N.Ignore)] 
        public string store_type { get; set; } //FS - DB - BOTH
    }
    public class Settings2
    {
        [J("min_file_size", NullValueHandling = N.Ignore)]
        public int min_file_size { get; set; }
        [J("max_file_size", NullValueHandling = N.Ignore)]
        public int max_file_size { get; set; }
        [J("destination_path", NullValueHandling = N.Ignore)]
        public string destination_path { get; set; }
        [J("allow_mimes", NullValueHandling = N.Ignore)]
        public string allow_mimes { get; set; }
        [J("allow_rename", NullValueHandling = N.Ignore)]
        public bool allow_rename { get; set; }
        [J("allow_override", NullValueHandling = N.Ignore)]
        public bool allow_override { get; set; }
    }

    public class LogWrite
    {
        public int status { get; set; }
        public string location { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }
    }
}
