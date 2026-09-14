namespace School_Managment_system.Models
{
    public class Cards
    {
        public int Id { get; set; }
        public string? Image{ get; set; } = "";
        public string? Video { get; set; } = "";


        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public int date { get; set; }
    }
}
