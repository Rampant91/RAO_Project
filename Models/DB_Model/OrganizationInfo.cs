using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DB_Model
{
    class OrganizationInfo
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        private OrgInfoPath _orgInfoPath = new OrgInfoPath();
        public OrgInfoPath OrgInfoPath
        {
            get
            {
                return _orgInfoPath;
            }
            set
            {
                _orgInfoPath = value;
            }
        }
        public OrganizationInfo() { }
        public OrganizationInfo(int id, OrgInfoPath orgInfoPath)
        {
            Id = id;
            OrgInfoPath = orgInfoPath;
        }
    }
}
