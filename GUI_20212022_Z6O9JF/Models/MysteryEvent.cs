namespace GUI_20212022_Z6O9JF.Models
{
    public class MysteryEvent
    {
        public string Description { get; set; }

        public int Number { get; set; }
        public string Resource { get; set; }

        public MysteryEvent(string description, int number, string resource)
        {
            Description = description;
            Number = number;
            Resource = resource;
        }
    }
}
