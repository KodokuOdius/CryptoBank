using Crypto;
using WalletSpace;


namespace UserSpace {
    abstract class AUser {
        private string username;
        private string password;
        private Wallet wallet;

        public AUser(string username, string password) {
            this.username = username;
            this.password = _sha256.hash(password);
            this.wallet = new Wallet();
        }

        public Boolean CheckPassword(string password) {
            return _sha256.hash(password) == this.password;
        }

        static public String? ReadPassword() {
            Console.ForegroundColor = ConsoleColor.Yellow;
            String passwd = String.Empty;
            ConsoleKey key;
            do {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && passwd.Length > 0) {
                    Console.Write("\b \b");
                    passwd = passwd[0..^1];
                } else if (!char.IsControl(keyInfo.KeyChar)) {
                    Console.Write("*");
                    passwd += keyInfo.KeyChar;
                }
            }
            while (key != ConsoleKey.Enter);

            Console.ResetColor();
            return passwd;
        }
    }

    class User : AUser {
        private string email;
        private string password;
        private Wallet wallet;

        public User(string email, string password) : base(email, password) {
            this.email = email;
            this.password = _sha256.hash(password);
            this.wallet = new Wallet();
        }

        public string Email { get { return this.email; } }
        public Wallet UserWallet { get { return this.wallet; } }

        public override string ToString() {
            return $"email:{this.email}";
        }
    }
}