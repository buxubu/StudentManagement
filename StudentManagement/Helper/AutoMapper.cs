using AutoMapper;
using StudentManagement.Models;
using StudentManagement.ModelsView;

namespace BlogMVC.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Student, StudentModelView>();
            CreateMap<StudentModelView, Student>();
            CreateMap<StudentFee, StudentFeeModelView>();
            CreateMap<StudentFeeModelView, StudentFee>();
        }
    }
}
