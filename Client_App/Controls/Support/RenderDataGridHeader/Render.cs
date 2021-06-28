using Avalonia.Controls;

namespace Client_App.Controls.Support.RenderDataGridHeader
{
    public class Render
    {
        public static Control GetControl(string Type)
        {
            Control ctrl = null;
            if (Type != "")
            {
                string? formT1 = Type.Split('/')[0];
                string? formT2 = Type.Split('/')[1];

                switch (formT1)
                {
                    case "0":
                        ctrl = Support.RenderDataGridHeader.Main.GetControl(formT2);
                        break;
                    case "1":
                        ctrl = Support.RenderDataGridHeader.Form1.GetControl(formT2);
                        break;
                    case "2":
                        ctrl = Support.RenderDataGridHeader.Form2.GetControl(formT2);
                        break;
                    case "3":
                        ctrl = Support.RenderDataGridHeader.Form3.GetControl(formT2);
                        break;
                    case "4":
                        ctrl = Support.RenderDataGridHeader.Form4.GetControl(formT2);
                        break;
                    case "5":
                        ctrl = Support.RenderDataGridHeader.Form5.GetControl(formT2);
                        break;
                }
            }
            return ctrl;
        }
    }
}
