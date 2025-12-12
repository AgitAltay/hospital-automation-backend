using AutoMapper;
using Hospital.Domain.Entities;
using Hospital.Application.DTOs.AppointmentDTOs;
using Hospital.Application.DTOs.SpecialtyDTOs;
using Hospital.Application.DTOs.DoctorDTOs;
using Hospital.Domain.Enums;

namespace Hospital.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
             Console.WriteLine("--> DIKKAT: MappingProfile constructor'ı çalıştı! Kurallar yükleniyor...");
            

            CreateMap<CreateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppointmentStatus.Active));

            CreateMap<UpdateAppointmentDto, Appointment>();
            
            CreateMap<CreateAppointmentPublicDto, Patient>();

            CreateMap<CreateAppointmentPublicDto, Appointment>()
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Note))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppointmentStatus.Active));

            CreateMap<ValidatePatientDto, Patient>();

            CreateMap<Appointment, AppointmentListDto>()
  
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? $"{src.Doctor.FirstName} {src.Doctor.LastName}" : ""))
                .ForMember(dest => dest.PatientName, opt => opt.MapFrom(src => src.Patient != null ? $"{src.Patient.FirstName} {src.Patient.LastName}" : ""))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Doctor != null && src.Doctor.Specialty != null ? src.Doctor.Specialty.Name : ""));


  
            CreateMap<Specialty, SpecialtyDto>();
            CreateMap<CreateSpecialtyDto, Specialty>();
            CreateMap<UpdateSpecialtyDto, Specialty>();
            

            CreateMap<Doctor, DoctorDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.SpecialtyName, opt => opt.MapFrom(src => src.Specialty != null ? src.Specialty.Name : string.Empty));


            CreateMap<CreateDoctorDto, Doctor>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => RoleType.Doctor))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));

            
            CreateMap<UpdateDoctorDto, Doctor>();
        }
    }
}