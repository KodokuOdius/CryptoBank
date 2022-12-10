using Crypto;
using WalletSpace;

namespace UserSpace {
    class User {
        private string email;
        private string password;
        private Wallet wallet;

        public User(string email, string password) {
            this.email = email;
            this.password = _sha256.hash(password);
            this.wallet = new Wallet();
        }

        public string Email { get { return this.email; } }
        public Wallet UserWallet { get { return this.wallet; } }

        public Boolean CheckPassword(string password) {
            return _sha256.hash(password) == this.password;
        }

        public override string ToString() {
            return $"email:{this.email}";
        }

    }
}