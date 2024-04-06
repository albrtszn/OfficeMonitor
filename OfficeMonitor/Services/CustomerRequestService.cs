using AutoMapper;
using CRUD.implementation;
using OfficeMonitor.DataBase.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class CustomerRequestService
    {
        private CustomerRequestRepo CustomerRequestRepo;
        private IMapper mapper;
        public CustomerRequestService(CustomerRequestRepo _CustomerRequestRepo, IMapper _mapper)
        {
            CustomerRequestRepo = _CustomerRequestRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await CustomerRequestRepo.DeleteById(id);
        }

        public async Task<List<CustomerRequest>> GetAll()
        {
            return await CustomerRequestRepo.GetAll();
        }

        public async Task<List<CustomerRequestDto>> GetAllDtos()
        {
            List<CustomerRequestDto> CustomerRequests = new List<CustomerRequestDto>();
            List<CustomerRequest> list = await CustomerRequestRepo.GetAll();
            list.ForEach(x => CustomerRequests.Add(mapper.Map<CustomerRequestDto>(x)));
            return CustomerRequests;
        }

        public async Task<CustomerRequest?> GetById(int id)
        {
            return await CustomerRequestRepo.GetById(id);
        }

        public async Task<CustomerRequestDto> GetDtoById(int id)
        {
            return mapper.Map<CustomerRequestDto>(await CustomerRequestRepo.GetById(id));
        }

        public async Task<bool> Save(CustomerRequest CustomerRequestToSave)
        {
            return await CustomerRequestRepo.Save(CustomerRequestToSave);
        }

        public async Task<bool> Save(CustomerRequestDto CustomerRequestDtoToSave)
        {
            return await CustomerRequestRepo.Save(mapper.Map<CustomerRequest>(CustomerRequestDtoToSave));
        }
    }
}
