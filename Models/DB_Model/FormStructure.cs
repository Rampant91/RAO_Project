using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DB_Model
{
    public enum ValidationType
    {
        type1,
        type2,
        type3,
        none
    }
    class FormStructure
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
        private Path _path = new Path();
        public Path Path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }
        private SPath _sPath = new SPath();
        public SPath SPath
        {
            get
            {
                return _sPath;
            }
            set
            {
                _sPath = value;
            }
        }
        private FormStructureInfo _formStructureInfo = new FormStructureInfo();
        public FormStructureInfo FormStructureInfo
        {
            get
            {
                return _formStructureInfo;
            }
            set
            {
                _formStructureInfo = value;
            }
        }
        private PathInfo _pathInfo = new PathInfo();
        public PathInfo PathInfo
        {
            get
            {
                return _pathInfo;
            }
            set
            {
                _pathInfo = value;
            }
        }
        private string _valueType = "";
        public string ValueType
        {
            get
            {
                return _valueType;
            }
            set
            {
                _valueType = value;
            }
        }
        private ValidationType _validationType = ValidationType.none;
        public ValidationType TypeOfValidation
        {
            get
            {
                return _validationType;
            }
            set
            {
                _validationType = value;
            }
        }
        public FormStructure() { }
        public FormStructure(int id, Path path, SPath spath, FormStructureInfo formStructureInfo, 
            PathInfo pathInfo, string valueType, ValidationType validationType)
        {
            Id = id;
            Path = path;
            SPath = spath;
            FormStructureInfo = formStructureInfo;
            PathInfo = pathInfo;
            ValueType = valueType;
            TypeOfValidation = validationType;
        }
    }
}
