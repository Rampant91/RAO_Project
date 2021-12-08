using System.Collections.Generic;
using Models.DataAccess;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using Models.Collections;
using Models.Attributes;
using OfficeOpenXml;

namespace Models
{
    public class Note : IKey
    {
        public Note()
        {
            Init();
        }
        public Note(string rowNumber, string graphNumber, string comment)
        {
            RowNumber.Value = rowNumber;
            GraphNumber.Value = graphNumber;
            Comment.Value = comment;
            Init();
        }
        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }
        [NotMapped]
        Dictionary<string, RamAccess> Dictionary { get; set; } = new Dictionary<string, RamAccess>();
        public void Init()
        {
            RowNumber_Validation(RowNumber);
            GraphNumber_Validation(GraphNumber);
            Comment_Validation(Comment);
        }

        public int Id { get; set; }

        #region RowNUmber
        public string? RowNumber_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("№ строки")]
        #nullable enable
        public RamAccess<string?> RowNumber
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(RowNumber)))
                {
                    ((RamAccess<string?>)Dictionary[nameof(RowNumber)]).Value = RowNumber_DB;
                    return (RamAccess<string?>)Dictionary[nameof(RowNumber)];
                }
                else
                {
                    var rm = new RamAccess<string?>(RowNumber_Validation, RowNumber_DB);
                    rm.PropertyChanged += RowNumberValueChanged;
                    Dictionary.Add(nameof(RowNumber), rm);
                    return (RamAccess<string?>)Dictionary[nameof(RowNumber)];
                }
            }
            set
            {
                RowNumber_DB = value.Value;
                OnPropertyChanged(nameof(RowNumber));
            }
        }
        private void RowNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RowNumber_DB = ((RamAccess<string?>)Value).Value;
            }
        }
        private bool RowNumber_Validation(RamAccess<string?> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region GraphNumber
        public string? GraphNumber_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("№ графы")]
        #nullable enable
        public RamAccess<string?> GraphNumber
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(GraphNumber)))
                {
                    ((RamAccess<string?>)Dictionary[nameof(GraphNumber)]).Value = GraphNumber_DB;
                    return (RamAccess<string?>)Dictionary[nameof(GraphNumber)];
                }
                else
                {
                    var rm = new RamAccess<string?>(GraphNumber_Validation, GraphNumber_DB);
                    rm.PropertyChanged += GraphNumberValueChanged;
                    Dictionary.Add(nameof(GraphNumber), rm);
                    return (RamAccess<string?>)Dictionary[nameof(GraphNumber)];
                }
            }
            set
            {
                GraphNumber_DB = value.Value;
                OnPropertyChanged(nameof(GraphNumber));
            }
        }
        private void GraphNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                GraphNumber_DB = ((RamAccess<string?>)Value).Value;
            }
        }
        private bool GraphNumber_Validation(RamAccess<string?> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region Comment
        public string? Comment_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Пояснение")]
        public RamAccess<string> Comment
        {
            get
            {
                if (Dictionary.ContainsKey(nameof(Comment)))
                {
                    ((RamAccess<string>)Dictionary[nameof(Comment)]).Value = Comment_DB;
                    return (RamAccess<string>)Dictionary[nameof(Comment)];
                }
                else
                {
                    var rm = new RamAccess<string>(Comment_Validation, Comment_DB);
                    rm.PropertyChanged += CommentValueChanged;
                    Dictionary.Add(nameof(Comment), rm);
                    return (RamAccess<string>)Dictionary[nameof(Comment)];
                }
            }
            set
            {
                Comment_DB = value.Value;
                OnPropertyChanged(nameof(Comment));
            }
        }
        private void CommentValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Comment_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool Comment_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        //Для валидации
        public bool Object_Validation()
        {
            return true;
        }
        //Для валидации

        //Property Changed
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed
        #region IExcel
        public int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
        {
            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = RowNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = GraphNumber_DB;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = Comment_DB;

            return 3;
        }

        public static int ExcelHeader(ExcelWorksheet worksheet, int Row, int Column, bool Transpon = true)
        {
            worksheet.Cells[Row + (Transpon == false ? 0 : 0), Column + (Transpon == true ? 0 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Note,Models").GetProperty(nameof(RowNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 1 : 0), Column + (Transpon == true ? 1 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Note,Models").GetProperty(nameof(GraphNumber)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[Row + (Transpon == false ? 2 : 0), Column + (Transpon == true ? 2 : 0)].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Note,Models").GetProperty(nameof(Comment)).GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            return 3;
        }
        #endregion
    }
}
