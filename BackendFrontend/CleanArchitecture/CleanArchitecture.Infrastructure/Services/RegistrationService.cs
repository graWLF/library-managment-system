using AutoMapper;
using CleanArchitecture.Core.DTOs.Registration;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository _repository;
        private readonly IMapper _mapper;

        public RegistrationService(IRegistrationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Get all registrations
        public async Task<IEnumerable<RegistrationDTO>> GetAllAsync()
        {
            var registrations = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<RegistrationDTO>>(registrations);
        }
        // Get user info by username or email
        public async Task<RegistrationDTO> GetUserInfoAsync(string usernameOrEmail)
        {
            var registration = await _repository.GetUserInfoAsync(usernameOrEmail);
            return _mapper.Map<RegistrationDTO>(registration);
        }

        // Get a registration by ID
        public async Task<RegistrationDTO> GetByIDAsync(int ID)
        {
            var registration = await _repository.GetByIDAsync(ID);
            return _mapper.Map<RegistrationDTO>(registration);
        }

        // Create a new registration
        public async Task CreateAsync(RegistrationDTO dto)
        {
            var registration = _mapper.Map<Registration>(dto);
            await _repository.AddAsync(registration);
        }

        // Update an existing registration
        public async Task UpdateAsync(int ID, RegistrationDTO dto)
        {
            var existing = await _repository.GetByIDAsync(ID);
            if (existing == null) throw new Exception("Registration not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        // Delete a registration
        public async Task DeleteAsync(int ID)
        {
            var registration = await _repository.GetByIDAsync(ID);
            if (registration == null) throw new Exception("Registration not found");

            await _repository.DeleteAsync(registration);
        }
        // Check supervisor
        public bool CheckSupervisor(string username, string password)
        {
            bool registration = _repository.CheckSupervisor(username, password);
            return registration;

        }

        // Login a registration
        public bool Login(string username, string password)
        {
            bool registration = _repository.Login(username, password);
            //if (registration == false)
                // show error message


                //throw new Exception("Invalid credentials.");

            // Here you would typically check the password, etc.
            // For simplicity, we are just returning the registration DTO
            return registration;
        }
    }
}
