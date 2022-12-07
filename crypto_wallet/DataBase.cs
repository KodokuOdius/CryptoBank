using System;

using Crypto;
using UserSpace;


namespace DataBaseSpace {
    enum Currency { RUB, USD };

    class Transaction {
        private string from;
        private string to;
        private int amount;
        private Currency currency;

        public Transaction(string from, string to, int amount, Currency currency) {
            this.from = from;
            this.to = to;
            this.amount = amount;
            this.currency = currency;
        }

        public override string ToString() {
            return $"from:{this.from},to:{this.to},amount:{this.amount},currency:{this.currency}";
        }
    }

    class Block {
        private string currentHash;
        private string prevHash;
        private long timestamp;
        private Transaction data;

        public Block(long timestamp, Transaction data, string prevHash = "") {
            this.timestamp = timestamp;
            this.data = data;
            this.prevHash = prevHash;
            this.currentHash = _sha256.hash(timestamp + this.prevHash + data.ToString());
        }

        public override string ToString() {
            return $"timestamp:{this.timestamp},currentHash:{this.currentHash},prevHash:{this.prevHash},data:{this.data}";
        }

        public string CurrentHash { get { return this.currentHash; } }
        public string PrevHash { get { return this.prevHash; } }
    }

    class Blockchain {
        static Blockchain() {}
        private Blockchain() {
            this.data = new List<Block> {new Block(DateTimeOffset.Now.ToUnixTimeSeconds(), new Transaction("", "", 0, 0))};
        }

        private static readonly Blockchain chain = new Blockchain();
        private List<Block> data;

        public static Blockchain blockchain {
            get { 
                return chain;
            }
        }

        public Block LastBlock() {
            return this.data.Last<Block>();
        }

        public void AddBlock(Transaction data) {
            this.data.Add(new Block(DateTimeOffset.Now.ToUnixTimeSeconds(), data, Blockchain.blockchain.LastBlock().CurrentHash));
        }

        public Boolean isValid() {
            for (int i = this.data.Count - 1; i > 0; i--) {
                if (this.data[i].PrevHash == this.data[i-1].CurrentHash) { return false; }
            }
            return true;
        }

        public void Show(){
            for (int i = 0; i < this.data.Count; i++) {
                Console.WriteLine(this.data[i]);
            }
        }
    }

    class UsersDB {
        static UsersDB() { }
        private UsersDB() {
            this.users = new List<User>{};
        }
        private static readonly UsersDB db = new UsersDB();
        private List<User> users;

        public static UsersDB DB {
            get { return db; }
        }

        public User? GetUser(string email) {
            for (int i = 0; i < this.users.Count; i++) {
                if (this.users[i].Email == email) {
                    return this.users[i];
                }
            }
            return null;
        }

        public User? CreateUser(string email, string password) { 
            if (this.GetUser(email) == null) {
                User new_user = new User(email, password);
                this.users.Add(new_user);
                return new_user;
            }
            return null;
        }

        public Boolean DeleteUser(string email) {
            User? user = this.GetUser(email);
            if (user != null) {
                return this.users.Remove(user);
            }
            return false;
        }
    }
}