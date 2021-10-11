using Avalonia.Controls;
using System.Collections.Generic;
using Client_App.Controls.DataGrid;

namespace Client_App.Controls.Support.RenderDataGridHeader
{
    public class Render
    {
        public static List<Row> GetControl(string Type)
        {
            List<Row> ctrl = null;
            if (Type != "")
            {
                var formT1 = Type.Split('.')[0];
                var formT2 = Type.Split('.')[1];

                switch (formT1)
                {
                    case "0":
                        ctrl = Main.GetControl(formT2);
                        break;
                    case "1":
                        ctrl = Form1.GetControl(formT2);
                        break;
                    //case "2":
                    //    ctrl = Form2.GetControl(formT2);
                    //    break;
                    //case "3":
                    //    ctrl = Form3.GetControl(formT2);
                    //    break;
                    //case "4":
                    //    ctrl = Form4.GetControl(formT2);
                    //    break;
                    //case "5":
                    //    ctrl = Form5.GetControl(formT2);
                    //    break;
                }
            }

            return ctrl;
        }
    }
}