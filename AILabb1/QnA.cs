namespace AILabb1;
public static class QnA
{
    private static readonly Dictionary<string, string> QnADictionary = new()
    {
        {"support", "Our customer support are overworked and would prefer if you don't contact them."},
        {"hours", "We are open from 5:00am to 5:01am on wednesdays every third week."},
        {"location", "We won't tell you, we don't want people to show up and complain."},
        {"contact", "Contact our CEO at lefuden@gmail.com."},
        {"banana", "We have plenty banana for friendly monke."}
    };

    public static string AnswerQna(string userQuestion)
    {
        foreach (var entry in QnADictionary.Where(entry => userQuestion.Contains(entry.Key, StringComparison.OrdinalIgnoreCase)))
        {
            return entry.Value;
        }

        return "I'm sorry, I do not have an answer for that question.";
    }
}
