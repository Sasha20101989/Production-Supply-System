namespace DAL.Models.Document
{
    public class CustomError
    {
        public string ErrorMessage { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }
    }
}
