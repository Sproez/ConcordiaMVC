namespace ConcordiaLib.Abstract;

using Domain;
using Collections;

public interface IDbMiddleware
{
    Task<DatabaseImage> GetDatabaseData();

    Task<List<Card>> GetAllCards();

    Task<List<Person>> GetAllPeople();

    Task<Person?> GetPerson(string id);

    Task<List<Card>> GetScientistAssignments(string scientistId);

    Task UpdateData(MergingResults merge);
}

