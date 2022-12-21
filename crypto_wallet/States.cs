using UserSpace;
using DataBaseSpace;


namespace States {
    interface State {
        static public int Number;
        static public int Action(Delegate? method = null) {
            return Number;
        }
    }

    class HelloState : State {
        static public int Number = 0;
        static public int Action(Delegate? method = null) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(
                "Приветствую тебя в криптокошельке! \n" +
                "Этот кошелёк защитит твои данные лучшим шифрованием \n" +
                "Выбери, что будешь делать: \n"
            );

            Console.ResetColor();
            Console.WriteLine(
                "1 - Вход \n" +
                "2 - Зарегестрироваться \n" +
                "0 - Выйти \n"
            );

            string? choise = Console.ReadLine();
            if (choise == "1") {
                return EnterState.Number;
            } else if (choise == "2") {
                return RegistrationState.Number;
            } else if (choise == "0") {
                return -1;
            } else {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("ОШИБКА: Нет такого вариата");
            }
            return Number;
        }
    }

    class EnterState : State {
        static public int Number = 1;
        static public int Action(Delegate? setUser = null) {
            for (int i = 0; i < 3; i++) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    $"\nВыполните вход в систему ({3 - i} попытки)"
                );
                Console.ResetColor();

                Console.Write("Введите Логин: ");
                string? email = Console.ReadLine();

                Console.Write("Введите пароль: ");
                string? password = User.ReadPassword();

                User? user = UsersDB.DB.GetUser(email);

                if (user == null || password == null || !user.CheckPassword(password)) {
                    continue;
                }
                if (setUser != null) {
                    setUser.DynamicInvoke(user);
                    return ProfileState.Number;
                }
            }
            return HelloState.Number;
        }
    }

    class RegistrationState : State {
        static public int Number = 2;
        static public int Action(Delegate? setUser = null) {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Регистрация нового пользователя");   
            Console.ResetColor();

            Console.Write("Введите Логин: ");
            string? email = Console.ReadLine();

            if (UsersDB.DB.GetUser(email) != null) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Предупреждение: Этот логин уже используется");
                return Number;
            }

            Console.Write("Введите пароль: ");
            string? password = User.ReadPassword();

            if (email != null && password != null && setUser != null) {
                User new_user = UsersDB.DB.CreateUser(email, password);
                setUser.DynamicInvoke(new_user);

                return ProfileState.Number;
            } else {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ОШИБКА: Вы пропустили какое-то поле");
            }
            return Number;
        }
    }


    class ProfileState : State {
        static public int Number = 3;
        static public int Action(Delegate? getUser = null) {
            if (getUser == null) {
                return Number;
            }

            User? currentUser = (User?)getUser.DynamicInvoke();
            if (currentUser != null) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(
                    "Ваш профиль: \n" +
                    $"Логин: {currentUser.Email} \n\n"
                );
                Console.ResetColor();

                Console.WriteLine("Ваш кошелёк: \n");
                Console.ResetColor();

                currentUser.UserWallet.ShowData();

                Console.WriteLine(
                    "\n1 - Сделать перевод \n" +
                    "0 - Выйти из системы"
                );

                string? choise = Console.ReadLine();

                if (choise == "1") {
                    return CreateTransactionState.Number;
                } else if (choise == "0") {
                    return HelloState.Number;
                } else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ОШИБКА: Нет такого варианта");
                }
            }
            return Number;
        }
    }

    class CreateTransactionState : State {
        static public int Number = 4;
        static public int Action(Delegate? getUser = null) {
            if (getUser == null) {
                return ProfileState.Number;
            }

            User? currentUser = (User?)getUser.DynamicInvoke();
            if (currentUser == null) {
                return ProfileState.Number;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Введите логин пользователя, которому хотите совершить перевод: \nЛогин:");
            Console.ResetColor();

            string? email = Console.ReadLine();

            User? search_user = UsersDB.DB.GetUser(email);

            if (search_user == null) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ОШИБКА: Пользователь не найден");
                return Number;
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
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ОШИБКА: Не хватает средств");
                        return Number;
                    }
                    Blockchain.blockchain.AddBlock(currentUser.Email, search_user.Email, amount, currency);
                } else {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ОШИБКА: Ошибка ввода");
                    return Number;
                }
            }
            return ProfileState.Number;
        }
    }

}