using System;
using System.Collections.Generic;
using System.IO;

namespace ObjectGame // ITEMS 
{
    public class Item
    {
        public readonly ItemEnum Name; //nemu6u to menit 

        public Item(ItemEnum Name) // kdyz6 tvorim item pak potrebuju ho uloyit jako jmeno 
        {
            this.Name = Name;
        }

        //---------------- TODO Each class should have a name (use constructor)       
    }

    public enum PotionTypeEnum
    {
        MANA_POTION = 2,
        LIFE_POTION = 3
    }

    //----------------------- TODO create class PotionItem (see usage in Program class)
    //----------------------- you should inherit from Item

    public class PotionItem : Item
    {
        public PotionTypeEnum Type;

        public PotionItem(PotionTypeEnum potionType) : base(ItemEnum.POTION)
        {
            this.Type = potionType;
        }

    }

    /// <summary>
    /// There are no other places, where you can where or hold something
    /// </summary>
    public enum BodyEnum
    {
        HEAD,
        HAND1,
        HAND2,
        BODY
    }


    /// <summary>
    /// All posible names for item
    /// </summary>
    public enum ItemEnum
    {
        CAPKA,
        KLOBOUK,
        PRILBA,
        KOZENAZBROJ,
        PLATENAZBROJ,
        ROBA,
        MEC,
        STIT,
        HUL,
        LUK,
        RING,
        POTION
    }

    /// <summary>
    /// TODO create class Armor
    /// see usage in Program class, you must be compatible!
    /// </summary>
    public class Armor : Item
    {
        public BodyEnum BodyPart;
        public int AttackBoost;
        public int DefenseBoost;

        // TODO Implement a constructor
        // use the parent class constructor for the name setting
        public Armor(BodyEnum bodyPart, int attack, int defense, ItemEnum name) : base(name)
        {
            this.BodyPart = bodyPart;
            this.AttackBoost = attack;
            this.DefenseBoost = defense;
        }
    }


}
/*-----------------------------------------------------------------------------------*/

namespace ObjectGame // POSTAVY 
{
    /// <summary>
    /// This class represents a team. Main functionality is to choose the next attacker
    /// or target from its members.
    /// </summary>
    public class Team
    {
        public List<Unit> teamMembers { get; set; }
        private int lastPlayer = -1;

        public Team(List<Unit> t)
        {
            teamMembers = t;
        }

        public void Add(Unit u)
        {
            teamMembers.Add(u);
        }

        /// <summary>
        /// Chooses players (cycling through all members)
        /// </summary>
        /// <returns>next player in the team to play</returns>
        public Unit GetNextPlayer()
        {

            int teamCount = teamMembers.Count;
            int i = (lastPlayer + 1) % teamCount;
            Unit u;

            while (i != lastPlayer)
            {

                u = teamMembers[i];
                if (u.Life > 0)
                {
                    lastPlayer = i;
                    return u;
                }
                i = (i + 1) % teamCount;
            }

            u = teamMembers[lastPlayer];

            if (u.Life > 0) return u;
            return null;
        }

        /// <summary>
        /// Chooses player to be attacked
        /// </summary>
        /// <returns>player to be attacked</returns>
        public Unit GetNextTarget()
        {
            Unit firstArcher = null;
            Unit firstHealer = null;

            for (int i = 0; i < teamMembers.Count; i++)
            {
                Unit u = teamMembers[i];
                if (u.Life > 0)
                {
                    if (u is Warrior)
                    {
                        return u;
                    }
                    else if (u is Archer)
                    {
                        if (firstArcher == null) firstArcher = u;
                    }
                    else
                    {
                        if (firstHealer == null) firstHealer = u;
                    }
                }
            }

            if (firstArcher != null) return firstArcher;
            else return firstHealer;
        }
    }

    /// <summary>
    /// The most general class, which describe what all units should
    /// have in common
    /// </summary>
    public abstract class Unit
    {
        /********** PRIVATE PROPERTIES AND ATTRIBUTES **********/
        protected int mana;
        protected int life;
        protected int BaseAttack;
        protected int BaseDefense;

        /********** PUBLIC PROPERTIES AND ATTRIBUTES **********
        * These attributes are visible to other classes.This is what the class
        * provides to others, her functionality/interface. */

