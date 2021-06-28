using Avalonia.Controls;

namespace Client_App.Controls.Support.RenderDataGridRow
{
    public class Render
    {
        public static Control GetControl(string Type, int Row, INameScope scp, string TopName)
        {
            Control ctrl = null;
            if (Type != "")
            {
                string? formT1 = Type.Split('/')[0];
                string? formT2 = Type.Split('/')[1];

                switch (formT1)
                {
                    case "0":
                        ctrl = Support.RenderDataGridRow.Main.GetControl(formT2, Row, scp, TopName);
                        break;
                    case "1":
                        ctrl = Support.RenderDataGridRow.Form1.GetControl(formT2, Row, scp, TopName);
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
