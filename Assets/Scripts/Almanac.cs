using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Almanac : MonoBehaviour
{

    private static Almanac instance;
    public static Almanac Instance { get { return instance; } }

    private bool activeOnPlant = true;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(int ID)
    {

    }

    public void ShowPlants(bool plant)
    {
        if (activeOnPlant == plant) return;
        activeOnPlant = plant;
    }

    private string[] plantDescriptions = new string[]
    {
        "Shoots a single pea forward",
        "Produces sun regularly",
        "Blows up zombies in an area",
        "Walls off zombies with its high HP",
        "Blows up zombies when stepped on, but takes time to arm itself",
        "Shoots chilling peas that slow zombies",
        "Eats a whole zombie, but takes time to chew",
        "Shoots two peas at a time",
        "Free attacker that shoots at a short range",
        "Produces sun slowly, then faster once it grows after time",
        "Shoots fumes that hit all zombies in range",
        "Destroys a grave",
        "Hypotizes the zombie that eats it, making it fight for you",
        "Shoots at a long range, but hides when zombies are nearby",
        "Freezes all zombies for some time",
        "Blows up zombies in a large area, but leaves a crater in its tile",
        "Allows non-aquatic plants be planted on top",
        "Squashes zombies in front or behind",
        "Shoots peas in 3 lanes",
        "Pulls the first zombie it meets underwater",
        "Burns all zombies in its lane and melts ice",
        "Attacks zombies that walk on it and pops tires",
        "Ignites peas that pass through, giving splash and double damage",
        "Walls off zombies and blocks jumps",
        "Free aquatic attacker that shoots at a short range",
        "Lights up nearby fog and heals surrounding plants",
        "Shoots spikes from any height that pierce 2 zombies",
        "Removes all fog for a while and blows back zombies in its lane",
        "Shoots a single pea forward and two peas backwards",
        "Shoots stars in 5 directions",
        "Protects plants in its tile",
        "Removes metal objects from zombies, but takes time to dispose them",
        "Lobs cabbages over slopes and shields",
        "Allows plants to be planted on top",
        "Lobs kernels and a butter every fourth shot that stuns zombies",
        "Wakes up a sleeping plant",
        "Diverts zombies to other lanes",
        "Blocks descending zombies and projectiles in an area",
        "Shrinks zombies in an area, reducing their remaining HP to a third",
        "Lobs melons that do heavy splash damage",
        "Shoots four peas at a time",
        "Produces two suns regularly",
        "Shoots heavy fumes in all surrounding tiles",
        "Shoots two homing spikes at a time to the furthest left zombie",
        "Lobs chilling melons that slow zombies",
        "Collects sun for you regularly",
        "Attacks zombies that walk on it twice at a time and pops many tires",
        "Tap to aim, then tap anywhere on the lawn to launch an cob that blows up zombies in an area",
    };

    private string[] plantNotes = new string[]
    {
        "I really like what Brutal Mode EX+ did with it where each shot buffs its attack speed a tiny bit, capping at 1.5x. Maybe I should do that...",
        "Do not plant these in the back",
        "cherry bomb",
        "wallnut",
        "I kept its ability to hit Digger zombies while they're underground",
        "snowpea",
        "I really like what Brutal Mode EX+ did with it where the time it takes to chew depends on the HP of the zombie. Maybe I should do that...",
        "I balanced everything around this",
        "I really want to nerf this but then it makes night roof even more difficult so it's rough",
        "I really don't want sun in non-multiples of 25 so it just produces sun less often instead",
        "In vanilla it pierces the roof slope, but not here",
        "It might be cool to make this 0 recharge with 50 sun",
        "Hopefully the price cut and Giga-Football zombies makes this finally not suck",
        "scaredyshroom",
        "Instead of applying chill it just freezes longer, like Iceberg Lettuce plant food",
        "I won't nerf it as hard as PvZ Expansion did",
        "lilypad",
        "Right after it detects a zombie and before it jumps, it should have infinite HP",
        "If planted in row 1 or 5, it shoots 2 peas in its lane to compensate. Why it wasn't normally like this I'll never know",
        "Currenlty zombies still jump / do all kinds of stuff before it dies. Hopefully nothing breaks because of it",
        "jalapeno",
        "Can't be placed in roof like in vanilla",
        "I know a lot of mods offer different tiers of fire peas, and maybe I'll do that if I feel like it",
        "I always debate on whether this should block Balloon zombies, but it doesn't yet",
        "Hopefully the huge recharge decrease from vanilla makes this finally not suck",
        "Hopefully the added ability gives this more use outside of Fog levels",
        "This thing was excruciating to code so y'all better use it",
        "With the new ability, maybe I should retexture this to Hurrikale",
        "I genuinely can't think of a single way to make this not useless without stat inflation. This thing might actually be hopeless please help",
        "I still can't find a good sprite for it that has the limbs pointed the way I want it to",
        "How did the PvZ devs manage to have the plant sprites layered in between the front and back parts of the pumpkin, I need to know",
        "This thing was also annoying to code yet I have a level banning it...",
        "See Peashooter",
        "Maybe I should let them be planted on grass and lily pads just for the memes",
        "I don't like RNG so that is no more",
        "I'd do what PvZ Expansion did where it overclocks plants too if I had even the tiniest desire to go through with coding that",
        "This allows Doomshroom-main-DPS strategies so it's automatically a good plant",
        "I might have this also kill jumps but I'm not sure how balanced that would be",
        "It's Shrinking Violet now. Never again with the coin stuff",
        "melonpult",
        "I wanted to standardize prices but I'm not sure if this is balanced",
        "twinsunflower",
        "Hopefully the new world 6 zombies make this not as broken",
        "With the ordering of plants in the original, I'm forced to introduce this plant when there's no water for the rest of the campaign. I hate everything",
        "Hopefully the price increase makes this not as broken",
        "This was the absolute most I could come up with to give this plant even an iota of relevance",
        "spikerock",
        "There's no way in hell I'm coding a 2-tile plant with the framework I've built up"
    };

}
