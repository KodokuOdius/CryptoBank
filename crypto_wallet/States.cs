using UserSpace;
using DataBaseSpace;


namespace States {
    abstract class State {
        static public int Number;
        static public int Action(Delegate? method = null) {
            return Number;
        }
    }

    class HelloState : State {
        static public new int Number = 0;
        static public new int Action(Delegate? method = null) {
            Console.WriteLine(
                "Приветствию тебя в криптокошельке! \n" +
                "Выбери что будешь делать: \n" +
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
                Console.WriteLine("Нет такого вариата");
            }
            return Number;
        }
    }

    class EnterState : State {
        static public new int Number = 1;
        static public new int Action(Delegate? setUser = null) {
            for (int i = 0; i < 3; i++) {    
                Console.WriteLine(
                    $"Выполните вход в систему ({3 - i} попытки)"
                );
                Console.Write("Email: ");
                string? email = Console.ReadLine();

                Console.Write("Password: ");
                string? password = User.ReadPassword();

                User? user = UsersDB.DB.GetUser(email);

                if (user == null) {
                    continue;
                };

                if (password == null || !user.CheckPassword(password)) {
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
        static public new int Number = 2;
        static public new int Action(Delegate? setUser = null) {
            // TODO: покрасить тект
            Console.WriteLine("Зарегестрируйте пользователя");   
            Console.Write("Email: ");
            string? email = Console.ReadLine();

            if (UsersDB.DB.GetUser(email) != null) {
                Console.WriteLine("Эта почта уже используется");
                return Number;
            }

            // TODO: Скрыть пароль
            Console.Write("Password: ");
            string? password = User.ReadPassword();

            if (email != null && password != null && setUser != null) {
                User new_user = UsersDB.DB.CreateUser(email, password);
                setUser.DynamicInvoke(new_user);

                return ProfileState.Number;
            } else {
                Console.WriteLine("Вы пропустили какое-то поле");
            }
            return Number;
        }
    }


    class ProfileState : State {
        static public new int Number = 3;
        static public new int Action(Delegate? getUser = null) {
            if (getUser == null) {
                return Number;
            }

            User? currentUser = (User?)getUser.DynamicInvoke();
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
                    return CreateTransactionState.Number;
                } else if (choise == "0") {
                    return HelloState.Number;
                } else {
                    Console.WriteLine("Нет такого варианта");
                }
            }
            return Number;
        }
    }

    class CreateTransactionState : State {
        static public new int Number = 4;
        static public new int Action(Delegate? getUser = null) {
            if (getUser == null) {
                return ProfileState.Number;
            }

            User? currentUser = (User?)getUser.DynamicInvoke();
            if (currentUser == null) {
                return ProfileState.Number;
            }

            Console.WriteLine("Введите почту пользователя которому хотите совершить перевод: \nEmail:");
            string? email = Console.ReadLine();

            User? search_user = UsersDB.DB.GetUser(email);

            if (search_user == null) {
                Console.WriteLine("Пользователь не найден");
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
                        Console.WriteLine("Не хватает средств");
                        return Number;
                    }
                    Blockchain.blockchain.AddBlock(currentUser.Email, search_user.Email, amount, currency);
                } else {
                    Console.WriteLine("Ошибка ввода");
                    return Number;
                }
            }
            return ProfileState.Number;
        }
    }

}