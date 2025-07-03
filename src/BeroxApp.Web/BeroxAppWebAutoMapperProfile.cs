using AutoMapper;
using BeroxApp.Customers;
using BeroxApp.Employees;
using BeroxApp.Services;
using BeroxApp.Web.Pages.Services;

namespace BeroxApp.Web;

public class BeroxAppWebAutoMapperProfile : Profile
{
    public BeroxAppWebAutoMapperProfile()
    {
        // Service mappings for modals
        CreateMap<ServiceDto, CreateUpdateServiceDto>();

        // Customer mappings for modals
        CreateMap<CustomerDto, CreateUpdateCustomerDto>();

        // Employee mappings for modals
        CreateMap<EmployeeDto, CreateUpdateEmployeeDto>();
    }
}
