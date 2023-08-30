using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;

public class TelegramNotifier : ITelegramNotifier
{
    private readonly HttpClient _httpClient;
    private readonly string _botToken;
    private readonly string _chatId;

    public TelegramNotifier(HttpClient httpClient, IOptions<TelegramSettings> telegramSettings)
    {
        _httpClient = httpClient;
        _botToken = telegramSettings.Value.BotID;
        _chatId = telegramSettings.Value.ChatID;
    }

    public void Notify(string message)
    {
        SendMessageAsync(message).Wait();
    }

    private async Task SendMessageAsync(string message)
    {
        string url = $"https://api.telegram.org/bot{_botToken}/sendMessage";
        var values = new Dictionary<string, string>
    {
        { "chat_id", _chatId },
        { "text", message },
        { "parse_mode", "Markdown" }  // Přidáno pro podporu Markdown formátování
    };

        var content = new FormUrlEncodedContent(values);
        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            // Zde můžete logovat nebo zpracovat chybový stav
        }
    }
}