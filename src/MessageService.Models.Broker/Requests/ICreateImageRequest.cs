namespace LT.DigitalOffice.Broker.Requests
{
    public interface ICreateImageRequest
    {
        public string Image { get; }

        static object CreateObj(string image)
        {
            return new
            {
                Image = image
            };
        }
    }
}
