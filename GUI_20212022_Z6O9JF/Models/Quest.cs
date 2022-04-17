namespace GUI_20212022_Z6O9JF.Models
{
    public class Quest
    {
        public int Id { get; }
        public string Name { get; set; }
        public bool Done { get; set; }
        public Quest(string Name, bool Done)
        {
            Id++;
            this.Name = Name;
            this.Done = Done;
        }
        public bool QuestDone(int count, int crit)
        {
            if (count >= crit)
            {
                return true;
            }
            return false;
        }
    }
}
