using AutoMapper;
using BlindBox.Models;

namespace BilndBox.Dto
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<StaffDto, Staff>();
            CreateMap<Staff, StaffDto>();
        }

    }
}