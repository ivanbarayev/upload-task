using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace upload_task.Models
{
    public class UploadModel
    {
        private Common cm = new Common();
        private string current_path = Directory.GetCurrentDirectory();
        public readonly IMongoDatabase db;
        private static string collection_name = "uploads";
        private int END = S.SUCCESS;
        private readonly IMongoCollection<FileInfo> _upload;
        private string img_base64 = null;
        private string file_path_dest = null;
        private string generated_name = null;

        public UploadModel()
        {
            try
            {
                var settings = MongoClientSettings.FromConnectionString("mongodb+srv://ivanbarayev:dobjdw7BMzdcKpJG@docdb.xmg8e.mongodb.net/upload_db?retryWrites=true&w=majority");
                var client = new MongoClient(settings);
                db = client.GetDatabase("upload_db");
                _upload = db.GetCollection<FileInfo>(collection_name);
            }
            catch (Exception ex)
            {
                cm.LogWriter(S.SUCCESS, "UploadModel.Constructor", ex.Message);
            }
        }


        public int UploadInsertFS(HttpRequest req)
        {
            var file = req.Form.Files[0];
            cm.ChkDir2(SettingsENV.destination_path);
            file_path_dest = (SettingsENV.allow_rename == true ? Guid.NewGuid().ToString() + file.FileName.Split(".")[1] : null);
            var file_name = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            generated_name = (SettingsENV.allow_rename == true ? cm.FileRenamer(file_name) : file.FileName);
            var fs_path = Path.Combine(SettingsENV.destination_path, generated_name);
            

            using (var stream = new FileStream(fs_path, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (SettingsENV.store_type == "DB")
            {
                generated_name = null;
                byte[] b = File.ReadAllBytes(fs_path);
                img_base64 = "data:image/png;base64," + Convert.ToBase64String(b);
                File.Delete(fs_path);
            }

            FileInfo structer = new FileInfo
            {
                store_type = SettingsENV.store_type,
                file_name = generated_name,
                file_mime = file.ContentType,
                file_size = file.Length,
                file_path_dest = (SettingsENV.store_type == "FS" ? SettingsENV.destination_path : null),
                upload_date = DateTime.Now,
                image = img_base64
            };

            UploadInsertDB(structer);
            return END;
        }


        public async Task<int> UploadInsertDB(FileInfo data)
        {
            try
            {
                var collection = db.GetCollection<FileInfo>(collection_name);
                collection.InsertOne(data);
                END = S.SUCCESS;
            }
            catch (Exception ex)
            {
                await cm.LogWriter(S.ERROR, "UploadModel.UploadInsertDB", ex.Message);
                END = S.ERROR;
            }

            return END;
        }


        public async Task<dynamic> UploadList(int? page, int? pageSize)
        {
            int currPage = page ?? 1;
            int size = pageSize ?? 5;
            int? skipCount = size * (currPage - 1);

            var projection = Builders<FileInfo>.Projection.Exclude("image");
            var document = _upload.Find(new BsonDocument()).Project<FileInfo>(projection).Skip(skipCount).Limit(size).ToList();
            long total = _upload.CountDocuments(new BsonDocument());
            int count = document.Count;

            var resp = new Pager
            {
                total = total,
                count = count,
                page = currPage,
                size = size,
                data = document
            };

            return resp;
        }


        public async Task<string> UploadView(string id)
        {
            FileInfo info = getFileInfo(id);
            if(info.store_type == "DB")
            {
                img_base64 = info.image;
            } else
            {
                var full_path = Path.Combine(info.file_path_dest.ToString(), info.file_name.ToString());
                img_base64 = "data:image/png;base64,"+Convert.ToBase64String(File.ReadAllBytes(full_path));
            }

            return img_base64;
        }


        public async Task<int> UploadDelete(string src, string id)
        {
            if (src == "FS")
            {
                FileInfo info = getFileInfo(id);
                var full_path = Path.Combine(info.file_path_dest.ToString(), info.file_name.ToString());
                File.Delete(full_path);
            }

            var resp = _upload.DeleteOne(a => a._id == id);
            END = int.Parse(resp.DeletedCount.ToString());
            return END;
        }

        public FileInfo getFileInfo(string id)
        {
            var projection = Builders<FileInfo>.Projection.Include("store_type").Include("file_name").Include("file_path_dest").Include("image");
            var resp = _upload.Find(dat => dat._id == id).Project<FileInfo>(projection).SingleOrDefault();

            return resp;
        }
    }
}
