namespace StudentManagement.ModelsView
{
    public class StudentFeeModelView : FeeModelView
    {
        public int IdStuFee { get; set; }
        public int IdStudent { get; set; }
        public string? NameStudent { get; set; }
        public double? TotalFee { get; set; }
        public DateTime? DateCreate { get; set; }

    }
}
