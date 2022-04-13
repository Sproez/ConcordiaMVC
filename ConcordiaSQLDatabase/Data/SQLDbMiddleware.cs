namespace ConcordiaSqlDatabase.Data;

using Microsoft.EntityFrameworkCore;
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

    public async Task UpdateData((DatabaseImage created, DatabaseImage updated, DatabaseImage deleted) tuple)
    {
        //Card lists
        _context.CardLists.RemoveRange(tuple.deleted.CardLists);
        _context.CardLists.UpdateRange(tuple.updated.CardLists);
        await _context.CardLists.AddRangeAsync(tuple.created.CardLists);

        //People
        await _context.People.AddRangeAsync(tuple.created.People);
        _context.People.UpdateRange(tuple.updated.People);
        _context.People.RemoveRange(tuple.deleted.People);

        //Cards
        await _context.Cards.AddRangeAsync(tuple.created.Cards);
        _context.Cards.UpdateRange(tuple.updated.Cards);
        _context.Cards.RemoveRange(tuple.deleted.Cards);

        //Assignments
        await _context.Assignments.AddRangeAsync(tuple.created.Assignments);
        _context.Assignments.UpdateRange(tuple.updated.Assignments);
        _context.Assignments.RemoveRange(tuple.deleted.Assignments);

        //Comments
        await _context.Comments.AddRangeAsync(tuple.created.Comments);
        _context.Comments.UpdateRange(tuple.updated.Comments);
        _context.Comments.RemoveRange(tuple.deleted.Comments);

        await _context.SaveChangesAsync();
    }

    public async Task TEST_OverwriteAll(DatabaseImage img)
    {
        //Remove old data
        _context.Assignments.RemoveRange(_context.Assignments);
        _context.Comments.RemoveRange(_context.Comments);
        _context.Cards.RemoveRange(_context.Cards);
        _context.People.RemoveRange(_context.People);
        _context.CardLists.RemoveRange(_context.CardLists);
        await _context.SaveChangesAsync();
        //Replace with new data from API
        await _context.CardLists.AddRangeAsync(img.CardLists);
        await _context.People.AddRangeAsync(img.People);
        await _context.Cards.AddRangeAsync(img.Cards);
        await _context.Comments.AddRangeAsync(img.Comments);
        await _context.Assignments.AddRangeAsync(img.Assignments);
        await _context.SaveChangesAsync();
    }

}