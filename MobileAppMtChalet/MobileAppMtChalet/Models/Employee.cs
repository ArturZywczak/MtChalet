using System;
using System.Collections.Generic;
using System.Text;

namespace MobileAppMtChalet.Models {
    public class Employee {
        public int EmployeeId { get; set; }
        public string Auth0ID { get; set; }
        public string ConnectionType { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
    }
}
