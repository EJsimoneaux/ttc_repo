using api.Models;

namespace api.Interfaces
{
    public interface IGetEvent
    {
        Event GetEvent(int id); 
        Event NextEvent(); 
        Event GetByInvoice(int id); 
    }
}