        //NON-STATIC PROPERTIES AND ATTRIBUTES
        //each instance of the class should has its own copy with potential
        //different values to other instances.
        public readonly List<Item> Bag = new List<Item>();
        public Armor[] Equipment = new Armor[Enum.GetNames(typeof(BodyEnum)).Length]; //pole dlouhe jako pocet casti
        public List<ItemEnum> AllowedItems;
        public Team team;
        public int lastSpecial;
        public int COOLDOWN;

        public abstract int Mana { get; set; }
        public abstract int Life { get; set; }

        /************ CONSTRUCTORS *************/
        public Unit(int life_max, int mana_max, List<ItemEnum> MyAllowedItems, Team team)
        {
            this.life = life_max;
            this.mana = mana_max;
            this.AllowedItems = MyAllowedItems;
            this.team = team;
        }

        public Unit(int life_max, int mana_max, List<ItemEnum> MyAllowedItems, List<Armor> armor, Team team) :
            this(life_max, mana_max, MyAllowedItems, team)
        {
            this.AddArmor(armor);
        }

        /************ PUBLIC METHODS *************/
        //These methods is the functionality provided by the class =
        //this is the purpose of the existence of this class
        //They are defined in the abstract class, because their funcitonality
        //could be same for all derived classes. If suitable, you can override
        //them with class-specific implementation in derived classes.

        /// <summary>
        /// Adding one piece of equipement
        /// </summary>
        public void AddArmor(Armor armor)
        {
            //------------------------------------------------TODO implement
            if (AllowedItems.Contains(armor.Name))
            {
                int kde = (int)armor.BodyPart;

                if (Equipment[kde] != null)
                {
                    Armor vymena = Equipment[kde];
                    Bag.Add(vymena);
                }
                Equipment[kde] = armor;
            }
            else
            {
                Bag.Add(armor);
            }

        }

        /// <summary>
        /// Put on all armor from the list
        /// </summary>
        /// <param name="armor">List of equipement</param>
        public void AddArmor(List<Armor> armor)
        {
            foreach (Armor arm in armor)
            {
                AddArmor(arm);
            }
        }

        /// <summary>
        /// Add and item into bag
        /// </summary>
        /// <param name="item"></param>
        public void AddToBag(Item item)
        {
            Bag.Add(item);
        }

        /// <summary>
        /// Add all items form list to bag
        /// </summary>
        /// <param name="items">list of items for adding</param>
        public void AddToBag(List<Item> items)
        {
            Bag.AddRange(items);
        }

        /// <summary>
        /// Using potion
        /// </summary>
        /// <param name="PotionType">Type of the potion</param>
        public void UsePotion(PotionTypeEnum PotionType)
        {
            //TODO this method will work while you have correctly implemented PotionItem class
            int remove = -1;
            for (int i = 0; i < Bag.Count; i++)
            {
                Item it = Bag[i];
                if (it is PotionItem && ((PotionItem)it).Type == PotionType)
                {
                    remove = i;
                    if (PotionType == PotionTypeEnum.LIFE_POTION) Life += (int)PotionType;
                    else Mana += (int)PotionType;
                    break;
                }
            }
            if (remove > -1) Bag.RemoveAt(remove);

        }

        /// <summary>
        /// Counts defense of the unit plus defense added by its armor.
        /// </summary>
        /// <returns>The defense value</returns>
        public virtual int CountDefense()
        {
            int def = BaseDefense;
            foreach (Armor arm in Equipment)
            {
                if (arm != null)
                {
                    def += arm.DefenseBoost;
                }
            }
            return def;


        }

        /// <summary>
        /// Counts base plus equipement attack power.
        /// </summary>
        /// <returns>Attack power</returns>
        public virtual int CountAttack()
        {
            int att = BaseAttack;
            foreach (Armor arm in Equipment)
            {
                if (arm != null)
                {
                    att += arm.AttackBoost;
                }
            }
            return att;
        }



        //Following two methods are abstract => They should be
        //implemented in all derived classes but no common/default
        //implemenetation makes sense.
        public abstract int BaseAction(int roundCount);
        public abstract int SpecialAction(int roundCount);
    }


    public class Warrior : Unit
    {
        private int defenseBonus = 0;
        private int attackBonus = 0;

        //STATIC PROPERTIES AND ATTRIBUTES
        //these things does not differ among instances, i.e. all archers has the
        //same list of allowed items (static), in contrast to current life count,
        //which will differ among different team members.
        private static int SPECIAL_COOLDOWN;
        private static int SPECIAL_COST;
        private static int mana_max;
        private static int life_max;
        private static List<ItemEnum> MyAllowedItems;

