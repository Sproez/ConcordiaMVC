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

    Task<(int, int)> GetScientistPerformanceReport(string scientistId, string completedListId);

    Task PostComment(Comment comment);

    Task<List<CardList>> GetAllCardLists();

    Task ChangeCardStatus(string id, string newStatus);

    Task UpdateData(MergingResults merge);
}

