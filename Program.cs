using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using MovieInfoSearcher.Services;


namespace TelegramBotExperiments
{

    class Program
    {
        static IMovieSearcherService movieSearcherService = new MovieSeracherService();

        static ITelegramBotClient bot = new TelegramBotClient("6537585440:AAHDqmqQwZ9baHjMQBH1Sfb_zUtlALvibKw");
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                string lowerText = message.Text.ToLower();
                string command = lowerText.Split(' ')[0];

                if (CommandDictionary.Commands.ContainsKey(command))
                {
                    await CommandDictionary.Commands[command].Invoke(update);
                    return;
                }
                if (CommandDictionary.Commands.ContainsKey(lowerText))
                {
                    await CommandDictionary.Commands[lowerText].Invoke(update);
                    return;
                }

                await botClient.SendTextMessageAsync(message.Chat, "I don't understand you :(");
            }
        }

        public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        }


        static void Main(string[] args)
        {
            CommandDictionary.Initialize(bot, movieSearcherService);

            Console.WriteLine("Bot started " + bot.GetMeAsync().Result.FirstName);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { },
            };
            bot.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );
            Console.ReadLine();
        }

    }
}