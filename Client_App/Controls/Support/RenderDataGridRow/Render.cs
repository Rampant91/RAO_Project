using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;

namespace Client_App.Controls.Support.RenderDataGridRow
{
    public class Render
    {
        public static Control GetControl(string Type,string Name,Object Context)
        {
            Control ctrl = null;
            if (Type != "")
            {
                var formT1 = Type.Split('/')[0];
                var formT2 = Type.Split('/')[1];

                switch (formT1)
                {
                    case "0":
                        ctrl = Support.RenderDataGridRow.Main.GetControl(formT2,Name,Context);
                        break;
                    case "1":
                        ctrl = Support.RenderDataGridRow.Form1.GetControl(formT2,Name,Context);
                        break;
                    case "2":
                        ctrl = Support.RenderDataGridRow.Form2.GetControl(formT2);
                        break;
                    case "3":
                        ctrl = Support.RenderDataGridRow.Form3.GetControl(formT2);
                        break;
                    case "4":
                        ctrl = Support.RenderDataGridRow.Form4.GetControl(formT2);
                        break;
                    case "5":
                        ctrl = Support.RenderDataGridRow.Form5.GetControl(formT2);
                        break;
                }
            }
            return ctrl;
        }
    }
}
