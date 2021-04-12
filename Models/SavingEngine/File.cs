using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Models.Saving
{
    public class File:SavingEngine
    {
        /// <summary>
        /// Сохраняет формы в файл
        /// </summary>
        public async Task Save(Forms dictionary, string path)
        {
            if (path != "")
            {
                Serrialize(dictionary, path);
            }
            else
            {
            }
        }
        /// <summary>
        /// Загружает формы
        /// </summary>
        public async Task<Forms> Load(string path)
        {
            if (path != "")
            {
                return Deserrialize(path);
            }
            else
            {
                return null;
            }
        }


        object locker = new object();
        /// <summary>
        /// Функция серриализует формы
        /// </summary>
        //Сделать
        void Serrialize(Forms dictionary,string path)
        {
            lock (locker)
            {
                if (!System.IO.File.Exists(path))
                {
                    System.IO.File.Create(path);
                }
                LocalStorage[] lt = new LocalStorage[dictionary.Forms.Values.Count];
                dictionary.Forms.Values.CopyTo(lt, 0);
                XmlSerializer ser = new XmlSerializer(typeof(Client_Model.Form[]));
                StreamWriter wrt = new StreamWriter(path);
                ser.Serialize(wrt, lt);
                wrt.Close();
            }
        }

        /// <summary>
        /// Функция десерриализует формы 
        /// </summary>
        Forms Deserrialize(string path)
        {
            Forms dict = null;
            lock (locker)
            {
                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        dict = new Forms();
                        XmlSerializer ser = new XmlSerializer(typeof(Forms));
                        StreamReader wrt = new StreamReader(path);
                        var tl = (Forms)ser.Deserialize(wrt);
                        wrt.Close();
                        dict = tl;
                    }
                    catch
                    {
                        dict = null;
                    }
                }
            }
            return dict;
        }
    }
}
