using System;
using System.Collections.Generic;
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


        [Route("getlist")]
        public List<Employee> Get() => data.GetList();

        [Route("getlist/{id}")]
        public Employee GetEmp(int id) => data.GetEmpById(id);

        [Route("addemp")]
        public HttpResponseMessage Post([FromBody]Employee value)
        {
            if (data.AddEmp(value))
                return Request.CreateResponse(HttpStatusCode.Created);
            else return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

    }
}
