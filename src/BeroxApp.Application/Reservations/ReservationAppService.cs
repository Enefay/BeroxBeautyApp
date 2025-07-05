using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeroxApp.ReservationServiceEmployees;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BeroxApp.Reservations
{
    [Authorize]
    public class ReservationAppService : ApplicationService, IReservationAppService
    {
        private readonly IRepository<Reservation, Guid> _reservationRepository;
        private readonly IRepository<Services.Service, Guid> _serviceRepository;
        private readonly IRepository<Customers.Customer, Guid> _customerRepository;
        private readonly IRepository<Employees.Employee, Guid> _employeeRepository;

        public ReservationAppService(
            IRepository<Reservation, Guid> reservationRepository,
            IRepository<Services.Service, Guid> serviceRepository,
            IRepository<Customers.Customer, Guid> customerRepository,
            IRepository<Employees.Employee, Guid> employeeRepository)
        {
            _reservationRepository = reservationRepository;
            _serviceRepository = serviceRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<ReservationDto> GetAsync(Guid id)
        {
            var queryable = await _reservationRepository.WithDetailsAsync(
                x => x.Customer,
                x => x.ReservationServices
            );

            var reservation = await AsyncExecuter.FirstOrDefaultAsync(
                queryable.Where(x => x.Id == id)
            );

            return ObjectMapper.Map<Reservation, ReservationDto>(reservation);
        }

        public async Task<PagedResultDto<ReservationDto>> GetListAsync(GetReservationListDto input)
        {
            var queryable = await _reservationRepository.WithDetailsAsync(
                x => x.Customer,
                x => x.ReservationServices
            );

            // Filtreleme
            queryable = queryable
                .WhereIf(input.StartDate.HasValue, x => x.ReservationDate >= input.StartDate.Value)
                .WhereIf(input.EndDate.HasValue, x => x.ReservationDate <= input.EndDate.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == input.Status.Value);

            // Sıralama
            queryable = queryable.OrderByDescending(x => x.ReservationDate);

            var totalCount = await AsyncExecuter.CountAsync(queryable);

            var reservations = await AsyncExecuter.ToListAsync(
                queryable.PageBy(input.SkipCount, input.MaxResultCount)
            );

            var dtos = ObjectMapper.Map<List<Reservation>, List<ReservationDto>>(reservations);

            return new PagedResultDto<ReservationDto>(totalCount, dtos);
        }

        public async Task<ReservationDto> CreateAsync(CreateReservationDto input)
        {
            if (input.Services == null || !input.Services.Any())
            {
                throw new BusinessException("Reservation:MustHaveAtLeastOneService");
            }

            decimal totalServicePrice = 0;

            // Tüm servis bilgilerini çek ve fiyatları topla
            var serviceIds = input.Services.Select(x => x.ServiceId).Distinct().ToList();
            var services = await _serviceRepository.GetListAsync(x => serviceIds.Contains(x.Id));
            var serviceDict = services.ToDictionary(x => x.Id, x => x);

            var reservation = new Reservation(
                GuidGenerator.Create(),
                input.CustomerId,
                input.ReservationDate,
                0 // geçici olarak sıfır veriyoruz
            )
            {
                Notes = input.Notes
            };

            foreach (var item in input.Services)
            {
                if (!serviceDict.TryGetValue(item.ServiceId, out var service))
                {
                    throw new BusinessException("Reservation:InvalidService").WithData("serviceId", item.ServiceId);
                }

                var entry = new ReservationServiceEmployee
                {
                    ReservationId = reservation.Id,
                    ServiceId = item.ServiceId,
                    EmployeeId = item.EmployeeId
                };

                reservation.ReservationServices.Add(entry);

                totalServicePrice += service.Price;
            }

            reservation.ServicePrice = totalServicePrice;
            reservation.FinalPrice = totalServicePrice;

            await _reservationRepository.InsertAsync(reservation);

            return await GetAsync(reservation.Id);
        }

        public async Task<ReservationDto> UpdateAsync(Guid id, UpdateReservationDto input)
        {
            var reservation = await _reservationRepository.GetAsync(id);

            reservation.ReservationDate = input.ReservationDate;
            reservation.Notes = input.Notes;

            if (input.DiscountAmount.HasValue)
            {
                reservation.ApplyDiscount(input.DiscountAmount.Value);
            }

            if (input.ExtraAmount.HasValue)
            {
                reservation.AddExtraCharge(input.ExtraAmount.Value);
            }

            await _reservationRepository.UpdateAsync(reservation);

            return await GetAsync(reservation.Id);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _reservationRepository.DeleteAsync(id);
        }

        public async Task<ReservationDto> ApproveAsync(Guid id)
        {
            var reservation = await _reservationRepository.GetAsync(id);
            reservation.ApproveReservation();
            await _reservationRepository.UpdateAsync(reservation);

            return await GetAsync(reservation.Id);
        }

        public async Task<ReservationDto> CompleteAsync(Guid id)
        {
            var reservation = await _reservationRepository.GetAsync(id);
            reservation.CompleteReservation();
            await _reservationRepository.UpdateAsync(reservation);

            return await GetAsync(reservation.Id);
        }

        public async Task<ReservationDto> CancelAsync(Guid id)
        {
            var reservation = await _reservationRepository.GetAsync(id);
            reservation.CancelReservation();
            await _reservationRepository.UpdateAsync(reservation);

            return await GetAsync(reservation.Id);
        }

        public async Task<ListResultDto<ReservationDto>> GetTodayReservationsAsync()
        {
            var today = DateTime.Now.Date;
            var tomorrow = today.AddDays(1);

            var queryable = await _reservationRepository.WithDetailsAsync(
                x => x.Customer,
                x => x.ReservationServices
            );

            var reservations = await AsyncExecuter.ToListAsync(
                queryable.Where(x => x.ReservationDate >= today && x.ReservationDate < tomorrow)
                        .OrderBy(x => x.ReservationDate)
            );

            var dtos = ObjectMapper.Map<List<Reservation>, List<ReservationDto>>(reservations);

            return new ListResultDto<ReservationDto>(dtos);
        }
    }
}