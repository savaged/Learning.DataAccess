namespace Model
{
    public class ProgrammingLanguage : BaseModel
    {
        public string Name { get; set; }

        public int SafetyScore { get; set; }

        public int UsefulnessScore { get; set; }
    }
}
