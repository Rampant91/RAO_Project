using Avalonia.Controls;

namespace Client_App.Controls.Support.RenderDataGridHeader
{
    public class Form4
    {

        public static Control GetControl(string type)
        {
            switch (type)
            {
                case "0": return Get0();
                case "1": return Get1();
            }
            return null;
        }

        private static Control Get0()
        {
            return null;
        }

        private static Control Get1()
        {
            return null;
        }
    }
}
