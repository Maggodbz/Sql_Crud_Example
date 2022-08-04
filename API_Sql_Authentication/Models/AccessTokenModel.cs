namespace API_Sql_Authentication.Models
{
    public class AccessTokenModel
    {
        public string Token { get; set; }

        public AccessTokenModel(string token)
        {
            Token = token;
        }
    }
}