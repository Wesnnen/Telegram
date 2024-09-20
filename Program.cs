using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;


class Program
{
    // Coloque seu token do bot aqui
    private static readonly string BotToken = "7506461388:AAHUVP1o3oX0Mv_FPO9Ju7L8tUDWgPz1-Cw";
    private static TelegramBotClient botClient;

    static void Main(string[] args)
    {
        // Inicializa o cliente do bot com o token
        botClient = new TelegramBotClient(BotToken);

        // Configurações para o recebimento de mensagens
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // Recebe todos os tipos de atualizações
        };

        // Inicia o recebimento de atualizações
        botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions);

        Console.WriteLine("Bot iniciado. Pressione qualquer tecla para parar o bot...");
        Console.ReadKey();

        // Para o recebimento de mensagens
        botClient.CloseAsync().Wait();
    }

    // Método para processar as atualizações (mensagens)
    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // Apenas processa mensagens de texto
        if (update.Type == UpdateType.Message && update.Message!.Text != null)
        {
            var message = update.Message;

            Console.WriteLine($"Recebi uma mensagem de {message.Chat.FirstName}: {message.Text}");

            // Verificar perguntas sobre o Rio de Janeiro
            string resposta = ProcessarPerguntaSobreRio(message.Text.ToLower());

            // Enviar a resposta para o usuário
            await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: resposta
            );
        }
    }

    // Método para processar perguntas relacionadas ao Rio de Janeiro
    private static string ProcessarPerguntaSobreRio(string pergunta)
    {
        if (pergunta.Contains("cristo redentor"))
        {
            return "O Cristo Redentor é uma estátua de Jesus Cristo localizada no topo do Corcovado, no Rio de Janeiro.";
        }
        else if (pergunta.Contains("ola")||
                pergunta.Contains("oi")||
                pergunta.Contains("bom dia")||
                pergunta.Contains("boa tarde")||
                pergunta.Contains("boa noite")||
                pergunta.Contains("saudações")||
                pergunta.Contains("e ai")||
                pergunta.Contains("olá"))
        {
            return "Olá! Bem-vindo ao nosso restaurante. Como posso ajudar você hoje?\n Digite Menu para mais opções";
        }
        else if (pergunta.Contains("menu")||
            pergunta.Contains("Menu")||
            pergunta.Contains("voltar")||
            pergunta.Contains("Voltar"))
        {
            return "Aqui está nosso menu:\n1. Filé Mignon\n2. Peixe Grelhado\n3. Risoto de Funghi\n4. Lasanha de Espinafre\n Precisa de recomendação?\n Digite Recomenda ou pedido";
        }
        else if (pergunta.Contains("recomenda") ||
            pergunta.Contains("Recomenda"))
        {
            return "Eu recomendo o Filé Mignon e o Risoto de Funghi!\n Deseja fazer o pedido? \n Digite Pedido ou Menu para retorna as opções";
        }
        else if (pergunta.Contains("pedido") ||
            pergunta.Contains("Pedido"))
        {
            return "Qual o seu pedido? Digite o número. ";
        }
        else if (pergunta.Contains("1") ||
            pergunta.Contains("2")||
            pergunta.Contains("3") ||
            pergunta.Contains("4"))
        {
            return "Obrigado pelo pedido ";
        }
        else { 
            return "Olá! Como posso ajudar você hoje? Digite 'menu' para ver as opções."; 
        }
    }
    // Método para lidar com erros
    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Erro na API do Telegram:\n{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}