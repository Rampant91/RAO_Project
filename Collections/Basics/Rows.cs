using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Models;

namespace Collections.Basics
{
    public class Rows
    {
        public Models.Abstracts.Form this[string key]
        {
            get 
            {
                return null;
            }
        }

        public IEnumerator<Models.Abstracts.Form> GetAllElements
        {
            get
            {
                yield break;
            }
        }

        public void Add(Models.Abstracts.Form item)
        {

        }

        public void RemoveAt(int ID)
        {

        }
    }
}
