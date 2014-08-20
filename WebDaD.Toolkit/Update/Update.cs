using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebDaD.Toolkit.Communications;

namespace WebDaD.Toolkit.Update
{
    public class Update
    {
        private string updatePath;
        private string apppath;
        private string appname;
        private string webappname;
        private double version;
        private string checkPath;
        private double recentVersion;
        private bool serverReachable;
        private WebDaD.Toolkit.Database.Database db;

        public Update(string apppath,string appname, double version,WebDaD.Toolkit.Database.Database db, string updatePath = "http://updates.webdad.eu/")
        {
            this.apppath = apppath;
            this.appname = appname;
            this.webappname = appname.ToLower().Replace(" ", "_");
            this.version = version;
            this.updatePath = updatePath;
            this.db = db;
            this.checkPath = updatePath +webappname + "/recentVersion";
            this.recentVersion = getRecentVersion();
            if (this.recentVersion == 0.0) { this.serverReachable = false; }
            else this.serverReachable = true;
        }

        private double getRecentVersion()
        {
            try
            {
                WebDaD.Toolkit.Communications.WebResponse w = Web.GetPageContent(this.checkPath);
                if (w.HttpResponseCode == HttpStatusCode.OK)
                    return Double.Parse(w.ReponseText);
                else return 0.0;
            }
            catch { return 0.0; }
        }

        public bool UpdateAvaiable
        {
            get
            {
                return this.recentVersion > this.version;
            }
        }
        public bool ServerReachable { get { return this.serverReachable; } }

        public bool PerformUpdate(string tempfolder)
        {
            //download update.zip from http://updates.webdad.eu/[appname]/[recentVersion]/update.zip
            WebDaD.Toolkit.Communications.WebResponse w = Web.GetFile("http://updates.webdad.eu/" + this.webappname + "/" + this.recentVersion + "/update.zip", tempfolder + Path.DirectorySeparatorChar + "update.zip");
            if (w.HttpResponseCode != HttpStatusCode.OK) return false;

            //unzip to tempfolder
            ZipFile.ExtractToDirectory(tempfolder + Path.DirectorySeparatorChar + "update.zip", tempfolder + Path.DirectorySeparatorChar + "update");

            //peform database changes from database_changes.txt
            string line = "";
            System.IO.StreamReader file = new System.IO.StreamReader(tempfolder + Path.DirectorySeparatorChar + "update" + Path.DirectorySeparatorChar + "database_changes.txt");
            while ((line = file.ReadLine()) != null)
            {
                this.db.Execute(line);
            }
            file.Close();

            //remove database_changes.txt
            File.Delete(tempfolder + Path.DirectorySeparatorChar + "update"+Path.DirectorySeparatorChar+"database_changes.txt");

            //copy files to apppath
            foreach (string f in Directory.GetFiles(tempfolder + Path.DirectorySeparatorChar + "update"))
            {
                File.Copy(f, this.apppath + Path.DirectorySeparatorChar + Path.GetFileName(f));
            }

            //remove content in tempfolder
            Directory.Delete(tempfolder + Path.DirectorySeparatorChar + "update", true);
            File.Delete(tempfolder + Path.DirectorySeparatorChar + "update.zip");

            return true;
        }

        /// <summary>
        /// Launches Path(startExe)/update_[this_version].exe AND then starts startExe again
        /// </summary>
        /// <param name="startExe">The software which called this method and should be started after this</param>
        /// <returns>True if the Update Exe could be called (may then kill the calling assmebly)</returns>
        public bool LaunchUpdater(string startExe="")
        {
            string updateExe = Path.GetDirectoryName(startExe) + Path.DirectorySeparatorChar + "update_" + Assembly.GetExecutingAssembly().GetName()
.Version.ToString().Replace(".", "_") + ".exe";
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = updateExe;

            startInfo.Arguments = "autostart ";
            startInfo.Arguments += "apppath=" + this.apppath + " ";
            startInfo.Arguments += "appname=" + this.appname + " ";
            startInfo.Arguments += "version=" + this.version.ToString() + " ";
            startInfo.Arguments += "db="+this.db.getType().ToString()+"|" + this.db.ConnectionString() + " ";
            startInfo.Arguments += "updatePath=" + this.updatePath;

            Process.Start(startInfo);
            return true;
        }
    }
}
