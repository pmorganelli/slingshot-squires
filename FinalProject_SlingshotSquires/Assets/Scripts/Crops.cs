public class Crop
{
    public string cropName;
    public float salePrice;

    public int totalGrowthStates;
    public int growthState;
    // Growth interval-- how many days between incrementing growthState
    public int growthInterval;
    // How many days has the plant been planted?
    public int age;
    public int totalHealth;
    public int currHealth;

    public Crop(string name, float price, int totalGrowthStates, int state, int totalHealth, int currHealth)
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
        float price = 5.0f;
        int growthStates = 5;
        int startingState = 0;
        int totalHealth = 100;
        int currHealth = 100;
        return new Crop("Pumpkin", price, growthStates, startingState, totalHealth, currHealth);
    }

    public static Crop Tomato()
    {
        float price = 5.0f;
        int growthStates = 1;
        int startingState = 0;
        int totalHealth = 100;
        int currHealth = 100;
        return new Crop("Tomato", price, growthStates, startingState, totalHealth, currHealth);
    }
    public static Crop Carrot()
    {
        float price = 5.0f;
        int growthStates = 5;
        int startingState = 0;
        int totalHealth = 100;
        int currHealth = 100;
        return new Crop("Carrot", price, growthStates, startingState, totalHealth, currHealth);
    }
    public static Crop Lettuce()
    {
        float price = 5.0f;
        int growthStates = 5;
        int startingState = 0;
        int totalHealth = 100;
        int currHealth = 100;
        return new Crop("Lettuce", price, growthStates, startingState, totalHealth, currHealth);
    }
    public static Crop Watermelon()
    {
        float price = 5.0f;
        int growthStates = 5;
        int startingState = 0;
        int totalHealth = 100;
        int currHealth = 100;
        return new Crop("Watermelon", price, growthStates, startingState, totalHealth, currHealth);
    }
};