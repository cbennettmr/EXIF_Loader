using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

namespace EXIF
{
    class Program
    {
        // arg is JPG file or txt file containing list of JPG files.
        static void Main(string[] args)
        {
            if (args.Length == 0) return;
            string path = args[0];
            if (!File.Exists(path)) return;

            CreateDbIfNeeded();

            StringBuilder imgText = new StringBuilder();
            StringBuilder errText = new StringBuilder();
            string[] files = GetFileList(path);
            DateTime dt, orig, digit;
            foreach (var file in files) {
                try{
                using (var exif = new ExifLib.ExifReader(file)) {
                    exif.GetTagValue<DateTime>(ExifLib.ExifTags.DateTime, out dt);
                    exif.GetTagValue<DateTime>(ExifLib.ExifTags.DateTimeDigitized, out digit);
                    exif.GetTagValue<DateTime>(ExifLib.ExifTags.DateTimeOriginal, out orig);
                    imgText.AppendFormat("{0}\t{1}\t{2}\t{3}\n", file, dt, orig, digit);
                }} catch(Exception ex){
                    errText.AppendFormat("{0}: {1}\n",file,ex.Message);
                }
            }
            using (StreamWriter outfile = new StreamWriter(@"C:\temp\imagedata.txt")) {
                outfile.Write(imgText.ToString());

            }
            using (StreamWriter outfile = new StreamWriter(@"C:\temp\imagedataerrs.txt")) {
                outfile.Write(errText.ToString());

            }
        }

        private static void CreateDbIfNeeded() {
            throw new NotImplementedException();
        }

        private static string[] GetFileList(string path) {
            if (Path.GetExtension(path).ToLower() == ".jpg") {
                return new string[] { path };
            }

            return File.ReadAllLines(path);
        }
    }
}
