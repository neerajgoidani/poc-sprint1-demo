using IdentityFrame.Models;
using Mvc.Models;
using Mvc.Views.AccountMvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Controllers
{
    public class EmployeeMvcController : Controller
    {
        HttpClient client = new HttpClient();
        // GET: AccountMvc
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            using (var client = new HttpClient())
            {

                HttpResponseMessage responseMessage = client.GetAsync("http://localhost:50581/api/StudioApi/GetStudioList").Result;
                HttpResponseMessage responseMessages = client.GetAsync("http://localhost:50581/api/Employee/GetRoleList").Result;
                if (responseMessage.IsSuccessStatusCode == true  && responseMessages.IsSuccessStatusCode)
                {
                   
                    IEnumerable<StudioModel> StudioList = responseMessage.Content.ReadAsAsync<IEnumerable<StudioModel>>().Result;
                    ViewData["MyStudioList"] = new SelectList(StudioList, "StudioName", "StudioName");

                    IEnumerable<RoleViewModel> RoleList = responseMessages.Content.ReadAsAsync<IEnumerable<RoleViewModel>>().Result;
                    ViewData["MyRoleList"] = new SelectList(RoleList, "Name", "Name");
                 
                    return View();
                }
                else
                {
                    return View();// wrong 
                }
               
                

          
            }
               
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditEmployee(ApplicationUser employee)
        {

            try
            {

                HttpResponseMessage response = client.PutAsJsonAsync("http://localhost:50581/api/employee/" + employee.Id, employee).Result;
                bool xyz = response.IsSuccessStatusCode;
                if (xyz)
                    return RedirectToAction("GetEmployees");
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception exe)
            {
                return RedirectToAction("Login");
                //   return View();
            }

        }



        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel employee) // should be LoginViewModel
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.PostAsJsonAsync("http://localhost:50581/api/Employee/Login" , employee).Result;

                    bool xyz = response.IsSuccessStatusCode;
                    if (xyz)
                        return RedirectToAction("GetEmployees");
                    else
                    {
                        return RedirectToAction("Login");
                    }

                }

            }
            catch (Exception exe)
            {
                return RedirectToAction("Login");
                //   return View();
            }

        }










        [AllowAnonymous]
        [HttpGet]
        public ActionResult EditEmployee(string id)
        {
            ApplicationUser employee = null;
            try
            {

               // HttpResponseMessage response = client.GetAsync($"api/employee/employee/{id}").Result;
                HttpResponseMessage response = client.GetAsync($"http://localhost:50581/api/Employee/GetEmployee/{id}").Result;
                response.EnsureSuccessStatusCode();


                HttpResponseMessage responseMessage = client.GetAsync("http://localhost:50581/api/StudioApi/GetStudioList").Result;
                HttpResponseMessage responseMessages = client.GetAsync("http://localhost:50581/api/Employee/GetRoleList").Result;

                if (responseMessage.IsSuccessStatusCode == true && responseMessages.IsSuccessStatusCode)
                {

                    IEnumerable<StudioModel> StudioList = responseMessage.Content.ReadAsAsync<IEnumerable<StudioModel>>().Result;
                    ViewData["MyStudioList"] = new SelectList(StudioList, "StudioName", "StudioName");

                    IEnumerable<RoleViewModel> RoleList = responseMessages.Content.ReadAsAsync<IEnumerable<RoleViewModel>>().Result;
                    ViewData["MyRoleList"] = new SelectList(RoleList, "Name", "Name");

                }
                if (response.IsSuccessStatusCode)
                {



                    employee = response.Content.ReadAsAsync<ApplicationUser>().Result;
                    return View(employee);
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception exe)
            {
                return RedirectToAction("Login");
                // return View();
            }

        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {


            using (var client = new HttpClient())
            {
                HttpResponseMessage response = client.PostAsJsonAsync("http://localhost:50581/api/Employee/Register", model).Result;
                bool xyz = response.IsSuccessStatusCode;
                if (xyz == true)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    return RedirectToAction("Register");
                }
            } 

        }

        // public ActionResult EmployeeDetails(string id)
        //{


        //  ApplicationUser employee = null;
        //try
        // {


        //   HttpResponseMessage response = client.GetAsync("http://localhost:50581/api/Employee/GetEmployee/",id).Result;
        // if (response.IsSuccessStatusCode)
        // {

        //   employee = response.Content.ReadAsAsync<ApplicationUser>().Result;
        // return View(employee);
        // }
        // else
        //  {
        //    return RedirectToAction("Login");
        //  }


        //  }
        //  catch (Exception exe)
        //  {
        //    return RedirectToAction("Login");
        // }


        // }
        

        [HttpGet]
        [AllowAnonymous]       
        public ActionResult GetEmployees(RegisterViewModel model)
        {
           
            HttpResponseMessage responseMessage = client.GetAsync("http://localhost:50581/api/Employee/GetEmployees").Result;


            if (responseMessage.IsSuccessStatusCode == true)
            {
                List<ApplicationUser> employeeList = responseMessage.Content.ReadAsAsync<List<ApplicationUser>>().Result;

                return View(employeeList);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }


        
        [AllowAnonymous]
        public ActionResult DeleteEmployee(string id)
        {
            try
            {

                HttpResponseMessage response = client.DeleteAsync($"http://localhost:50581/api/Employee/DeleteEmployeeById/{id}").Result;
                bool xyz = response.IsSuccessStatusCode;
                if (xyz == true)
                {
                    return RedirectToAction("GetEmployees");
                }
                else
                {
                    return RedirectToAction("GetEmployees");
                }
            }
            catch (Exception exe)
            {
                return RedirectToAction("Login");
                //  return View();
            }

        }

        [AllowAnonymous]
        public ActionResult EmployeeDetails(string id)
        {


            ApplicationUser employee = null;
            try
            {


                HttpResponseMessage response = client.GetAsync($"http://localhost:50581/api/Employee/GetEmployee/{id}").Result;
                if (response.IsSuccessStatusCode)
                {

                    employee = response.Content.ReadAsAsync<ApplicationUser>().Result;
                    return View(employee);
                }
                else
                {
                    return RedirectToAction("Login");
                }


            }
            catch (Exception exe)
            {
                return RedirectToAction("Login");
            }


        }








    }
}