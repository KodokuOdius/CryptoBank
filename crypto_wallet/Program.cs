using System;

using Crypto;
using DataBaseSpace;

class App {
    private static void Start() {
        Blockchain chain = Blockchain.blockchain;  
        chain.AddBlock(new Transaction("me", "you", 10, 0));
        chain.AddBlock(new Transaction("you", "you", 10, 0));
        chain.AddBlock(new Transaction("me", "you", 10, 0));
        chain.AddBlock(new Transaction("you", "you", 10, 0));

        chain.Show();
    }

    public static int Main(string[] args) {

        App.Start();
        
        return 0;
    }
}