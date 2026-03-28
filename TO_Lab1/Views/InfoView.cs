namespace TO_Lab1.Views
{
    public class InfoView : View
    {
        private readonly string _message;

        public InfoView(string message)
        {
            _message = message;
        }

        public string render()
        {
            return _message;
        }
    }
}
