using ElectronicHealthCardApp.Models;

namespace ElectronicHealthCardApp.Repository
{
    public class Repository : IRepository
    {
        private readonly EHealthCardContext _dbContext;
        public Repository(EHealthCardContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Response<List<Person>> GetPeople()
        {
            Response<List<Person>> result = new Response<List<Person>>();
            result.Data = _dbContext.People.ToList();
            return result;
        }
    }
}
