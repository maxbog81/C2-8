using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIServiceEmp.Models;

namespace WebAPIServiceEmp.Controllers
{
    public class DataController : ApiController
    {
        private Data data = new Data();


        [Route("emplist")]
        public ObservableCollection<Employee> GetEmpList() => data.GetEmpList();

        [Route("emplist/{id}")]
        public Employee GetEmp(int id) => data.GetEmpById(id);

        [Route("addemp")]
        public HttpResponseMessage PostEmp([FromBody]Employee value)
        {
            if (data.AddEmp(value))
                return Request.CreateResponse(HttpStatusCode.Created);
            else return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [Route("deptlist")]
        public ObservableCollection<Department> GetDeptList() => data.GetDeptList();

        [Route("deptlist/{dept}")]
        public Department GetDept(string dept) => data.GetDept(dept);

        [Route("adddept")]
        public HttpResponseMessage PostDept([FromBody]Department value)
        {
            if (data.AddDept(value))
                return Request.CreateResponse(HttpStatusCode.Created);
            else return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

    }
}
