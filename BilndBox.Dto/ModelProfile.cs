using AutoMapper;
using BilndBox.Dto.Entity;
using BlindBox.Models;

namespace BilndBox.Dto
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<StaffDto, Staff>();
            CreateMap<Staff, StaffDto>();

            CreateMap<GradeDto, Grade>();
            CreateMap<Grade, GradeDto>();

            CreateMap<DescribeTypeDto, DescribeType>();
            CreateMap<DescribeType, DescribeTypeDto>();
        }

    }
}