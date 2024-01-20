namespace API.Helpers
{
    public class UserSpecParams  : PaginationParams
    {
 
        public string Sort { get; set; } = "lastActive";

        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();  
        }
        
    }


}