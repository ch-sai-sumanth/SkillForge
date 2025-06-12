using Application.DTOs;
using AutoMapper;
using User.Domain.Entities;

namespace Application.Mappings;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<RecordPaymentDto, Payment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Pending"))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.RecordedAt, opt => opt.Ignore());

        CreateMap<Payment, PaymentDto>();
    }
}