using ElectronicHealthCardApp.Models;

namespace ElectronicHealthCardApp.Repository
{
    public interface IRepository
    {
        Response<List<Person>> GetPeople();
    }
}
