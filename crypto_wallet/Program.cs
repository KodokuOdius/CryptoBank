using System;

using Crypto;
using DataBaseSpace;
using UserSpace;

class App {

    struct StateExit {
        public static int stateNumber = -1;
    }

    struct StateHello {
        public static int stateNumber = 0;
        public static string stateText() {
            return "Приветствие";
        }
        public static int State1 = StateEnter.stateNumber;
        public static int exit = StateExit.stateNumber;
    }

    struct StateEnter {
        public static int stateNumber = 1;
        public static string stateText() {
            return "Вход";
        }
        public static void stateAction() {
            for (int i = 0; i < 3; i++) {
                Console.WriteLine("Введите логин и пороль");
                string email = Console.ReadLine();
                string password = Console.ReadLine();

                User? user = UsersDB.DB.GetUser(email);
                if (user == null) {
                    Console.WriteLine();
                    continue;
                }

                if(user.CheckPassword(password) == false) {
                    Console.WriteLine();
                    continue;
                };
                
                currentUser = user;
                break;
            }
        }

        public static int State1 = StateHello.stateNumber;
        public static int State2 = StateProfile.stateNumber;
    }

    struct StateProfile {
        public static int stateNumber = 3;
    }


    private static Boolean isAppOpen = false;
    private static int userState = 0;
    private static User? currentUser = null;
    private static void Menu() {

    }

    private static void Start() {
        isAppOpen = true;
    }

    private static void Exit() {
        isAppOpen = false;
    }

    private static void Update() {
        if (userState == 0) {
            Console.WriteLine(
                "Приветствую тебя в системе \n" +
                "1 - Вход \n" +
                "2 - Регестрация \n" +
                "0 - Закрыть \n"
            );
        }
    }

    private static void UserProcess(int choise) {

    }

    public static int Main(string[] args) {
        int userChoice = 0;
        string? line;
        App.Start();

        while (isAppOpen) {
            Update();

            line = Console.ReadLine();
            if (line != null) {
                userChoice = int.Parse(line);
                UserProcess(userChoice); 
            }
        }
        
        return 0;
    }
}