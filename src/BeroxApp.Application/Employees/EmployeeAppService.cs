using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace BeroxApp.Employees
{
    [Authorize]
    public class EmployeeAppService : CrudAppService<Employee, EmployeeDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateEmployeeDto>, IEmployeeAppService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IIdentityRoleRepository _roleRepository;

        public EmployeeAppService(
            IRepository<Employee, Guid> repository,
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository) : base(repository)
        {
            _userManager = userManager;
            _roleRepository = roleRepository;
        }

        public async Task<ListResultDto<EmployeeDto>> GetActiveEmployeesAsync()
        {
            var employees = await Repository.GetListAsync(x => x.IsActive);
            var employeeDtos = ObjectMapper.Map<List<Employee>, List<EmployeeDto>>(employees);
            return new ListResultDto<EmployeeDto>(employeeDtos);
        }

        public override async Task<EmployeeDto> CreateAsync(CreateUpdateEmployeeDto input)
        {
            // Employee rolü kontrolü
            var employeeRole = await _roleRepository.FindByNormalizedNameAsync("CALISAN");
            if (employeeRole == null)
            {
                throw new BusinessException("BeroxApp:EmployeeRoleNotFound");
            }


            var firstName = input.FirstName
            .Replace("ı", "i")
            .Replace("ğ", "g")
            .Replace("ü", "u")
            .Replace("ş", "s")
            .Replace("ö", "o")
            .Replace("ç", "c")
            .Replace("İ", "i")
            .Replace("Ğ", "g")
            .Replace("Ü", "u")
            .Replace("Ş", "s")
            .Replace("Ö", "o")
            .Replace("Ç", "c")
            .ToLower();

            var lastName = input.LastName
                .Replace("ı", "i")
                .Replace("ğ", "g")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("ö", "o")
                .Replace("ç", "c")
                .Replace("İ", "i")
                .Replace("Ğ", "g")
                .Replace("Ü", "u")
                .Replace("Ş", "s")
                .Replace("Ö", "o")
                .Replace("Ç", "c")
                .ToLower();

            // Boşlukları kaldır ve kullanıcı adını oluştur
            var userName = $"{firstName}.{lastName}".Replace(" ", "");


            var email = input.Email ?? $"{userName}@beroxapp.com";
            var password = $"{userName}1*";

            // Kullanıcı oluştur
            var user = new IdentityUser(
                GuidGenerator.Create(),
                userName,
                email,
                CurrentTenant.Id
            );

            user.Name = input.FirstName;
            user.Surname = input.LastName;
            user.SetPhoneNumber(input.PhoneNumber, false);
            user.SetIsActive(input.IsActive);

            // Kullanıcıyı kaydet
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessException("BeroxApp:UserCreationFailed").WithData("Errors", errors);
            }

            // Employee rolünü ata
            await _userManager.AddToRoleAsync(user, "Calisan");

            // Çalışan oluştur
            var employee = new Employee(
                GuidGenerator.Create(),
                input.FirstName,
                input.LastName
            )
            {
                PhoneNumber = input.PhoneNumber,
                Email = email,
                MonthlySalary = input.MonthlySalary,
                IsActive = input.IsActive,
                UserId = user.Id
            };

            await Repository.InsertAsync(employee,true);
            return ObjectMapper.Map<Employee, EmployeeDto>(employee);
        }

        public override async Task<EmployeeDto> UpdateAsync(Guid id, CreateUpdateEmployeeDto input)
        {
            var employee = await Repository.GetAsync(id);

            // Kullanıcıyı güncelle
            if (employee.UserId.HasValue)
            {
                var user = await _userManager.GetByIdAsync(employee.UserId.Value);
                if (user != null)
                {
                    user.Name = input.FirstName;
                    user.Surname = input.LastName;
                    user.SetPhoneNumber(input.PhoneNumber, false);
                    user.SetIsActive(input.IsActive);

                    // Email güncelleme
                    if (!string.IsNullOrEmpty(input.Email) && user.Email != input.Email)
                    {
                        await _userManager.SetEmailAsync(user, input.Email);
                    }

                    await _userManager.UpdateAsync(user);
                }
            }

            // Çalışanı güncelle
            employee.FirstName = input.FirstName;
            employee.LastName = input.LastName;
            employee.PhoneNumber = input.PhoneNumber;
            employee.Email = input.Email;
            employee.MonthlySalary = input.MonthlySalary;
            employee.IsActive = input.IsActive;

            await Repository.UpdateAsync(employee);
            return ObjectMapper.Map<Employee, EmployeeDto>(employee);
        }

        public override async Task DeleteAsync(Guid id)
        {
            var employee = await Repository.GetAsync(id);

            // Kullanıcıyı sil
            if (employee.UserId.HasValue)
            {
                var user = await _userManager.GetByIdAsync(employee.UserId.Value);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            await Repository.DeleteAsync(id);
        }
    }
}