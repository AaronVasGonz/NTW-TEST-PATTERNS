using FAQBot.FAQBot.models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using NTW_TEST_PATTERNSV.FAQBot.FAQBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAQBot.Bot;

public class FAQBotService(IFAQService faqService) : IBot
{
    private readonly IFAQService _faqService = faqService;

    public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
    {
        if (turnContext.Activity.Type == ActivityTypes.Message)
        {
            var userMessage = turnContext.Activity.Text;
            var answer = _faqService.GetAnswer(userMessage);

            if (!string.IsNullOrWhiteSpace(answer))
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(answer), cancellationToken);

            }
            else
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("I'm sorry, I don't know the answer to that question."), cancellationToken);
            }
        }
    }
}
