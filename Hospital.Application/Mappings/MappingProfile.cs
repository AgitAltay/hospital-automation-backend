using AutoMapper;
using Hospital.Domain.Entities;
using Hospital.Application.DTOs.AppointmentDTOs;
using Hospital.Application.DTOs.SpecialtyDTOs; 

namespace Hospital.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();

            CreateMap<Appointment, AppointmentListDto>();


            CreateMap<Specialty, SpecialtyDto>();
            CreateMap<CreateSpecialtyDto, Specialty>();
            CreateMap<UpdateSpecialtyDto, Specialty>();
        }
    }
}