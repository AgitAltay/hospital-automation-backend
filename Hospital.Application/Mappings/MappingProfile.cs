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
            Console.WriteLine("--> DIKKAT: MappingProfile constructor'ı çalıştı! Kurallar yükleniyor...");
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<UpdateAppointmentDto, Appointment>();

            
            CreateMap<CreateAppointmentPublicDto, Patient>();
            CreateMap<CreateAppointmentPublicDto, Appointment>();


            
            CreateMap<ValidatePatientDto, Patient>();

            CreateMap<Appointment, AppointmentListDto>();
            
            CreateMap<Specialty, SpecialtyDto>();
            CreateMap<CreateSpecialtyDto, Specialty>();
            CreateMap<UpdateSpecialtyDto, Specialty>();
        }
    }
}