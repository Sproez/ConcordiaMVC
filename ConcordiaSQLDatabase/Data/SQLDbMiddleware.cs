namespace ConcordiaSqlDatabase.Data;

using Microsoft.EntityFrameworkCore;
using ConcordiaLib.Collections;
using ConcordiaLib.Domain;
using ConcordiaLib.Abstract;

public class SQLDbMiddleware : IDbMiddleware
{
    private readonly ConcordiaDbContext _context;

    public SQLDbMiddleware(ConcordiaDbContext context)
    {
        _context = context;
    }

    public async Task<List<Card>> GetAllCards()
    {
        return await _context.Cards
            .Include(c => c.CardList)
            .Include(c => c.LastComment)
            .Include(c => c.Assignees)
            .ToListAsync();
    }

    public async Task<List<Person>> GetAllPeople()
    {
        return await _context.People
            .ToListAsync();
    }

    public async Task<Person?> GetPerson(string id)
    {
        return await _context.People
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Card>> GetScientistAssignments(string scientistId)
    {
        return await _context.Cards
            .Include(c => c.Assignees)
            .Where(c => c.Assignees.Any(a => a.PersonId == scientistId))
            .ToListAsync();
    }

    public async Task<DatabaseImage> GetDatabaseData()
    {
        var cards = await _context.Cards.ToListAsync();
        var people = await _context.People.ToListAsync();
        var comments = await _context.Comments.ToListAsync();
        var assignments = await _context.Assignments.ToListAsync();
        var cardLists = await _context.CardLists.ToListAsync();

        return new DatabaseImage(cards, people, comments, assignments, cardLists);
    }

    public async Task UpdateData(MergingResults merge)
    {
        var cards = merge.Cards.Local;
        var assignments = merge.Assignments.Local;
        var comments = merge.Comments.Local;
        var cardLists = merge.CardLists.Local;
        var people = merge.People.Local;

        //Card lists
        await _context.CardLists.AddRangeAsync(cardLists.Created);
        _context.CardLists.UpdateRange(cardLists.Updated);
        _context.CardLists.RemoveRange(cardLists.Deleted);

        //People
        await _context.People.AddRangeAsync(people.Created);
        _context.People.UpdateRange(people.Updated);
        _context.People.RemoveRange(people.Deleted);

        //Cards
        await _context.Cards.AddRangeAsync(cards.Created);
        _context.Cards.UpdateRange(cards.Updated);
        _context.Cards.RemoveRange(cards.Deleted);

        //Assignments
        await _context.Assignments.AddRangeAsync(assignments.Created);
        _context.Assignments.UpdateRange(assignments.Updated);
        _context.Assignments.RemoveRange(assignments.Deleted);

        //Comments
        await _context.Comments.AddRangeAsync(comments.Created);
        _context.Comments.UpdateRange(comments.Updated);
        _context.Comments.RemoveRange(comments.Deleted);

        await _context.SaveChangesAsync();
    }
}

public class MergingResult
{
}