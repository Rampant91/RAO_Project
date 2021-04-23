using System;
using System.Collections.Generic;
using System.Text;

namespace Collections
{
    public class Notes
    {
        public Models.Note this[string key]
        {
            get
            {
                return null;
            }
        }

        public IEnumerator<Models.Note> GetAllElements
        {
            get
            {
                yield break;
            }
        }

        public void Add(Models.Note item)
        {

        }
        public void RemoveAt(int ID)
        {

        }
    }
}
