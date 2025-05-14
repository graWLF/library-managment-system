using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Core.DTOs.Registration
{
    public class RegistrationDTO
    {
        public int Id { get; set; }                  // Maps to Registration.Id
        public string Username { get; set; }         // Maps to Registration.Username
        public string Name { get; set; }             // Maps to Registration.Name
        public string Lastname { get; set; }         // Maps to Registration.Lastname
        public string Email { get; set; }            // Maps to Registration.Email
        public string Password { get; set; }         // Maps to Registration.Password
        public int AuthLevel { get; set; }           // Maps to Registration.AuthLevel
    }
}
