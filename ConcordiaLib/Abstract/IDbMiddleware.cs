namespace ConcordiaLib.Abstract;

using Domain;

public interface IDbMiddleware
{
    Task<DatabaseImage> GetDatabaseData();

    Task<List<Card>> GetAllCards();

    Task<List<Person>> GetAllPeople();

    Task<Person?> GetPerson(string id);

    Task<List<Card>> GetScientistAssignments(string scientistId);

    Task UpdateData((DatabaseImage created, DatabaseImage updated, DatabaseImage deleted) tuple);
}

