using System.Collections.Generic;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : Controller
    {
        // GET: api/event
        [EnableCors("AnotherPolicy")]
        [HttpGet]
        public List<Event> GetAllEvents()
        {
            IGetAllEvents myObject = new ReadEventData(); 
            return myObject.GetAllEvents(); 
        }
        
        [EnableCors("AnotherPolicy")]
        [Route("getupcoming")]
        public List<Event> GetUpcoming()
        {
            IGetAllEvents myObject = new ReadEventData(); 
            return myObject.GetAllUpComingEvents(); 
        }
        [EnableCors("AnotherPolicy")]
        [Route("getnext")]
        public Event GetNext()
        {
            IGetEvent myObject = new ReadEventData(); 
            return myObject.NextEvent(); 
        }
        [EnableCors("AnotherPolicy")]
        [Route("getinvoice/{id}")]
        public Event GetInvoice(int id)
        {
            IGetEvent myObject = new ReadEventData(); 
            return myObject.GetByInvoice(id); 
        }

        // GET: api/event/5
        [EnableCors("AnotherPolicy")]
        [HttpGet("{id}", Name = "GetEvent")]
        public Event Get(int id)
        {
            IGetEvent myObject = new ReadEventData();
            return myObject.GetEvent(id); 
        }

        // POST: api/ttc
        [EnableCors("AnotherPolicy")]
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ttc/5
        [EnableCors("AnotherPolicy")]
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ttc/5
        [EnableCors("AnotherPolicy")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
                
    }
}