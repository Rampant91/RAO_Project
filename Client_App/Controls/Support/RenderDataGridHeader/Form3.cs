using Avalonia.Controls;

namespace Client_App.Controls.Support.RenderDataGridHeader
{
    public class Form3
    {
        public static Control GetControl(string type)
        {
            switch (type)
            {
                case "0": return Get0();
                case "1": return Get1();
                case "1_1": return Get1_1();
                case "2": return Get2();
                case "2_1": return Get2_1();
                case "2_2": return Get2_2();
                case "2_3": return Get2_3();
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

        private static Control Get1_1()
        {
            return null;
        }

        private static Control Get2()
        {
            return null;
        }

        private static Control Get2_1()
        {
            return null;
        }

        private static Control Get2_2()
        {
            return null;
        }

        private static Control Get2_3()
        {
            return null;
        }
    }
}
