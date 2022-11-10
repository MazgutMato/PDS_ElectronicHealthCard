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

        public Response<string> AddPerson(Person person)
        {
            Response<string> result = new Response<string>();
            try
            {
                _dbContext.People.Add(person);
                var res = _dbContext.SaveChanges();
                if (res == 1)
                {
                    result.message = "Successfully added!";
                }
                else
                {
                    result.message = "Added faild!";
                }
            }
            catch (Exception ex)
            {                
                result.message = ex.InnerException.Message;

            }
            return result;
        }

        public Response<string> DeletePerson(string id)
        {
            Response<string> result = new Response<string>();
            try
            {
                Person data = _dbContext.People.FirstOrDefault(u => u.PersonId == id);
                _dbContext.People.Remove(data);
                var res = _dbContext.SaveChanges();
                if (res == 1)
                {
                    result.message = "Successfully deleted!";
                }
                else
                {
                    result.message = "Added faild!";
                }

            }
            catch (Exception ex)
            {
                result.message = ex.Message;

            }
            return result;
        }

        public Response<List<Person>> GetPeople()
        {
            Response<List<Person>> result = new Response<List<Person>>();
            result.Data = _dbContext.People.ToList();
            var i = 0;
            return result;
        }

        public Response<Person> GetPersonById(string id)
        {
            Response<Person> result = new Response<Person>();
            result.Data = _dbContext.People.Find(id);
            return result;
        }

        public Response<string> UpdatePerson(Person person)
        {
            Response<string> result = new Response<string>();
            try
            {

                Person data = _dbContext.People.FirstOrDefault(d => d.PersonId == person.PersonId);

                data.PersonId = person.PersonId;
                data.FirstName = person.FirstName;
                data.LastName = person.LastName;

                var res = _dbContext.SaveChanges();
                if (res == 1)
                {
                    result.message = "Successfully updated!";
                }
                else
                {
                    result.message = "Updated failed!";
                }

            }
            catch (Exception ex)
            {
                result.message = ex.Message;
            }
            return result;
        }
    }
}
