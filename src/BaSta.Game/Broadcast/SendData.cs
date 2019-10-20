namespace BaSta.Game.Broadcast
{
    public class SendData
    {
        public SendData()
        {
        }

        public SendData(string name, string value)
        {
            Name = name;
            Data = value;
        }

        public string Name { get; set; } = string.Empty;

        public string Data { get; set; } = string.Empty;

        public bool UseAlternateColor { get; set; }
    }
}