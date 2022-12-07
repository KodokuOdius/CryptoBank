using Crypto;

namespace UserSpace {
    class User {
        private string email;
        private string password;

        public User(string email, string password) {
            this.email = email;
            this.password = _sha256.hash(password);
        }

        public Boolean CheckPassword(string password) {
            return _sha256.hash(password) == this.password;
        }

        public override string ToString() {
            return $"email:{this.email}";
        }
    }
}