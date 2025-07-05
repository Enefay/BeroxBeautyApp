using AutoMapper;
using BeroxApp.Customers;
using BeroxApp.Employees;
using BeroxApp.Finance;
using BeroxApp.Reservations;
using BeroxApp.Services;

namespace BeroxApp;

public class BeroxAppApplicationAutoMapperProfile : Profile
{
    public BeroxAppApplicationAutoMapperProfile()
    {
        // Service mappings
        CreateMap<Service, ServiceDto>();
        CreateMap<CreateUpdateServiceDto, Service>();

        // Customer mappings
        CreateMap<Customer, CustomerDto>();
        CreateMap<CreateUpdateCustomerDto, Customer>();

        // Employee mappings
        CreateMap<Employee, EmployeeDto>();
        CreateMap<CreateUpdateEmployeeDto, Employee>();

        // Reservation mappings
        CreateMap<Reservation, ReservationDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FullName))
            .ForMember(dest => dest.CustomerPhone, opt => opt.MapFrom(src => src.Customer.PhoneNumber));
        CreateMap<CreateReservationDto, Reservation>();
        CreateMap<UpdateReservationDto, Reservation>();

        // Expense mappings
        CreateMap<Expense, ExpenseDto>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.FullName : null));
        CreateMap<CreateUpdateExpenseDto, Expense>();
    }
}
