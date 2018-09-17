namespace Sales01.Backend.Helpers
{
    using System;
    using System.IO;
    using System.Web;

    public class FilesHelper
    {
        public static string UploadPhoto(HttpPostedFileBase file, string folder)
        {
            string path = string.Empty;
            string pic = string.Empty;

            if (file != null)
            {
                var guid = Guid.NewGuid().ToString();
                var files = string.Format("{0}.jpg", guid);
                var fullPath = string.Format("{0}/{1}", folder, file);

                pic = Path.GetFileName(file.FileName);
                path = Path.Combine(HttpContext.Current.Server.MapPath(folder), pic);
                //path = Path.Combine(HttpContext.Current.Server.MapPath(folder), files);
                file.SaveAs(path);
            }

            return pic;
        }

        public static bool UploadPhoto(MemoryStream stream, string folder, string name)
        {
            try
            {
                stream.Position = 0;
                var path = Path.Combine(HttpContext.Current.Server.MapPath(folder), name);
                File.WriteAllBytes(path, stream.ToArray());
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static string UploadPhoto(HttpPostedFileBase file, string folder, string name)
        {

            string path = string.Empty;
            string Paths = string.Empty;
            string pic = string.Empty;

            if (file == null || string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            try
            {
                if (file != null)
                {
                    var guid = Guid.NewGuid().ToString();
                    var files = string.Format("{0}.jpg", guid + "_" + name);
                    var fullPath = string.Format("{0}/{1}", folder, files);

                    pic = Path.GetFileName(file.FileName);
                    //path = Path.Combine(HttpContext.Current.Server.MapPath(folder), pic);
                    path = Path.Combine(HttpContext.Current.Server.MapPath(folder), files);
                    file.SaveAs(path);

                    Paths = fullPath;
                }
            }
            catch (Exception)
            {

                return string.Empty;
            }

            return Paths;
        }

        public static string UploadPhoto(HttpPostedFileBase file, string folder, string name, string Id)
        {

            string path = string.Empty;
            string Paths = string.Empty;
            string pic = string.Empty;

            if (file == null || string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(Id))
            {
                return string.Empty;
            }

            try
            {
                if (file != null)
                {
                    var guid = Guid.NewGuid().ToString();
                    var files = string.Format("{0}.jpg", name + "_" + Id);
                    var fullPath = string.Format("{0}/{1}", folder, files);

                    pic = Path.GetFileName(file.FileName);
                    //path = Path.Combine(HttpContext.Current.Server.MapPath(folder), pic);
                    path = Path.Combine(HttpContext.Current.Server.MapPath(folder), files);
                    file.SaveAs(path);

                    Paths = fullPath;
                }
            }
            catch (Exception)
            {

                return string.Empty;
            }

            return Paths;
        }
    }
}