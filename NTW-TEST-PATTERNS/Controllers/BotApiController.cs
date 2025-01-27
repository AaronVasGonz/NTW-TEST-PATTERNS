using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Schema;

namespace NTW_TEST_PATTERNS.Controllers;


[Route("api/[controller]")]
[ApiController]
public class BotApiController(IBot bot) : Controller
{
    private readonly IBot _bot = bot;

    [HttpPost]
    public async Task<IActionResult> AskQuesiton([FromBody] string userQuestion)
    {
        if (string.IsNullOrWhiteSpace(userQuestion))
        {
            throw new ArgumentException("User question cannot be null or empty.");
        }

        var activity = new Activity
        {
            Type = ActivityTypes.Message,
            Text = userQuestion
        };

        var adapter = new TestAdapter();
        var turnContext = new TurnContext(adapter, activity);

        var cancellationToken = new CancellationToken();
        await _bot.OnTurnAsync(turnContext, cancellationToken);

        var botResponses = adapter.ActiveQueue
            .Where(a => a.Type == ActivityTypes.Message)
            .Select(a => ((IMessageActivity)a).Text)
            .ToList();

        return Ok(botResponses);
    }
}
