using AutoMapper;
using Hospital.Domain.Entities;
using Hospital.Application.DTOs.AppointmentDTOs;

namespace Hospital.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();

            CreateMap<Appointment, AppointmentListDto>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FirstName + " " + src.Doctor.LastName))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient.FirstName + " " + src.Patient.LastName))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Doctor.Specialty.Name));
        }
    }
}