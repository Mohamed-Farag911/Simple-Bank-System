
    // The User class remains as the data model, stored only in the application's memory
    public class User
    {
        private string id;
        private string username;
        private string password;
        private double balance;

        public User(string id, string username, string password, double balance)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.balance = balance;
        }

        public string GetId() => id;
        public string GetUsername() => username;
        public string GetPassword() => password;
        public double GetBalance() => balance;

        public void SetBalance(double balance) => this.balance = balance;
    }
