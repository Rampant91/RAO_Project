using System.Collections.Generic;
using System.Drawing;

namespace Models.Interfaces;

public enum ColorType {
    None,
    Green,
    Yellow
}
public interface IBaseColor
{
    public static List<Color> ColorTypeList = new()
    {
        Color.White,
        Color.FromArgb(100, 0, 255, 0),
        Color.FromArgb(100, 255, 255, 150)
    };
    ColorType BaseColor { get; set; }
}