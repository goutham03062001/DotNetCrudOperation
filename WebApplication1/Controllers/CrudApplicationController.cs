using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Linq.Expressions;
using WebApplication1.CommonLayer.Models;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using WebApplication1.CommonLayer.DBContext;
namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudApplicationController : ControllerBase
    {
        public readonly ILogger<CrudApplicationController> _logger;
        // public CrudApplicationController() { }

        public static List<AddInformationModel> myList = new List<AddInformationModel>();
        //public readonly List<AddInformationModel> staticList = new List<AddInformationModel>();

        private string connectionString = "Server=(localdb)\\mssqllocaldb;Database=aspnet-53bc9b9d-9d6a-45d4-8429-2a2761773502;Trusted_Connection=True;MultipleActiveResultSets=true";
        private readonly CrudDbContext _dbContext;
       
         public CrudApplicationController(CrudDbContext dbContext, ILogger<CrudApplicationController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        //static apis
        [HttpGet("GetStaticInfo")]
        public IActionResult GetStaticInfo()
        {
            AddInformationModel model = new AddInformationModel();
            model.phoneNumber = "12323";
            return Ok(model);
        }

        [HttpPost("AddInformation")]
        public IActionResult AddInformation(AddInformationModel requestModel)
        {

            AddInformationResponse responseModel = new AddInformationResponse();


            try
            {

                if (requestModel == null)
                {
                    responseModel.isSuccess = false;
                    responseModel.message = "failed to create!";

                }
                else
                {
                    myList.Add(requestModel);
                    responseModel.isSuccess = true;
                    responseModel.message = "Created Successfully";
                    responseModel.data = myList;
                }

            }
            catch (Exception ex)
            {
                responseModel.isSuccess = false;
                responseModel.message = "Something went wrong, While creating a user";
                return Ok(ex.Message);
            }

            return Ok(responseModel);
        }

        [HttpGet("GetAllInfo")]

        public IActionResult GetInfo()
        {
            AddInformationResponse response = new AddInformationResponse();
            // List<AddInformationResponse> list = new List<AddInformationResponse>();
            response.isSuccess = true;
            response.message = "Fetched Successfully!";
            response.data = myList;
            // return Ok(response);
            return Ok(response);
        }

        [HttpDelete("DeleteRecord")]
        public IActionResult DeleteInfo(AddInformationModel request)
        {

            AddInformationResponse response = new AddInformationResponse();
            //check whether request.email contains list
            int index = myList.FindIndex(item => item.EmailId == request.EmailId);
            Console.WriteLine("Deleting index - " + index);
            if (index >= 0)
            {
                myList.RemoveAt(index);
                response.isSuccess = true;
                response.message = "Record deleted with emaild id successfully!";
                response.data.Add(myList[index]);
                return Ok(response);
            }
            else
            {
                response.isSuccess = false;
                response.message = "Record not found with provided email id ";
                return Ok(response);
            }


        }

        [HttpPut("UpdateRecord")]
        public IActionResult updateInfo(AddInformationModel request)
        {
            AddInformationResponse response = new AddInformationResponse();
            int index = myList.FindIndex(item => item.EmailId == request.EmailId);
            Console.WriteLine("Updating index - " + index);
            try
            {
                //find the list index


                if (index >= 0)
                {
                    myList[index] = request;
                    List<AddInformationModel> list = new List<AddInformationModel>();
                    list.Add(request);
                    response.isSuccess = true;
                    response.message = "Record with " + request.EmailId + " Updated Successfully!";

                    response.data = list;
                    return Ok(response);

                }
                else
                {
                    response.message = "Record not found!" + index;
                    response.isSuccess = false;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.message = "Current Index - " + index + "Error Occurred! - " + ex.Message;
                return Ok(response);
            }
        }
      

        [HttpPost("AddRequest")]
     
        public AddInformationResponse addStudent(AddInformationModel input)
        {
           AddInformationResponse response = new AddInformationResponse();
            try {
                if (_dbContext.Students.Any(s => s.EmailId == input.EmailId))
                {
                    response.isSuccess = false;
                    response.message = "User already existed with this email";
                    Console.WriteLine("User already existsed");
                    return response;
                }

                else
                {
                    _dbContext.Students.Add(input);
                    _dbContext.SaveChanges();
                    response.isSuccess = true;
                    response.message = "Student created successfully";
                    Console.WriteLine("Student created successfully");
                    return response;
                }
            }
            catch (Exception e) {
               Console.WriteLine(e.Message);
                return response;
            }
        }
        [HttpGet("GetAllStudents")]
        public AddInformationResponse GetAllStudents() {
            AddInformationResponse response = new AddInformationResponse();
            try {

                List<AddInformationModel> list = _dbContext.Students.ToList();
                response.data = list;
                return response;
               
            }
            catch(Exception e)
            {
                return response;
            }
        
        }

        [HttpDelete]
        public string DeleteStudent(AddInformationModel input) {
            AddInformationResponse response = new AddInformationResponse();
            try {
                if( _dbContext.Students.Any(student=>student.EmailId == input.EmailId))
                {
                    _dbContext.Remove(input);
                    _dbContext.SaveChanges();
                    response.message = "Student Deleted with email id - "+input.EmailId;
                    return response.message;

                }
                else
                {
                    response.message="Student not found with email id - "+input.EmailId;
                    return response.message;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return e.Message;
            }
        
        }

        [HttpPut]
        public IActionResult updateStudentWithEmaild(AddInformationModel input)
        {
            AddInformationResponse response = new AddInformationResponse();
            try {
                // AddInformationModel isStudentExisted = (AddInformationModel)_dbContext.Students.Find(input);

                var isStudentExisted = _dbContext.Students.FirstOrDefault(student => student.EmailId == input.EmailId);
                if (isStudentExisted==null)
                {
                    response.message = "No Student Found with provided details";

                    return Ok(response);
                }
                else
                {
                    isStudentExisted.Name = input.Name;
                    isStudentExisted.phoneNumber = input.phoneNumber;
                    isStudentExisted.gender = input.gender;
                    isStudentExisted.country = input.country;
                    _dbContext.SaveChanges();
                    response.message = "Student details updated";
                    response.isSuccess = true;
                  
                    return Ok(response);
                }
            }
            catch (Exception e) {
                return Ok(e.Message);   
            }
        }

        //for employee crud opertaions
        [HttpPost("/AddEmployee")]
        public AddInformationResponse addEmployee(EmployeeInfo input)
        {
            _logger.LogInformation("Triggered Add Employee API");

            AddInformationResponse response = new AddInformationResponse();
            try
            {
                if (input == null)
                {
                    response.message = "Please Provide inputs";
                    return response;
                }
               
                var isExisted =  _dbContext.Table.Any(c => c.empId == input.empId);
                if (isExisted)
                {
                     response.isSuccess = false;
                     response.message = "Already a user existed with the current emaployee";
                     return response;
                }
                else
                {
                   

                    _dbContext.Table.Add(input);
                    _dbContext.SaveChanges();
                    _logger.LogInformation("New employee added");
                    response.isSuccess = true;
                    response.message = "New Employee added";
                  
                    return response;
                }   
            }
            catch (Exception e) { 
                Console.WriteLine(e.Message);
                _logger.LogInformation("Error Occurred - ");
                response.message = e.Message;
                return response;
            }
        }
        [HttpGet("/GetAllEmployees")]
        public AddInformationResponse GetAllEmployees()
        {
            AddInformationResponse response = new AddInformationResponse();
            try {
                List<EmployeeInfo>list = new List<EmployeeInfo>(_dbContext.Table);
                response.empData = list;
                return response;
            }
            catch(Exception e) {  _logger.LogInformation(e.Message);
                response.message = e.Message;
                return response;
          }
        }

        [HttpGet("/UpdateEmployee")]    
        public AddInformationResponse UpdateInfo(EmployeeInfo info)
        {
            AddInformationResponse response = new AddInformationResponse();
            try {
                var isExisted = _dbContext.Table.FirstOrDefault(c=>c.empId == info.empId);
                if (isExisted != null) {
                    isExisted.Name = info.Name;
                    
                    isExisted.phoneNumber = info.phoneNumber;
                    isExisted.department = info.department;
                    isExisted.salary = info.salary;

                    _dbContext.SaveChanges();

                    response.message = "Employee details update with id"+info.empId+" ";
                    return response;
                }
                else
                {
                    response.isSuccess = false;
                    response.message = "No user found with provided id";
                    return response;
                }
            }
            catch(Exception e) {
                response.message = e.Message;
                response.isSuccess=false;
                return response;
            }
        }


        [HttpGet("/getEmployeeById")]
        public AddInformationResponse GetEmployeeById(string id)
        {
            AddInformationResponse response = new AddInformationResponse();
            response.empData = new List<EmployeeInfo>();
            try {
                //EmployeeInfo currEmployee = new EmployeeInfo();
               
                var isExisted = _dbContext.Table.Any(c => c.empId == id);
                var currEmployee = _dbContext.Table.Find(id);
                if (isExisted)
                {
                    response.isSuccess = true;

                    if (currEmployee!=null)
                    {
                        response.empData.Add(currEmployee);
                    }
                    return response;
                }
                else
                {
                    response.message = "Employee not found with curr id - " + id;
                    response.isSuccess = false;
                    return response;
                }

            }
            catch(Exception e)
            {
                response.message = e.Message;
                response.isSuccess=false;
                return response;
            }
        }
    }
       
}
