using Newtonsoft.Json;
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

        ///<summary> Serialize employee data to Json string </summary>
        ///<returns> Json data string </returns>
        public string Serialize() {
            string jsonString = JsonConvert.SerializeObject(this, new JsonSerializerSettings());
            return jsonString;
        }

        ///<summary> Deserialize Json string to Employee </summary>
        ///<param name = "e"> Json string </param>
        ///<returns> Employee object </returns>
        public Employee Deserialize(string e) {
            Employee temp = JsonConvert.DeserializeObject<Employee>(e);
            return temp;
        }

        public Employee() {

        }

        ///<summary> Create new Employee object using json string </summary>
        ///<param name="e"> Json string </param>
        public Employee(string e) {
            Employee temp = Deserialize(e);

            this.Auth0ID = temp.Auth0ID;
            this.EmployeeId = temp.EmployeeId;
            this.ConnectionType = temp.ConnectionType;
            this.Name = temp.Name;
            this.Surname = temp.Surname;
            this.Phone = temp.Phone;
            this.Email = temp.Email;
            this.Role = temp.Role;
        }

    }


}
