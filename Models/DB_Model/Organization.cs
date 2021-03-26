using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DB_Model
{
    class Organization
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
        private Form _form = new Form();
        public Form Form
        {
            get
            {
                return _form;
            }
            set
            {
                _form = value;
            }
        }
        private OrganizationInfo _organizationInfo = new OrganizationInfo();
        public OrganizationInfo OrganizationInfo
        {
            get
            {
                return _organizationInfo;
            }
            set
            {
                _organizationInfo = value;
            }
        }
        public Organization() { }
        public Organization(int id, Form form, OrganizationInfo organizationInfo)
        {
            Id = id;
            Form = form;
            OrganizationInfo = organizationInfo;
        }
    }
}
