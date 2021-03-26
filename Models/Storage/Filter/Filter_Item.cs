namespace Models.Storage.Filter
{
    /// <summary>
    ///  Элемент фильтра, для хранения значения и типа сравнения.
    /// </summary>
    /// <typeparam name="T">Тип значения</typeparam>
    public class Filter_Item<T>
    {
        /// <summary>
        ///  Конструктор Filter_Item.
        /// </summary>
        /// <param name="Path">Имя поля для сравнения.</param>
        /// <param name="Value">Значение.</param>
        /// <param name="Type">
        /// Тип сравнения
        /// <list type="">
        ///    <listheader>
        ///      <description>Возможные значения:</description>
        ///     </listheader>
        /// <item>- "&lt;="</item>
        /// <item>- "&lt;"</item>
        /// <item>- "="</item>
        /// <item>- "&gt;"</item>
        /// <item>- "&gt;="</item>
        /// </list>
        ///  </param>
        public Filter_Item(string Path, object Value, string Type)
        {
            this.Path = Path;
            this.Value = Value;
            this.Type = Type;
        }

        public bool CheckObject(Client_Model.Report obj)
        {
            bool flag = true;
            var prop = obj.GetType().GetProperty(Path);
            if (prop != null)
            {
                var val = prop.GetValue(obj);
                if (val != null)
                {
                    if (val.GetType() == Value.GetType())
                    {
                        var tmp1 = (dynamic)val;
                        var tmp2 = (dynamic)Value;
                        switch (Type)
                        {
                            case "<=":
                                {
                                    if (!(tmp1 <= tmp2))
                                    {
                                        flag = false;
                                    }
                                    break;
                                }
                            case "<":
                                {
                                    if (!(tmp1 < tmp2))
                                    {
                                        flag = false;
                                    }
                                    break;
                                }
                            case "=":
                                {
                                    if (!(tmp1 == tmp2))
                                    {
                                        flag = false;
                                    }
                                    break;
                                }
                            case ">":
                                {
                                    if (!(tmp1 > tmp2))
                                    {
                                        flag = false;
                                    }
                                    break;
                                }
                            case ">=":
                                {
                                    if (!(tmp1 >= tmp2))
                                    {
                                        flag = false;
                                    }
                                    break;
                                }
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        ///  Имя поля для сравнения.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///  Значение.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        ///  Тип сравнения.
        ///          /// <list type="">
        ///    <listheader>
        ///      <description>Возможные значения:</description>
        ///     </listheader>
        /// <item>- "&lt;="</item>
        /// <item>- "&lt;"</item>
        /// <item>- "="</item>
        /// <item>- "&gt;"</item>
        /// <item>- "&gt;="</item>
        /// </list>
        /// </summary>
        public string Type { get; set; }
    }
}
