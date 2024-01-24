using AutoMapper;
using InsuranceManagement.Domain;
using InsuranceManagement.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsuranceManagement.Services
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            // InsuranceController
            CreateMap<Insurance, InsuranceDTO>().ReverseMap();
            
            CreateMap<Insurance, InsertInsuranceDTO>().ReverseMap();

            CreateMap<Insurance, UpdateInsuranceDTO>().ReverseMap();

            // FeedbackController
            CreateMap<Feedback, InsertFeedbackDTO>().ReverseMap();

            // PurchaseController
            CreateMap<Purchase, PurchaseDTO>().ReverseMap();

            // PaymentController
            CreateMap<Payment, InsertPaymentDTO>().ReverseMap();
            CreateMap<Payment, UpdatePaymentDTO>().ReverseMap();

        }
    }
}
