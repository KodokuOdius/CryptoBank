using System;

using DataBaseSpace;
using UserSpace;

class App {

    private static Boolean isAppOpen = false;
    private static int currentState = 0;
    private static User? currentUser = null;

    private static void Start() {
        Console.Clear();
        isAppOpen = true;
    }

    private static void Exit() {
        isAppOpen = false;
        Console.Clear();
    }


    private static void Hello() {
        string? choise;
        Console.WriteLine(
            "Приветствию тебя в криптокошельке! \n" +
            "Выбери что будешь делать: \n" +
            "1 - Вход \n" +
            "2 - Зарегестрироваться \n" +
            "0 - Выйти \n"
        );
        choise = Console.ReadLine();
        if (choise == "1") {
            currentState = 1;
        } else if (choise == "2") {
            currentState = 2;
        } else if (choise == "0") {
            Exit();
        } else {
            Console.WriteLine("Нет такого вариата");
        }
    }


    private static void Enter() {
        for (int i = 0; i < 3; i++) {    
            Console.WriteLine(
                $"Выполните вход в систему ({3 - i} попытки)"
            );
            Console.Write("Email: ");
            string? email = Console.ReadLine();

            Console.Write("Password: ");
            string? password = Console.ReadLine();

            User? user = UsersDB.DB.GetUser(email);

            if (user == null) {
                continue;
            };

            if (password == null || !user.CheckPassword(password)) {
                continue;
            }

            currentUser = user;
            currentState = 3;
            return;
        }
        currentState = 0;
    }

    public static void Regestration() {
        Console.WriteLine("Зарегестрируйте пользователя");   
        Console.Write("Email: ");
        string? email = Console.ReadLine();

        if (UsersDB.DB.GetUser(email) != null) {
            Console.WriteLine("Эта почта уже используется");
            return;
        }

        Console.Write("Password: ");
        string? password = Console.ReadLine();

        if (email != null && password != null) {
            User new_user = UsersDB.DB.CreateUser(email, password);
            currentUser = new_user;
            currentState = 3;
        } else {
            Console.WriteLine("Вы пропустили какое-то поле");
        }
    }

    public static void Profile() {
        if (currentUser != null) {
            Console.WriteLine(
                "Ваш профиль: \n" +
                $"Email: {currentUser.Email} \n\n" +
                "Ваш кошелёк: \n"
            );
            currentUser.UserWallet.ShowData();

            Console.WriteLine(
                "\n1 - Сделать перевод \n" +
                "0 - Выйти из системы"
            );

            string? choise = Console.ReadLine();

            if (choise == "1") {
                currentState = 4;
            } else if (choise == "0") {
                currentState = 0;
            } else {
                Console.WriteLine("Нет такого варианта");
            }

        } else {
            currentState = 0;
        }
    }

    public static void CreateTransaction() {
        if (currentUser == null) {
            currentState = 3;
            return;
        }

        Console.WriteLine("Введите почту пользователя которому хотите совершить перевод: \nEmail:");
        string? email = Console.ReadLine();

        User? search_user = UsersDB.DB.GetUser(email);

        if (search_user == null) {
            Console.WriteLine("Пользователь не найден");
            return;
        } else {
            Console.WriteLine("Ваш кошелёк:");
            currentUser.UserWallet.ShowData();

            Console.Write("\nВведите сумму: ");
            string? amountS = Console.ReadLine();

            Console.Write("Введите валюту (как в кошельке): ");
            string? currency = Console.ReadLine();

            if (amountS != null && currency != null && int.TryParse(amountS, out int amount)) {
                bool result = currentUser.UserWallet.CheckAmount(currency, amount);
                if (result == false) {
                    Console.WriteLine("Не хватает средств");
                    return;
                }
                Blockchain.blockchain.AddBlock(currentUser.Email, search_user.Email, amount, currency);
            } else {
                Console.WriteLine("Ошибка ввода");
            }
        }
        currentState = 3;
    }

    private static void Update() {
        Console.Clear();

        if (currentState == 0) {
            Hello();
        } else if (currentState == 1) {
            Enter();   
        } else if (currentState == 2) {
            Regestration();
        } else if (currentState == 3) {
            Profile();
        } else if (currentState == 4) {
            CreateTransaction();
        }
    }

    public static int Main(string[] args) {
        App.Start();

        while (isAppOpen) {
            Update();
        }

        Console.WriteLine("Прощайте!");
        
        return 0;
    }
}