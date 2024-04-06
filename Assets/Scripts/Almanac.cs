using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Almanac : MonoBehaviour
{

    private static Almanac instance;
    public static Almanac Instance { get { return instance; } }

    private bool activeOnPlant = true;
    private GameObject g;

    public Image image;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI info;
    public GameObject plantButtons;
    public GameObject zombieButtons;

    public static int previousSceneIndex;

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
        int ID = 0;
        foreach (Transform t in plantButtons.transform)
        {
            t.GetComponent<AlmanacSelectSeed>().ID = ID;
            t.GetComponent<AlmanacSelectSeed>().afterStart();
            ID++;
        }
        ID = 0;
        foreach (Transform t in zombieButtons.transform)
        {
            if (ID == 11 || ID == 12) ID = 13; // Cone/buckethead ducky tube
            t.GetComponent<AlmanacSelectSeed>().ID = ID;
            t.GetComponent <AlmanacSelectSeed>().afterStart();
            ID++;
        }
        ShowPlants(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(int ID)
    {
        Destroy(g);
        if (activeOnPlant)
        {
            image.color = Color.white;
            image.sprite = PlantBuilder.Instance.allPlants[ID].GetComponent<SpriteRenderer>().sprite;
            Plant p = PlantBuilder.Instance.allPlants[ID].GetComponent<Plant>();
            nameText.text = p.name;
            info.text = "Cost: " + p.cost + "\nRecharge: " + p.recharge;
            if (p.effectiveDamage != "") info.text += "\nDamage: " + p.effectiveDamage.Replace("\\n", "\n"); ;
            if (p.aquatic) info.text += "\nCan only planted in water";
            else if (ID == 33) info.text += "\nCan only be planted in roof";
            info.text += "\n\n" + plantDescriptions[ID] + "\n\n" + plantNotes[ID];
        } 
        else
        {
            image.color = Color.clear;
            g = Instantiate(ZombieSpawner.Instance.allZombies[ID], image.transform, false);
            g.GetComponent<Zombie>().displayOnly = true;
            g.transform.localScale = new Vector3(g.transform.localScale.x * 50, g.transform.localScale.y * 50, 1);
            g.transform.localPosition -= new Vector3(0, image.GetComponent<RectTransform>().rect.height / 2, 0);
            Zombie z = ZombieSpawner.Instance.allZombies[ID].GetComponent<Zombie>();
            nameText.text = z.name;
            info.text = "HP: " + z.HP;
            if (z.armor != null) info.text += ", " + z.armor.GetComponent<Armor>().HP + " (armor)";
            if (z.shield != null) info.text += ", " + z.shield.GetComponent<Shield>().HP + " (shield)";
            if (ID == 25 || ID == 30) info.text += "\nSpeed: " + speed[5]; // Gargantuars
            else if (z.walkTime > 0) info.text += "\nSpeed: " + speed[z.walkTime];
            foreach (float s in z.alternateWalkTime)
            {
                info.text += ", then " + speed[s];
            }
            if (z.weakness != "") info.text += "\nWeakness: " + z.weakness.Replace("\\n", "\n"); ;
            if (z.aquatic) info.text += "\nOnly appears in water";
            else if (ID == 15) info.text += "\nOnly appears on ice";
            info.text += "\n\n" + zombieDescriptions[ID] + "\n\n" + zombieNotes[ID];
        }
    }

    public void ShowPlants(bool plant)
    {
        activeOnPlant = plant;
        plantButtons.SetActive(plant);
        zombieButtons.SetActive(!plant);
        Show(0);
    }

    public void Exit()
    {
        SceneManager.LoadScene(previousSceneIndex);
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
        "Shrinks zombies in an area, reducing their remaining HP to a third. If they've already been shrunk, eliminates them",
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
        "I will not buff its recharge and break the game like PvZ 2 did",
        "RIP to this if I ever decide to implement Torchlight Zombies",
        "I kept its ability to hit Digger zombies while they're underground",
        "My personal favorite plant for anyone who cares",
        "I really like what Brutal Mode EX+ did with it where the time it takes to chew depends on the HP of the zombie. Maybe I should do that...",
        "I balanced everything around this",
        "I really want to nerf this but then it makes night roof even more difficult so it's rough",
        "I really don't want sun in non-multiples of 25 so it just produces sun less often instead",
        "In vanilla it pierces the roof slope, but not here",
        "It might be cool to make this 0 recharge with 50 sun",
        "Hopefully the price cut and Giga-Football zombies makes this finally not suck",
        "I still don't know why they added this to PvZ 2",
        "Instead of applying chill it just freezes longer, like Iceberg Lettuce plant food",
        "I won't nerf it as hard as PvZ Expansion did",
        "Unlike vanilla, Potato mines can be planted here. Maybe they shouldn't?",
        "Right after it detects a zombie and before it jumps, it should have infinite HP",
        "If planted in row 1 or 5, it shoots 2 peas in its lane to compensate. Why it wasn't normally like this I'll never know",
        "Currently zombies still jump / do all kinds of stuff before it dies. Hopefully nothing breaks because of it",
        "I really like what Brutal Mode EX did where it attacks in a cross. Maybe I should do that...",
        "Can't be placed in roof like in vanilla",
        "I know a lot of mods offer different tiers of fire peas, and maybe I'll do that if I feel like it",
        "I always debate on whether this should block Balloon zombies, but it doesn't yet",
        "Hopefully the huge recharge decrease from vanilla makes this finally not suck",
        "Hopefully the added ability gives this more use outside of Fog levels",
        "This thing was excruciating to code so y'all better use it",
        "With the new ability, maybe I should retexture this to Hurrikale",
        "I genuinely can't think of a single way to make it not useless without stat inflation. This thing might actually be hopeless please help",
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
        "I've realized there's no intermediary lobbing plant between Cabbage and Melon, so if I ever decide to add more, maybe Pepper-pult would fill that role?",
        "I wanted to standardize prices but I'm not sure if this price is balanced",
        "Of all special plants from vanilla, this is the one I want to lower the recharge of the most. I don't know how game-breaking that would be though",
        "Hopefully the new world 6 zombies make this not as broken",
        "With the ordering of plants in the original, I'm forced to introduce this plant when there's no water for the rest of the campaign. I hate everything",
        "Hopefully the price increase makes this not as broken",
        "This was the absolute most I could come up with to give this plant even an iota of relevance",
        "On one hand I want to let this tank a lot of Gargantuar hits but on the other hand it would then be too good against them so it's rough",
        "There's no way in hell I'm coding a 2-tile plant with the framework I've built up"
    };

    private Dictionary<float, string> speed = new Dictionary<float, string>() {
        { 6, "Slow"},
        { 5, "Normal" },
        { 4, "Moderate" },
        { 3, "Fast" },
        { 2, "SPEED" },
    };

    private string[] zombieDescriptions = new string[]
    {
        "A regular zombie",
        "Signals a huge wave of zombies to approach",
        "A zombie whose cone grants more HP than normal",
        "Runs and leaps over the first encountered plant",
        "A zombie whose bucket grants much more HP than normal",
        "Holds a newspaper as a shield, but runs and eats faster when it's destroyed",
        "Holds a screen door as a shield that greatly protects it from straight projectiles",
        "A zombie whose helmet grants very high HP",
        "Summons four backup dancers around itself, endlessly replacing those that die",
        "A backup zombie",
        "A zombie well-equipped for water travel",
        "", "",
        "Hides underwater except when eating a plant",
        "Crushes plants it drives over and lays ice in its path",
        "Carries 4 Bobsled Zombies in its bobsled where there is ice, and then unloads them afterwards",
        "Rides a dolphin very quickly and leaps over the first encountered plant",
        "Explodes after some amount of time, killing all plants nearby",
        "Flies high above the ground, avoiding most plants",
        "Digs to the other side of the lawn, and then walks backwards away from the house",
        "Continuously jumps over plants",
        "A rare zombie that escapes quickly after appearing",
        "Descends onto a tile and steals a plant",
        "Holds a ladder as a shield, and places it over an obstacle for zombies to climb over",
        "Lobs basketballs towards the rightmost plant, then drives forward after running out of projectiles",
        "A huge zombie with monstrous HP that smashes plants it encounters and throws an Imp after losing 2/3 of its health",
        "A small zombie",
        "Destroys the first plant it encounters",
        "A zombie equipped with the highest levels of protection",
        "A zombie with twice as much HP as a regular Football Zombie",
        "A huge zombie with twice as much HP as a regular Gargantuar",
    };

    private string[] zombieNotes = new string[]
    {
        "Imagine being this guy",
        "What would happen if this was somehow spawned between flag waves? The universe may never know",
        "The quintissential damage threshold",
        "Blover and the roof setting singlehandedly made coding the jump a nightmare",
        "This thing makes you realize most plants in this game just doesn't do a lot of damage",
        "I really want to do the meme here, but I won't for the sake of balance",
        "I considered having the screen door reflect projectiles, but I'm not sure how ok that would be",
        "It alone makes Hypno-shroom not completely worthless even in vanilla",
        "Across all plants and zombies this is the only one I've ever just given up on. It still works on a surface level, but one Snow Pea and the whole team's desynchronized. Too bad though, I'm done",
        "I can't decide on if it be invincible while spawning since it makes sense but feels annoying to play again",
        "Technically nothing in the code prevents a normal Basic from appearing in the pool, but let's pretend otherwise",
        "", "",
        "I made the hurtbox really small and low, but that's really it. Hopefully it works",
        "It actually slows down over time in vanilla. I don't get why they made it like that and I didn't incorporate that here",
        "I don't know how long the ice trail needs to be for this to spawn in vanilla so there's no cutoff here unfortunately",
        "This is invincible until the dolphin starts swimming fast",
        "The way the song has to play in-game made me make an entire new AudioSource object just for this thing. I hate everything",
        "This is probably the single most impactful change from the entire game by letting catapults target it. I await to see where this goes",
        "Logically I don't think explosives should be able to target this, but balance wise it makes more sense",
        "I never liked this zombie in my opinion due to its redundancy, but oh well",
        "There's no shop here so I have no idea what to do when he dies",
        "From my experiences it's almost impossible to kill this in time, but maybe that's how it should be",
        "This thing was also annoying to code...",
        "In my opinion, having it shoot the rightmost plant and not the leftmost is a good thing because it opens a new strategy of whether to put Umbrella Leafs in the front or tank them with wall plants. At least it feels better than \"put Umbrella Leaf and win\"",
        "The imp throw becomes way better on roof slopes than in flat ground in a way that I don't really know how to balance",
        "Isn't it crazy how in all games this zombie is either equal to objectively stronger than a Basic? I don't feel like that should be",
        "Now squash zombie will feel less defining I guess",
        "I've actually never played PvZ Plus yet",
        "I don't know whether it's better for Chomper to be able to eat this or not, but it can't for now",
        "All things considered, I do hope Marigold ends up actually being useful outside of just dealing with this",
    };

}
