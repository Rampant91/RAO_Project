using System.Collections.Generic;

namespace Client_App;

public static class ErrorMessages
{
    #region Error1
    public static List<string> Error1=
        new(new string[] { 
            "Программа не может обработать функцию нахождения системной директории!\n Local Error: 1 \n Программа будет закрыта" ,
            "Ок"
        });
    #endregion

    #region Error2
    public static List<string> Error2 =
        new(new string[] {
            "Программа не может обработать функцию создания системной директории: temp!\n Local Error: 2 \n Программа будет закрыта" ,
            "Ок"
        });
    #endregion

    #region Error3
    public static List<string> Error3 =
        new(new string[] {
            "Программа не может обработать функцию удаления системной директории: temp!\n Local Error: 3 \n Программа будет закрыта" ,
            "Ок"
        });
    #endregion
}