using Azure;
using Azure.AI.TextAnalytics;
using Azure.AI.Translation.Text;
using DetectedLanguage = Azure.AI.TextAnalytics.DetectedLanguage;

namespace AILabb1;
public class AiService
{
    //Translate properties
    private string languageKey;
    private string languageEndpoint;
    private string region;
    private AzureKeyCredential credentials;
    private Uri endpoint;


    public void Init()
    {
        Console.Write("Enter key:> ");
        languageKey = Console.ReadLine();
        Console.Write("Enter region:> ");
        region = Console.ReadLine();
        Console.Write("Enter endpoint:> ");
        languageEndpoint = Console.ReadLine();

        credentials = new AzureKeyCredential(languageKey);
        endpoint = new Uri(languageEndpoint);

        Question();

    }

    //take user question, send to translate
    public void Question()
    {
        // take user input
        Console.WriteLine("Ask question:");
        string question = Console.ReadLine();

        // identify language
        var userLanguage = LanguageDetection(question);

        // translate to AI QnA
        var translatedQuestion = Translate(question, userLanguage).Result;

        //fetch answer
        var answer = QnA.AnswerQna(translatedQuestion);

        // answer the user, if user and qna language differs, translate
        var translatedAnswer = Translate(answer, "en", userLanguage).Result;
        Console.WriteLine(translatedAnswer);
    }

    //Detect language
    public string LanguageDetection(string question)
    {
        TextAnalyticsClient client = new TextAnalyticsClient(endpoint, credentials);

        DetectedLanguage detectedLanguage = client.DetectLanguage(question);
        //Console.WriteLine("Language:");
        //Console.WriteLine($"{detectedLanguage.Name},\tISO-6391: {detectedLanguage.Iso6391Name}\n");
        return detectedLanguage.Iso6391Name;
    }

    //translate
    public async Task<string> Translate(string question, string userLanguage, string targetLanguage = "en")
    {
        if (userLanguage == targetLanguage)
        {
            return question;
        }

        TextTranslationClient client = new(credentials, region);

        try
        {
            Response<IReadOnlyList<TranslatedTextItem>> response = await client.TranslateAsync(targetLanguage, question).ConfigureAwait(false);
            IReadOnlyList<TranslatedTextItem> translations = response.Value;
            TranslatedTextItem translation = translations.FirstOrDefault();
            return translation.Translations.FirstOrDefault().Text;
        }
        catch (RequestFailedException exception)
        {
            Console.WriteLine($"Error Code: {exception.ErrorCode}");
            Console.WriteLine($"Message: {exception.Message}");
            return question;
        }
    }
}
