using UserSpace;
using States;

class App {

    private static Boolean isAppOpen = false;
    private static int currentState = 0;
    private static User? currentUser = null;

    delegate void setCurrentUserDelegate(User user);
    static void setCurrentUser (User user) {
        currentUser = user;
    }
    delegate User? getCurrentUserDelegate();
    static User? getCurrentUser() {
        return currentUser;
    }

    private static void Start() {
        Console.Clear();
        isAppOpen = true;
    }

    private static void Exit() {
        isAppOpen = false;
        Console.Clear();
    }


    private static void Update() {
        System.Threading.Thread.Sleep(200);
        Console.Clear();
        Console.ResetColor();

        if (currentState == 0) {
            currentState = HelloState.Action();
        } else if (currentState == 1) {
            currentState = EnterState.Action(new setCurrentUserDelegate(setCurrentUser));
        } else if (currentState == 2) {
            currentState = RegistrationState.Action(new setCurrentUserDelegate(setCurrentUser));
        } else if (currentState == 3) {
            currentState = ProfileState.Action(new getCurrentUserDelegate(getCurrentUser));
        } else if (currentState == 4) {
            currentState = CreateTransactionState.Action(new getCurrentUserDelegate(getCurrentUser));
        } else {
            Exit();
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