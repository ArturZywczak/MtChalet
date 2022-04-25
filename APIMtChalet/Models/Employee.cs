using System;
using System.Collections.Generic;

#nullable disable

namespace APIMtChalet.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string Auth0Id { get; set; }
        public string ConnectionType { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
    }
}
