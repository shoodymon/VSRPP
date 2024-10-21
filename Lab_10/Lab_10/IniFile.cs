using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace IniFiles
{
    /// <summary>
    /// Ini-файлы лежат в папке рядом с программой, что позволяет пользователю изменить настройки вне-программы. 
    /// Данный способ хорош, если настройки программы заносятся вручную. 
    /// Например, эмулятор для запуска игры без лицензии (тот же revLoader).
    /// </summary>
    public class INIFile
    {
        private string filePath;

        // Импорт функции GetPrivateProfileString (для чтения значений) из библиотеки kernel32.dll
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // Импорт функции WritePrivateProfileString (для записи значений) из библиотеки kernel32.dll
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        // С помощью конструктора записываем пусть до файла и его имя
        public INIFile(string filePath)
        {
            this.filePath = filePath;
        }

        // Записываем в ini-файл. Запись происходит в выбранную секцию в выбранный ключ.
        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.filePath);
        }

        // Читаем ini-файл и возвращаем значение указного ключа из заданной секции.
        public string Read(string section, string key)
        {
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", sb, 255, this.filePath);
            return sb.ToString();
        }

        // Проверяем, есть ли такой ключ, в этой секции
        public bool KeyExists(string section, string key)
        {
            return Read(section, key).Length > 0;
        }
    }
}
