namespace Models.Attributes;

public class Form_ClassAttribute(string name) : System.Attribute
{
    public string Name { get; set; } = name;
}