        public static int MANA_MAX => mana_max;
        public static int LIFE_MAX => life_max;

        public override int Mana { get => mana; set => mana = Math.Min(Math.Max(0, value), MANA_MAX); }
        public override int Life { get => life; set => life = Math.Min(Math.Max(0, value), LIFE_MAX); }

        /************* CONSTRUCTORS **************/

        /// <summary>
        /// Static constructors is called only once (when the first usage
        /// of the class occurs) and is used to set of static properties (max mana, allowed items etc...
        /// </summary>
        static Warrior()
        {
            ItemEnum[] allowedItems = new ItemEnum[] { ItemEnum.CAPKA, ItemEnum.PRILBA, ItemEnum.PLATENAZBROJ, ItemEnum.KOZENAZBROJ, ItemEnum.ROBA, ItemEnum.MEC, ItemEnum.HUL, ItemEnum.STIT };
            MyAllowedItems = new List<ItemEnum>();
            MyAllowedItems.AddRange(allowedItems);
            mana_max = 10;
            life_max = 10;
            SPECIAL_COOLDOWN = 3;
            SPECIAL_COST = 0;

        }

        // Public constructors create instances

        /// <summary>
        /// This constructor calls a constructor of parent class, which already
        /// has a functionality for creating a unit with armor list.
        /// </summary>
        /// <param name="armor"></param>
        public Warrior(List<Armor> armor, Team team) : base(LIFE_MAX, MANA_MAX, MyAllowedItems, armor, team)
        {
            base.lastSpecial = -(SPECIAL_COOLDOWN + 1);
            base.COOLDOWN = SPECIAL_COOLDOWN;
        }

        public Warrior(Team team) : base(LIFE_MAX, MANA_MAX, MyAllowedItems, team)
        {
            base.lastSpecial = -(SPECIAL_COOLDOWN + 1);
            base.COOLDOWN = SPECIAL_COOLDOWN;
        }

        public override int CountAttack()
        {
            return base.CountAttack() + attackBonus;
        }
        public override int CountDefense()
        {
            return base.CountDefense() + defenseBonus;
        }
        public override int BaseAction(int roundCount)
        {
            return CountAttack();

        }
        /*
        public override int SpecialAction(int roundCount)
        {
            
            if (lastSpecial + COOLDOWN >= roundCount)
            {
                return 0;
            }

            if (Equipment[(int)BodyEnum.HAND1] != null)
            {
                if (Equipment[(int)BodyEnum.HAND1].Name == ItemEnum.MEC)
                {
                    attackBonus += 1;
                }
                else if (Equipment[(int)BodyEnum.HAND1].Name == ItemEnum.STIT)
                {
                    defenseBonus += 1;
                }
            }
            else if (Equipment[(int)BodyEnum.HAND2] != null)
            {
                if (Equipment[(int)BodyEnum.HAND2].Name == ItemEnum.MEC)
                {
                    attackBonus += 1;
                }
                else if (Equipment[(int)BodyEnum.HAND2].Name == ItemEnum.STIT)
                {
                    defenseBonus += 1;
                }
            }
            lastSpecial = roundCount;
            return 0;
            


            //defenseBonus = defenseBonus + base.CountDefense();
            //attackBonus = attackBonus + base.CountAttack();



        }*/
        public override int SpecialAction(int roundCount)
        {
            if (lastSpecial + COOLDOWN >= roundCount)
            {
                return 0;
            }
            foreach (Armor armor in Equipment)
            {
                if (armor != null && armor.Name is ItemEnum.MEC)
                {
                    attackBonus++;
                }
                else if (armor != null && armor.Name is ItemEnum.STIT)
                {
                    defenseBonus++;
                }
            }
            lastSpecial = roundCount;
            return 0;
        }

        /*************** PUBLIC METHODS *****************/
        //TODO implement base and special action as the parent class demands.
        //TODO implement CountDefense and CountAttack correctly (override base method)

        /*************** PRIVATE METHODS ************
         * These methods serves for implemting the inner functionality of the
         * class and there is no need for them to be public. 
         * Others should know what you do, not how you do it. */

    }

    public class Archer : Unit
    {
        private static int mana_max;
        private static int life_max;
        private static int SPECIAL_COOLDOWN;
        private static int SPECIAL_COST;

