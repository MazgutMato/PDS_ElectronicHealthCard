using EHealthCardApp.Models;

namespace EHealthCardApp.Repository
{
    public interface IRepository
    {
        Response<List<Person>> GetPeople();
        Response<Person> GetPersonById(string id);
        Response<string> DeletePerson(string id);
        Response<string> UpdatePerson(Person person);
        Response<string> AddPerson(Person person);
        Response<string> GenerateData(int number);
    }
}
