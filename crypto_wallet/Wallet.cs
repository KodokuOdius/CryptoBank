using System;

namespace WalletSpace {

    class Wallet {
        private Dictionary<string, int> data;
        public Wallet() {
            this.data = new Dictionary<string, int>{
                {"RUB", 1000}
            };
        }

        public Dictionary<string, int> Data { get { return this.data; } }

        public void AddMoney(string currency, int amount) {
            if (!this.data.ContainsKey(currency)) {
                this.data.Add(currency, 0);
            }
            this.data[currency] += amount;
        }

        public void TakeMoney(string currency, int amount) {
            this.data[currency] -= amount;
        }

        public Boolean CheckAmount(string currency, int amount) {
            if (this.data.ContainsKey(currency) == false) {
                return false;
            }
            return this.data[currency] - amount >= 0;
        }

        public void ShowData() {
            foreach (KeyValuePair<string, int> money in this.data) {
                Console.WriteLine($"{money.Value} - {money.Key}");
            }
        }

    }
}