        public static int MANA_MAX => mana_max;
        public static int LIFE_MAX => life_max;
        public override int Mana { get => mana; set => mana = Math.Min(Math.Max(0, value), MANA_MAX); }
        public override int Life { get => life; set => life = Math.Min(Math.Max(0, value), LIFE_MAX); }
        public static List<ItemEnum> MyAllowedItems;

        static Archer()
        {
            ItemEnum[] allowedItem = new ItemEnum[] { ItemEnum.CAPKA, ItemEnum.KOZENAZBROJ, ItemEnum.LUK };
            MyAllowedItems = new List<ItemEnum>();
            MyAllowedItems.AddRange(allowedItem);
            life_max = 10;
            mana_max = 10;
            SPECIAL_COOLDOWN = 4;
            SPECIAL_COST = 0;

        }

        public Archer(List<Armor> armor, Team team) : base(LIFE_MAX, MANA_MAX, MyAllowedItems, armor, team)
        {
            base.lastSpecial = -(SPECIAL_COOLDOWN + 1);
            base.COOLDOWN = SPECIAL_COOLDOWN;
        }

        public Archer(Team team) : base(LIFE_MAX, MANA_MAX, MyAllowedItems, team)
        {
            base.lastSpecial = -(SPECIAL_COOLDOWN + 1);
            base.COOLDOWN = SPECIAL_COOLDOWN;
        }
        
        public override int BaseAction(int roundCount)
        {
            return CountAttack();

        }
        public override int SpecialAction(int roundCount)
        {
            if (lastSpecial + COOLDOWN >= roundCount)
            {
                return 0;
            }
            lastSpecial = roundCount;
            return 3*CountAttack();

        }

    }

    public class Healer : Unit
    {
        private static int mana_max;
        private static int life_max;
        private static int SPECIAL_COOLDOWN;
        private static int SPECIAL_COST;

        public static int MANA_MAX => mana_max;
        public static int LIFE_MAX => life_max;
        public override int Mana { get => mana; set => mana = Math.Min(Math.Max(0, value), MANA_MAX); }
        public override int Life { get => life; set => life = Math.Min(Math.Max(0, value), LIFE_MAX); }
        public static List<ItemEnum> MyAllowedItems;

        static Healer()
        {
            life_max = 15;
            mana_max = 10;
            SPECIAL_COST = 0;
            SPECIAL_COOLDOWN = 5;
            ItemEnum[] allowedItems = new ItemEnum[] { ItemEnum.HUL, ItemEnum.ROBA, ItemEnum.KLOBOUK };
            MyAllowedItems = new List<ItemEnum>();
            MyAllowedItems.AddRange(allowedItems);
        }

        public Healer(Team team) : base(LIFE_MAX, MANA_MAX, MyAllowedItems, team)
        {
            base.lastSpecial = -(SPECIAL_COOLDOWN + 1);
            base.COOLDOWN = SPECIAL_COOLDOWN;
        }

        public Healer(List<Armor> armor, Team team) : base(LIFE_MAX, MANA_MAX, MyAllowedItems, armor, team)
        {
            base.lastSpecial = -(SPECIAL_COOLDOWN + 1);
            base.COOLDOWN = SPECIAL_COOLDOWN;
        }

        
        public override int BaseAction(int roundCount)
        {
            Unit player = team.GetNextTarget();
            player.Life += CountAttack();
            return 0;

        }
        public override int SpecialAction(int roundCount)
        {
            if (lastSpecial + COOLDOWN >= roundCount)
            {
                return 0;
            }
            foreach (Unit person in team.teamMembers)
            {
                if (person.Life != 0)
                {
                    person.Life += CountAttack();
                }
            }
            lastSpecial = roundCount;
            return 0;
        }

    }
}
/*----------------------------------------------------------------------------------*/


namespace ObjectGame // BATTLE
{
    public enum ActionType
    {
        BASE,
        SPECIAL,
        POTION
    }

    public class BattleAction
    {
        public ActionType Type;
        public PotionTypeEnum PotionType;
        public BattleAction(ActionType type, PotionTypeEnum potion = PotionTypeEnum.LIFE_POTION)
        {
            Type = type;
            PotionType = potion;
        }
    }
    /// <summary>
	/// This class can create and execute new batle for two given teams.
	/// </summary>
    ///
    public class Battle
    {
        public readonly List<Unit> Team1;
        public readonly List<Unit> Team2;
        private readonly Team[] teams = new Team[2];
        private int roundCount = 0;
        private int[] lastPlayers = new int[] { -1, -1 };
        private List<BattleAction> appliedActions = new List<BattleAction>();

