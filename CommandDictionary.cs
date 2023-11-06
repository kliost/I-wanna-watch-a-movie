using MovieInfoSearcher.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

public static class CommandDictionary
{
    public static Dictionary<string, Func<Update, Task>> Commands = new Dictionary<string, Func<Update, Task>>();

    public static void Initialize(ITelegramBotClient BotClient, IMovieSearcherService movieSearcherService)
    {
        Commands["/start"] = async (update) =>
        {
            var message = update.Message;
            string commands =
            ("/popular {count of positions} - there is top of popular movies right now \n"
            + "/playing {region} - what's plating in cinema in my region \n"
            );

            await BotClient.SendTextMessageAsync(message.Chat, "Hello. I can help you to choose what to watch! \n \n" + commands, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
        };



        Commands["/popular"] = async (update) =>
        {
            var message = update.Message;
            string[] words = message.Text.Split(' ');

            if (words.Length > 1)
            {
                if (int.TryParse(words[1], out int count))
                {
                    if (count < 5)
                    {
                        await BotClient.SendTextMessageAsync(message.Chat, movieSearcherService.GetPopular(count).Result, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);

                    }
                    else
                    {
                        await BotClient.SendTextMessageAsync(message.Chat, movieSearcherService.GetPopular(3).Result, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);

                    }
                }
                else
                {
                    await BotClient.SendTextMessageAsync(message.Chat, "Please provide a valid count after the command.");
                }
            }
            else
            {
                await BotClient.SendTextMessageAsync(message.Chat, "Please provide a count of movies after the command.");
            }
        };

        Commands["/playing"] = async (update) =>
        {
            var message = update.Message;
            string[] words = message.Text.Split(' ');

            if (words.Length > 1)
            {
                await BotClient.SendTextMessageAsync(message.Chat, movieSearcherService.GetPlayingNow(words[1]).Result, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown);
            }
            else
            {
                await BotClient.SendTextMessageAsync(message.Chat, "Please provide a region code after the command. For example Ukraine = ua, etc");
            }


        };

    }
}
