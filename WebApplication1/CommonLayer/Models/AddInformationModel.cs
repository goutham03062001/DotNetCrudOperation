namespace WebApplication1.CommonLayer.Models
{
    public class AddInformationModel
    {
        public string Name { get; set; }
        public string EmailId { get; set; }
        public string phoneNumber { get; set; }
        public string gender { get; set; }
        public string city { get; set; }
        public string country { get; set; }

        public void setName(string name)
        {
            this.Name = name;
        }
        public void setEmail(string email) { 
            this.EmailId = email;
        }
        public void setPhoneNumber(string phoneNumber)
        {
            this.phoneNumber = phoneNumber;
        }
        public void setGender(string gender) { this.gender = gender; }  
        public void setCity(string city) { this.city = city; }
        public void setCountry(string country) { this.country = country; }


    }


    public class EmployeeInfo
    {
        public string Name { get; set; }
        public string empId { get; set; }
        public string phoneNumber { get; set; }
        public string department { get; set; }
        public string salary { get; set; }
    }

        public class AddInformationResponse {
    public bool isSuccess {  get; set; }    
        public string message { get; set; }
        public List<AddInformationModel> data { get; set; }
        public List<EmployeeInfo> empData { get; set; }
    }



}
