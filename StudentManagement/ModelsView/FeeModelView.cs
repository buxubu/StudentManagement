namespace StudentManagement.ModelsView
{
    public class FeeModelView
    {
        public int IdFees { get; set; }
        public int IdSub { get; set; }
        public string? NameSub { get; set; }
        public string? Description { get; set; }
        public int? TotalCreditsOfSub { get; set; }
        public int? TheoryCreditsOfSub { get; set; }
        public int? PracticeCreditsOfSub { get; set; }
        public double? DefaultMoneyOfSub { get; set; }
        public double? Amount { get; set; }
    }

}