        private Queue<BattleAction> predefinedActions = new Queue<BattleAction>();

        private void ProcessDuelResults(Unit attacker, Unit target, ActionType t)
        {
            int attack_val;
            if (t == ActionType.BASE)
            {
                attack_val = attacker.BaseAction(roundCount);
            }
            else
            {
                attack_val = attacker.SpecialAction(roundCount);
            }

            int defense_val = target.CountDefense();

            int res = defense_val - attack_val;
            if (res < 0)
            {
                target.Life += res;
            }

            Console.Write("attack: {0}, defense {1}, res {2}", attack_val, defense_val, res);



        }

        public Battle(Team t1, Team t2)
        {
            teams[0] = t1;
            teams[1] = t2;
        }

        public Battle(Team t1, Team t2, Queue<BattleAction> predefined) : this(t1, t2)
        {
            this.predefinedActions = predefined;
        }

        private Unit GetNextPlayer(int teamNumber)
        {
            Team t = teams[teamNumber];
            return t.GetNextPlayer();

        }

        private void PrintAllTeam(int team)

        {
            List<Unit> t = teams[team].teamMembers;
            foreach (Unit u in t)

            {

                Console.Write("\nType: {0}, Life: {1} ", u.GetType(), u.Life);
            }
            Console.Write("\n");
        }

        private Unit GetNextTarget(int teamNumber)
        {
            Team t = teams[teamNumber];
            return t.GetNextTarget();

        }

        /// <summary>
        /// This method will start and execute a battle
        /// </summary>
        /// <returns>Returns the number of the winning team (1 or 2)</returns>
        public int Start()
        {

            int team = 0;
            Unit attacker = GetNextPlayer(team);

            BattleAction action;
            Unit target;

            while (attacker != null)
            {

                target = GetNextTarget((team + 1) % 2);
                if (target == null) break;

                action = GetNextAction();
                Console.WriteLine("\nRound: {0}, team:{1}, action:{2}, attacker:{3}, target: {4}",
roundCount, team + 1, action.Type, attacker.GetType(), target);

                appliedActions.Add(action);
                switch (action.Type)
                {
                    case ActionType.BASE:

                        ProcessDuelResults(attacker, target, ActionType.BASE);
                        break;

                    case ActionType.SPECIAL:

                        ProcessDuelResults(attacker, target, ActionType.SPECIAL);
                        break;

                    case ActionType.POTION:
                        attacker.UsePotion(action.PotionType);
                        break;
                }

                PrintAllTeam(0);
                PrintAllTeam(1);
                team = (team + 1) % 2;
                attacker = GetNextPlayer(team);
                roundCount++;
            }

            /*foreach(Action a in appliedActions)
            {
                Console.WriteLine("{0} ",a.Type);
            }*/

            return (team + 1) / 2 + 1;
        }

        private BattleAction GetNextAction()
        {
            if (predefinedActions.Count > 0) return predefinedActions.Dequeue();

            return null;
        }

        private BattleAction GetNextAction(Unit u)
        {
            if (roundCount - u.team.teamMembers.Count * ((Unit)u).COOLDOWN > u.lastSpecial)
            {
                return new BattleAction(ActionType.SPECIAL);
            }
            return new BattleAction(ActionType.BASE);
        }

    }
}

/*----------------------------------------------------------------------------------*/

