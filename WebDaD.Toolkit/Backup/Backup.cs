using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace WebDaD.Toolkit.Backup
{
    public class Backup
    {
        private WebDaD.Toolkit.Database.Database db;
        private string dataFolder;
        private string backupFolder;
        private string appname;

        public Dictionary<DateTime,string> Backups
        {
            get
            {
                Dictionary<DateTime, string> files = new Dictionary<DateTime,string>();
                foreach (string file in  Directory.GetFiles(backupFolder, "*.zip"))
                {
                    if (file.Contains('_'))
                    {
                        string[] tmp = Path.GetFileNameWithoutExtension(file).Split('_');
                        if (tmp[0] != appname) break;
                        if (tmp[1] != "backup") break;
                        DateTime dt = DateTime.Parse(tmp[2]);
                        files.Add(dt, file);
                    }
                }
                return files;
            }
        }
        public string LastBackup
        {
            get
            {
                DateTime latest = DateTime.MinValue;
                foreach (KeyValuePair<DateTime,string> item in Backups)
                {
                    if (latest < item.Key) latest = item.Key;
                }
                return Backups[latest];
            }
        }
        public bool RecentBackup
        {
            get
            {
                DateTime latest = DateTime.MinValue;
                foreach (KeyValuePair<DateTime, string> item in Backups)
                {
                    if (latest < item.Key) latest = item.Key;
                }
                if (latest <= DateTime.Now.AddDays(-2)) return false;
                else return true;
            }
        }

        public Backup(WebDaD.Toolkit.Database.Database db, string dataFolder, string backupFolder, string appname)
        {
            this.db = db;
            this.dataFolder = dataFolder;
            this.backupFolder = backupFolder;
            this.appname = appname;
        }

        public bool Dump()
        {
            bool ok = true;
            string datazip = "data.zip";
            string filename = appname+"_backup_"+DateTime.Now.ToString("YYYY-MM-DD")+".zip";

            Directory.CreateDirectory(backupFolder + Path.DirectorySeparatorChar + "dump");

            ZipFile.CreateFromDirectory(dataFolder, backupFolder + Path.DirectorySeparatorChar + "dump" + Path.DirectorySeparatorChar + datazip);
            ok = db.Dump(backupFolder + Path.DirectorySeparatorChar + "dump" + Path.DirectorySeparatorChar + "database.zip");

            if (ok) ZipFile.CreateFromDirectory(backupFolder + Path.DirectorySeparatorChar + "dump", backupFolder + Path.DirectorySeparatorChar + filename);

            if (File.Exists(backupFolder + Path.DirectorySeparatorChar + filename))
            {
                ok = true;
                Directory.Delete(backupFolder + Path.DirectorySeparatorChar + "dump", true);
            }
            else ok = false;
            return ok;
        }
        public bool Restore(string sourceFile)
        {
            bool ok = true;
            Directory.CreateDirectory(backupFolder + Path.DirectorySeparatorChar + "restore");

            ZipFile.ExtractToDirectory(backupFolder + Path.DirectorySeparatorChar + sourceFile, backupFolder + Path.DirectorySeparatorChar + "restore");

            ok = db.Restore(backupFolder + Path.DirectorySeparatorChar + "database.zip");

           ImprovedExtractToDirectory(backupFolder + Path.DirectorySeparatorChar + "restore" + Path.DirectorySeparatorChar + "data.zip", dataFolder,Overwrite.Always);
           if (ok) Directory.Delete(backupFolder + Path.DirectorySeparatorChar + "restore");
           return ok;
        }
        private enum Overwrite
        {
            Always,
            IfNewer,
            Never
        }
        private void ImprovedExtractToDirectory(string sourceArchiveFileName,string destinationDirectoryName,Overwrite overwriteMethod = Overwrite.IfNewer)
        {
            //Opens the zip file up to be read
            using (ZipArchive archive = ZipFile.OpenRead(sourceArchiveFileName))
            {
                //Loops through each file in the zip file
                foreach (ZipArchiveEntry file in archive.Entries)
                {
                    ImprovedExtractToFile(file, destinationDirectoryName, overwriteMethod);
                }
            }
        }
        private void ImprovedExtractToFile(ZipArchiveEntry file,string destinationPath,Overwrite overwriteMethod = Overwrite.IfNewer)
        {
            //Gets the complete path for the destination file, including any
            //relative paths that were in the zip file
            string destinationFileName = Path.Combine(destinationPath, file.FullName);

            //Gets just the new path, minus the file name so we can create the
            //directory if it does not exist
            string destinationFilePath = Path.GetDirectoryName(destinationFileName);

            //Creates the directory (if it doesn't exist) for the new path
            Directory.CreateDirectory(destinationFilePath);

            //Determines what to do with the file based upon the
            //method of overwriting chosen
            switch (overwriteMethod)
            {
                case Overwrite.Always:
                    //Just put the file in and overwrite anything that is found
                    file.ExtractToFile(destinationFileName, true);
                    break;
                case Overwrite.IfNewer:
                    //Checks to see if the file exists, and if so, if it should
                    //be overwritten
                    if (!File.Exists(destinationFileName) || File.GetLastWriteTime(destinationFileName) < file.LastWriteTime)
                    {
                        //Either the file didn't exist or this file is newer, so
                        //we will extract it and overwrite any existing file
                        file.ExtractToFile(destinationFileName, true);
                    }
                    break;
                case Overwrite.Never:
                    //Put the file in if it is new but ignores the 
                    //file if it already exists
                    if (!File.Exists(destinationFileName))
                    {
                        file.ExtractToFile(destinationFileName);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
