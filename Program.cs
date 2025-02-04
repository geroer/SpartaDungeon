using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace SpartaDungeon
{
    enum Job
    {
        전사 = 1,
        도적
    }

    public class Player
    {
        public int Level { get; set; } = 1;
        public String Name { get; set; }
        public int Job { get; set; }
        public int Attack { get; set; } = 10;
        public int Defense { get; set; } = 5;
        public int Health { get; set; } = 100;
        public int Gold { get; set; } = 1500;

        public List<Item> Inventory;

        public Player()
        {
            //테스트용 인벤토리
            Inventory = new List<Item>();
            Inventory.Add(new Item(1, "수련자 갑옷", 2, 5, "수련에 도움을 주는 갑옷입니다.", 1000));
            Inventory.Add(new Item(2, "무쇠갑옷", 2, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000));
            Inventory.Add(new Item(5, "청동 도끼", 1, 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500));
            Inventory[0].IsEquipped = true;
            Inventory[1].IsEquipped = true;
            for (int i = 0; i < Inventory.Count; i++)
            {
                Inventory[i].Isbuy = true;
            }
        }
    }

    public class Item
    {
        public int ItemCode { get; set; }
        public String ItemName { get; set; }
        public int ItemType { get; set; }
        public int ItemSpec { get; set; }
        public String ItemDesc { get; set; }
        public int ItemPrice { get; set; }
        public bool Isbuy { get; set; } = false;
        public bool IsEquipped { get; set; } = false;

        public Item(int Itemcode, String itemname, int itemtype, int itemspec, String itemdesc, int itemprice)
        {
            ItemCode = Itemcode;
            ItemName = itemname;
            ItemType = itemtype;
            ItemSpec = itemspec;
            ItemDesc = itemdesc;
            ItemPrice = itemprice;
        }
    }

    public class GamePlay
    {
        public Player player;
        public Shop shop;

        public GamePlay(Player player, Shop shop)
        {
            this.player = player;
            this.shop = shop;
        }

        public void start()
        {
            String selectedMenu;
            String playerName;
            String playerJob;

            Console.Clear();
            while (true)
            {
                //플레이어 이름 설정
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n원하시는 이름을 설정해주세요.");
                Console.Write(">> ");
                playerName = Console.ReadLine();
                Console.WriteLine();

                jmp1:
                Console.WriteLine($"입력하신 이름은 {playerName} 입니다.\n");
                Console.WriteLine("1. 저장\n2. 취소\n\n원하시는 행동을 입력해주세요");
                Console.Write(">> ");
                selectedMenu = Console.ReadLine();

                Console.WriteLine();
                if (selectedMenu != "1" && selectedMenu != "2")
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                    goto jmp1;
                }
                else if (selectedMenu == "2")
                {
                    Console.Clear();
                    continue;
                }
                else
                {
                    player.Name = playerName;
                    break;
                }
            }

            while (true)
            {
                //플레이어 직업 설정
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n원하시는 직업을 설정해주세요.");
                jmp2:
                Console.WriteLine("1. 전사\n2. 도적\n\n원하시는 행동을 입력해주세요");
                Console.Write(">> ");
                playerJob = Console.ReadLine();
                Console.WriteLine();

                if (playerJob != "1" && playerJob != "2")
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                    goto jmp2;
                }
                jmp3:
                Console.WriteLine($"선택하신 직업은 {(Job)int.Parse(playerJob)} 입니다.\n");
                Console.WriteLine("1. 저장\n2. 취소\n\n원하시는 행동을 입력해주세요");
                Console.Write(">> ");
                selectedMenu = Console.ReadLine();

                Console.WriteLine();
                if (selectedMenu != "1" && selectedMenu != "2")
                {
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다.");
                    goto jmp3;
                }
                else if (selectedMenu == "2")
                {
                    continue;
                }
                else
                {
                    player.Job = int.Parse(playerJob);
                    break;
                }
            }
            MainMenu();
        }

        public void MainMenu()
        {
            string selectedMenu;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
                Console.WriteLine("1. 상태보기\n2. 인벤토리\n3. 상점\n\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
            jmp1: //메뉴 선택이 잘못 된 경우 다시 선택
                selectedMenu = Console.ReadLine();
                Console.WriteLine();
                if (selectedMenu != "1" && selectedMenu != "2" && selectedMenu != "3")
                {
                    Console.Write("잘못된 입력입니다. 다시 선택해주세요\n>> ");
                    goto jmp1;
                }

                switch (selectedMenu)
                {
                    case "1":
                        ChracterInfo();
                        break;
                    case "2":
                        ItemInfo();
                        break;
                    case "3":
                        shop.ShopOpen();
                        break;
                }
            }
        }

        public void ChracterInfo()
        {
            string selectedMenu;
            int itemAttack = 0;
            int itemDefense = 0;

            for (int i = 0; i < player.Inventory.Count; i++)
            {
                if (player.Inventory[i].IsEquipped)
                {
                    if (player.Inventory[i].ItemType == 1)
                    {
                        itemAttack += player.Inventory[i].ItemSpec;
                    }
                    else if (player.Inventory[i].ItemType == 2)
                    {
                        itemDefense += player.Inventory[i].ItemSpec;
                    }
                }
            }

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("상태보기");
            Console.ResetColor();
            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");
            Console.WriteLine($"Lv. {player.Level}");
            Console.WriteLine($"{player.Name} ( {(Job)player.Job} )");
            if (itemAttack != 0) Console.WriteLine($"공격력 : {player.Attack+itemAttack} (+{itemAttack})");
            else Console.WriteLine($"공격력 : {player.Attack}");
            if (itemDefense != 0) Console.WriteLine($"방어력 : {player.Defense+itemDefense} (+{itemDefense})");
            else Console.WriteLine($"방어력 : {player.Defense}");
            Console.WriteLine($"체력 : {player.Health}");
            Console.WriteLine($"Gold : {player.Gold} G");
            Console.WriteLine("\n0. 나가기\n\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            jmp1:
            selectedMenu = Console.ReadLine();
            Console.WriteLine();
            if (selectedMenu != "0")
            {
                Console.Write("잘못된 입력입니다. 다시 선택해주세요\n>> ");
                goto jmp1;
            }

            MainMenu();
        }
        public void ItemInfo()
        {
            string selectedMenu;
            string itemTypeName;
            string Equip;

            Console.Clear();

            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < player.Inventory.Count(); i++)
            {
                if (player.Inventory[i].ItemType == 1) itemTypeName = "공격력";
                else if (player.Inventory[i].ItemType == 2) itemTypeName = "방어력";
                else itemTypeName = "";

                if (player.Inventory[i].IsEquipped) Equip = "[E]";
                else Equip = "";

                Console.WriteLine($" - {Equip}{player.Inventory[i].ItemName}\t | {itemTypeName} +{player.Inventory[i].ItemSpec} | {player.Inventory[i].ItemDesc}");
            }
            Console.WriteLine();
            Console.WriteLine("1. 장착 관리\n0. 나가기\n\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
        jmp1:
            selectedMenu = Console.ReadLine();
            Console.WriteLine();
            if (selectedMenu != "1" && selectedMenu != "0")
            {
                Console.Write("잘못된 입력입니다. 다시 선택해주세요\n>> ");
                goto jmp1;
            }
            switch (selectedMenu)
            {
                case "1":
                    ItemEquip();
                    break;
                case "0":
                    break;
            }
        }

        public void ItemEquip()
        {
            string selectedMenu;
            string itemTypeName;
            string Equip;
            int menuNum;

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.ResetColor();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < player.Inventory.Count; i++)
            {
                if (player.Inventory[i].ItemType == 1) itemTypeName = "공격력";
                else if (player.Inventory[i].ItemType == 2) itemTypeName = "방어력";
                else itemTypeName = "";

                if (player.Inventory[i].IsEquipped) Equip = "[E]";
                else Equip = "";

                Console.WriteLine($" - {i + 1} {Equip}{player.Inventory[i].ItemName}\t | {itemTypeName} +{player.Inventory[i].ItemSpec} | {player.Inventory[i].ItemDesc}");
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기\n\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
        jmp1:
            selectedMenu = Console.ReadLine();
            Console.WriteLine();
            if (int.TryParse(selectedMenu, out menuNum))
            {
                if (int.Parse(selectedMenu) > player.Inventory.Count || int.Parse(selectedMenu) < 0)
                {
                    Console.Write("잘못된 입력입니다. 다시 선택해주세요\n>> ");
                    goto jmp1;
                }
                else
                {
                    switch (selectedMenu)
                    {
                        case "0":
                            ItemInfo();
                            break;
                        default:
                            player.Inventory[menuNum - 1].IsEquipped = !player.Inventory[menuNum - 1].IsEquipped;
                            ItemEquip();
                            break;
                    }
                }
            }
            else
            {
                Console.Write("잘못된 입력입니다. 다시 선택해주세요\n>> ");
                goto jmp1;
            }
        }
    }

    public class Shop
    {
        Player player;
        public List<Item> itemList = new List<Item>();

        public Shop(ref Player player)
        {
            this.player = player;

            itemList.Add(new Item(1, "수련자 갑옷", 2, 5, "수련에 도움을 주는 갑옷입니다.", 1000));
            itemList.Add(new Item(2, "무쇠갑옷", 2, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2000));
            itemList.Add(new Item(3, "스파르타의 갑옷", 2, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500));
            itemList.Add(new Item(4, "낡은 검", 1, 2, "쉽게 볼 수 있는 낡은 검 입니다.", 600));
            itemList.Add(new Item(5, "청동 도끼", 1, 5, "어디선가 사용됐던거 같은 도끼입니다.", 1500));
            itemList.Add(new Item(6, "스파르타의 창", 1, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 3000));

            for (int i = 0; i < itemList.Count; i++)
            {
                for (int j = 0; j < player.Inventory.Count; j++)
                {
                    if (itemList[i].ItemCode == player.Inventory[j].ItemCode)
                    {
                        itemList[i].Isbuy = true;
                    }
                }
            }
        }

        public void ShopOpen()
        {
            string selectedMenu;
            string itemTypeName;

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("상점");
            Console.ResetColor();
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
            Console.WriteLine($"[보유 골드]\n{player.Gold} G\n");
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < itemList.Count(); i++)
            {
                if (itemList[i].ItemType == 1) itemTypeName = "공격력";
                else if (itemList[i].ItemType == 2) itemTypeName = "방어력";
                else itemTypeName = "";

                if (itemList[i].Isbuy)
                {
                    Console.WriteLine($" - {itemList[i].ItemName}\t | {itemTypeName} +{itemList[i].ItemSpec} | {itemList[i].ItemDesc}\t | 구매완료");
                }
                else
                {
                    Console.WriteLine($" - {itemList[i].ItemName}\t | {itemTypeName} +{itemList[i].ItemSpec} | {itemList[i].ItemDesc}\t | {itemList[i].ItemPrice} G");
                }
            }
            Console.WriteLine();
            Console.WriteLine("1. 아이템 구매\n0. 나가기\n\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
        jmp1:
            selectedMenu = Console.ReadLine();
            Console.WriteLine();
            if (selectedMenu != "1" && selectedMenu != "0")
            {
                Console.Write("잘못된 입력입니다. 다시 선택해주세요\n>> ");
                goto jmp1;
            }
            switch (selectedMenu)
            {
                case "1":
                    BuyItem();
                    break;
                case "0":
                    break;
            }
        }

        public void BuyItem()
        {
            string selectedMenu;
            string itemTypeName;
            int menuNum;

            while (true)
            {
                Console.Clear();
                jmp2:
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("상점 - 아이템 구매");
                Console.ResetColor();
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine($"[보유 골드]\n{player.Gold} G\n");
                Console.WriteLine("[아이템 목록]");

                for (int i = 0; i < itemList.Count(); i++)
                {
                    if (itemList[i].ItemType == 1) itemTypeName = "공격력";
                    else if (itemList[i].ItemType == 2) itemTypeName = "방어력";
                    else itemTypeName = "";

                    if (itemList[i].Isbuy)
                    {
                        Console.WriteLine($" - {i + 1} {itemList[i].ItemName}\t | {itemTypeName} +{itemList[i].ItemSpec} | {itemList[i].ItemDesc}\t | 구매완료");
                    }
                    else
                    {
                        Console.WriteLine($" - {i + 1} {itemList[i].ItemName}\t | {itemTypeName} +{itemList[i].ItemSpec} | {itemList[i].ItemDesc}\t | {itemList[i].ItemPrice} G");
                    }
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기\n\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
            jmp1:
                selectedMenu = Console.ReadLine();
                Console.WriteLine();
                if (int.TryParse(selectedMenu, out menuNum))
                {
                    if (int.Parse(selectedMenu) > itemList.Count || int.Parse(selectedMenu) < 0)
                    {
                        Console.Write("잘못된 입력입니다. 다시 선택해주세요\n>> ");
                        goto jmp1;
                    }
                    else
                    {
                        if (selectedMenu == "0")
                        {
                            break;
                        }
                        else
                        {
                            if (itemList[menuNum - 1].Isbuy)
                            {
                                Console.Clear();
                                Console.WriteLine("이미 구매한 아이템입니다.\n");
                                goto jmp2;
                            }
                            else
                            {
                                if (player.Gold >= itemList[menuNum - 1].ItemPrice)
                                {
                                    itemList[menuNum - 1].Isbuy = true;
                                    player.Inventory.Add(itemList[menuNum - 1]);
                                    player.Gold -= itemList[menuNum - 1].ItemPrice;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("골드가 부족합니다.\n");
                                    goto jmp2;
                                }
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    Console.Write("잘못된 입력입니다. 다시 선택해주세요\n>> ");
                    goto jmp1;
                }
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            Shop shop = new Shop(ref player);
            GamePlay gamePlay = new GamePlay(player, shop);

            gamePlay.start();
        }
    }
}
