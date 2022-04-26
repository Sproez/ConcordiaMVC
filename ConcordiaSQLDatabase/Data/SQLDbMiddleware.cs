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
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Person>> GetAllPeople()
    {
        return await _context.People
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Person?> GetPerson(string id)
    {
        return await _context.People
            .Where(p => p.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<List<Card>> GetScientistAssignments(string scientistId)
    {
        return await _context.Cards
            .Include(c => c.Assignees)
            .Where(c => c.Assignees.Any(a => a.PersonId == scientistId))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task PostComment(Comment comment)
    {
        var oldComments = await _context.Comments
            .Where(c => c.CardId == comment.CardId)
            .AsNoTracking()
            .ToListAsync();
        _context.Comments.RemoveRange(oldComments);

        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task ChangeCardStatus(string id, string newStatus)
    {
        var matchingCardLists = await _context.CardLists
            .Where(c => c.Name.Equals(newStatus))
            .AsNoTracking()
            .ToListAsync();
        switch (matchingCardLists.Count)
        {
            case 0:
                //TODO create new list
                throw new NotImplementedException();
            case 1:
                var card = await _context.Cards
                    .Where(c => c.Id == id)
                    .AsNoTracking()
                    .FirstAsync();
                var movedCard = card with { CardListId = matchingCardLists.First().Id };
                _context.Cards.Update(movedCard);
                break;
            default:
                throw new Exception();
        };
        await _context.SaveChangesAsync();
    }

    public async Task<DatabaseImage> GetDatabaseData()
    {
        var cards = await _context.Cards.AsNoTracking().ToListAsync();
        var people = await _context.People.AsNoTracking().ToListAsync();
        var comments = await _context.Comments.AsNoTracking().ToListAsync();
        var assignments = await _context.Assignments.AsNoTracking().ToListAsync();
        var cardLists = await _context.CardLists.AsNoTracking().ToListAsync();

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
        _context.CardLists.RemoveRange(cardLists.Deleted);
        _context.CardLists.UpdateRange(cardLists.Updated);
        await _context.CardLists.AddRangeAsync(cardLists.Created);

        //People
        _context.People.RemoveRange(people.Deleted);
        _context.People.UpdateRange(people.Updated);
        await _context.People.AddRangeAsync(people.Created);

        //Cards
        _context.Cards.RemoveRange(cards.Deleted);
        _context.Cards.UpdateRange(cards.Updated);
        await _context.Cards.AddRangeAsync(cards.Created);

        //Assignments
        _context.Assignments.RemoveRange(assignments.Deleted);
        _context.Assignments.UpdateRange(assignments.Updated);
        await _context.Assignments.AddRangeAsync(assignments.Created);

        //Comments
        _context.Comments.RemoveRange(comments.Deleted);
        _context.Comments.UpdateRange(comments.Updated);
        await _context.Comments.AddRangeAsync(comments.Created);

        await _context.SaveChangesAsync();
    }
}

public class MergingResult
{

}