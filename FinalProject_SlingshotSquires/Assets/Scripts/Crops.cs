public class Crop
{
    public string cropName;
    public int salePrice;

    public int totalGrowthStates;
    public int growthState;
    // Growth interval-- how many days between incrementing growthState
    public int growthInterval;
    // How many days has the plant been planted?
    public int age;
    public int totalHealth;
    public int currHealth;

    public Crop(string name, int price, int totalGrowthStates, int state, int totalHealth, int currHealth)
    {
        this.cropName = name;
        this.salePrice = price;
        this.growthState = state;
        this.totalGrowthStates = totalGrowthStates;
        this.totalHealth = totalHealth;
        this.currHealth = currHealth;
    }

    /* ADD A CONSTRUCTOR FOR ANY NEW CROP HERE */
    public static Crop Pumpkin()
    {
        int price = 50;
        int growthStates = 4;
        int startingState = 0;
        int totalHealth = 100;
        int currHealth = 100;
        return new Crop("Pumpkin", price, growthStates, startingState, totalHealth, currHealth);
    }

    public static Crop Tomato()
    {
        int price = 15;
        int growthStates = 1;
        int startingState = 0;
        int totalHealth = 100;
        int currHealth = 100;
        return new Crop("Tomato", price, growthStates, startingState, totalHealth, currHealth);
    }
    public static Crop Carrot()
    {
        int price = 30;
        int growthStates = 2;
        int startingState = 0;
        int totalHealth = 100;
        int currHealth = 100;
        return new Crop("Carrot", price, growthStates, startingState, totalHealth, currHealth);
    }
    public static Crop Lettuce()
    {
        int price = 5;
        int growthStates = 5;
        int startingState = 0;
        int totalHealth = 100;
        int currHealth = 100;
        return new Crop("Lettuce", price, growthStates, startingState, totalHealth, currHealth);
    }
    public static Crop Watermelon()
    {
        int price = 80;
        int growthStates = 5;
        int startingState = 0;
        int totalHealth = 100;
        int currHealth = 100;
        return new Crop("Watermelon", price, growthStates, startingState, totalHealth, currHealth);
    }
};