namespace ObjectGame // HRA 
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Unit> t1 = new List<Unit>(); // team_1 a team_2,- List je čehokoliv, generika = typ seznamu (dostává instance jednotky)
            List<Unit> t2 = new List<Unit>();
            Team team1 = new Team(t1);
            Team team2 = new Team(t2);

            int a = Warrior.LIFE_MAX; // proc jenom u variora 
            Queue<BattleAction> akce = new Queue<BattleAction>();  // fronta akci 


            //Pro rucni vytvoreni tymu pouzijte:
            //CreateTeams(team1,team2);

            CreateTeamsConsole(team1, team2, akce); // teamy ye vstupu na konzoli -> nemus9me nic d2lat 

            Battle b = new Battle(team1, team2, akce);
            int winningTeam = b.Start();
            Console.WriteLine("TEAM {0} IS WINNING!", winningTeam);
        }

        static void CreateTeams(Team t1, Team t2) // jak vztvorit team ale abysme to nemuseli d2lat sama 
        {
            t1.Add(new Warrior(new List<Armor> {
                new Armor(BodyEnum.HEAD,2,1,ItemEnum.PRILBA),
                new Armor(BodyEnum.HAND1,2,2,ItemEnum.STIT ),
                new Armor(BodyEnum.HAND2,2,1,ItemEnum.MEC),
                new Armor(BodyEnum.BODY, 2,1, ItemEnum.KOZENAZBROJ)
            }, t1));

            t1.Add(new Archer(new List<Armor> {
                new Armor(BodyEnum.HEAD,2,0,ItemEnum.CAPKA),
                new Armor(BodyEnum.HAND2,2,1,ItemEnum.LUK),
                new Armor(BodyEnum.BODY, 4,0, ItemEnum.PLATENAZBROJ)
            }, t1));
            t1.Add(new Healer(new List<Armor> {
                new Armor(BodyEnum.BODY, 9,2, ItemEnum.ROBA)
            }, t1));

            t2.Add(new Archer(new List<Armor> {
                new Armor(BodyEnum.HEAD,0,2,ItemEnum.CAPKA),
                new Armor(BodyEnum.HAND2,3,1,ItemEnum.LUK),
                new Armor(BodyEnum.BODY, 0,7, ItemEnum.PLATENAZBROJ)
            }, t2));
            t2.Add(new Warrior(new List<Armor> {
                new Armor(BodyEnum.HAND1,0,2,ItemEnum.STIT ),
                new Armor(BodyEnum.HAND2,6,1,ItemEnum.MEC),
            }, t2));

            t2.Add(new Healer(new List<Armor> {
                new Armor(BodyEnum.BODY, 9,2, ItemEnum.ROBA)
            }, t2));

        }


        /// <summary>
        /// This very ugly method reads teams description and actions from a console.
        /// It does not support adding various types of items, only potions
        /// </summary>
        /// <param name="t1">Reference to Team1</param>
        /// <param name="t2">Reference to Team2</param>
        /// <param name="akce">List of actions</param>
        static void CreateTeamsConsole(Team t1, Team t2, Queue<BattleAction> akce)
        {
            Team[] teams = new Team[] { t1, t2 };

            string ln = Console.ReadLine(); // line 
            Unit u = null;
            List<Armor> equip = new List<Armor>();
            List<Item> bag = new List<Item>();

            int team = 0;

            while (true)
            {
                string[] parsed = ln.Split();
                if (ln.Length == 0 || ln[0] == '*')
                {
                    if (u != null)
                    {
                        u.AddArmor(equip);
                        u.AddToBag(bag);
                        equip = new List<Armor>();
                        bag = new List<Item>();
                        teams[team].Add(u);

                    }
                    if (ln.Length == 0) return;
                    team = int.Parse(parsed[1]);

                    switch (parsed[2])
                    {
                        case "w":
                            u = new Warrior(teams[int.Parse(parsed[1])]);
                            break;
                        case "a":
                            u = new Archer(teams[int.Parse(parsed[1])]);
                            break;
                        case "h":
                            u = new Healer(teams[int.Parse(parsed[1])]);
                            break;
                    }
                }
                else if (ln[0] == 'e')
                {
                    int bodypart = int.Parse(parsed[1]);
                    BodyEnum e = (BodyEnum)bodypart;
                    int attack = int.Parse(parsed[2]);
                    int defense = int.Parse(parsed[3]);
                    ItemEnum name = (ItemEnum)Enum.Parse(typeof(ItemEnum), parsed[4], true);
                    equip.Add(new Armor(e, attack, defense, name));


                }
                else if (ln[0] == '!')
                {
                    BattleAction a = null;
                    for (int i = 1; i < parsed.Length; i++)
                    {

                        ActionType t = (ActionType)Enum.Parse(typeof(ActionType), parsed[i], true);
                        if (t == ActionType.POTION)
                        {
                            i++;
                            PotionTypeEnum pt = (PotionTypeEnum)Enum.Parse(typeof(PotionTypeEnum), parsed[i], true);
                            a = new BattleAction(t, pt);
                        }
                        a = new BattleAction(t);
                        akce.Enqueue(a);

                    }
                }
                else
                {
                    if (ln[0] == 'P')
                    {
                        PotionItem p = new PotionItem((PotionTypeEnum)Enum.Parse(typeof(PotionTypeEnum), parsed[1], true));
                        bag.Add(p);
                    }
                }

                ln = Console.ReadLine();
            }
        }
    }
}