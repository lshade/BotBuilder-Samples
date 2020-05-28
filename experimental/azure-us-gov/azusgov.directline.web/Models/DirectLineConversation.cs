namespace AzUsGov.DirectLine.Web.Models
{
    public class DirectLineConversation
    {
        public string ConversationId { get; set; }
        public string Token { get; set; }
        public int Expires_in { get; set; }
    }
}
