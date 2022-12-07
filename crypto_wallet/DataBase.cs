using System;

using UserSpace;
using Crypto;


namespace DataBaseSpace {
    abstract class Table {

    }

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

        public Block(long timestamp, Transaction data) {
            this.timestamp = timestamp;
            this.data = data;
            this.prevHash = Blockchain.Source.LastBlock().currentHash;    
            this.currentHash = _sha256.hash(timestamp + this.prevHash + data?.ToString());
        }
    }

    class Blockchain {
        private static readonly Blockchain chain = new Blockchain();
        private List<Block> data;

        static Blockchain() {}
        private Blockchain() {
            this.data = new List<Block> {new Block(DateTimeOffset.Now.ToUnixTimeSeconds(), new Transaction("", "", 0, 0))};
        }
        public static Blockchain Source {
            get { return chain; }
        }

        public Block LastBlock() {
            return data.Last<Block>();
        }
    }

    class DataBase {
        

    }
}