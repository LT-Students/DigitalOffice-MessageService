namespace LT.DigitalOffice.Broker.Requests
{
    public interface IAddImageRequest
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string Extension { get; set; }

        static object CreateObj(
            string name,
            string content,
            string extension)
        {
            return new
            {
                Name = name,
                Content = content,
                Extension = extension
            };
        }
    }
}
