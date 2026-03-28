namespace TO_Lab1.Views
{
    public class ErrorView : View
    {
        private readonly string _message;

        public ErrorView(string message)
        {
            _message = message;
        }

        public string render()
        {
            return $"[ERROR] {_message}";
        }
    }
}
