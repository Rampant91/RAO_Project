using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Models.Storage
{
    public class File:SavingEngine
    {
        /// <summary>
        /// Сохраняет формы в файл
        /// </summary>
        public async Task Save(LocalDictionary dictionary, string path)
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
        public async Task<LocalDictionary> Load(string path)
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
        void Serrialize(LocalDictionary dictionary,string path)
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
        LocalDictionary Deserrialize(string path)
        {
            LocalDictionary dict = null;
            lock (locker)
            {
                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        dict = new LocalDictionary();
                        XmlSerializer ser = new XmlSerializer(typeof(LocalDictionary));
                        StreamReader wrt = new StreamReader(path);
                        var tl = (LocalDictionary)ser.Deserialize(wrt);
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
