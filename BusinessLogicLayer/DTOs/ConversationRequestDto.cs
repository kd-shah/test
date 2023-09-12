namespace RealTimeChatApi.BusinessLogicLayer.DTOs
{
    public class ConversationRequestDto
    {
        public string Id;
        public DateTime before;
        public int count = 20;
        public string sort = "desc";
    }
}
