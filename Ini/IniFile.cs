using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace TechnoRex.Utils.Ini
{

    //Usage
    //Creates or loads an INI file in the same directory as your executable
    //named EXE.ini (where EXE is the name of your executable)
    //var MyIni = new IniFile();

    // Or specify a specific name in the current dir
    //var MyIni = new IniFile("Settings.ini");

    // Or specify a specific name in a specific dir
    //var MyIni = new IniFile(@"C:\Settings.ini");

    //MyIni.Write("DefaultVolume", "100");
    //MyIni.Write("HomePage", "http://www.google.com");

    //[MyProg]
    //DefaultVolume=100
    //HomePage=http://www.google.com

    //var DefaultVolume = IniFile.Read("DefaultVolume");
    //var HomePage = IniFile.Read("HomePage");
    //MyIni.Write("DefaultVolume", "100", "Audio");
    //MyIni.Write("HomePage", "http://www.google.com", "Web");

    //[Audio]
    //DefaultVolume=100

    //[Web]
    //HomePage=http://www.google.com

    //if(!MyIni.KeyExists("DefaultVolume", "Audio"))
    //{
    //    MyIni.Write("DefaultVolume", "100", "Audio");
    //}

    //MyIni.DeleteKey("DefaultVolume", "Audio");

    //MyIni.DeleteSection("Web");
    public class IniFile
    {
        private readonly string _path;
        private readonly string _exe = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string Default, StringBuilder retVal, int size, string filePath);


        public IniFile(string IniPath = null)
        {
            _path = new FileInfo(IniPath ?? _exe + ".ini").FullName.ToString();
        }

        public string Read(string Key, string Section = null)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section ?? _exe, Key, "", RetVal, 255, _path);
            return RetVal.ToString();
        }

        public void Write(string key, string value, string section = null)
        {
            WritePrivateProfileString(section ?? _exe, key, value, _path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? _exe);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? _exe);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }
    }
}