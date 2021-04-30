namespace Models.Attributes
{
    public class Form_ClassAttribute : System.Attribute
    {
        public string Name { get; set; }
        public Form_ClassAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
