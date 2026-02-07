using System;
using System.Collections;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Vstup_a_tabulka
    {
        public int sirka;
        public int vyska;
        public string obsah_tabulky;
        public char[,] tabulka;
        public string tajenka;

        public Hashtable ht = new Hashtable(); // do hashtablu pod jednotlive keye nacpu lss hlava = value
        public Lss lss;
        public Node head;

        public void nacti_vstupy()
        {
            this.sirka = Convert.ToInt16(Console.ReadLine());
            this.vyska = Convert.ToInt16(Console.ReadLine());

            this.tabulka = new char[sirka, vyska];

            this.obsah_tabulky = Console.ReadLine();

            int coutn_x = 0;
            int coutn_y = 0;

            foreach (char c in obsah_tabulky)
            {

                if (coutn_y == vyska)
                {
                    return;
                }
                else if (coutn_x == sirka)
                {
                    coutn_x = 0;
                    coutn_y++;
                }
                tabulka[coutn_x, coutn_y] = c; // zbytecny 
                // kouknu se jestli takove heslo existuje, pokud ne pak zalozim novy seynam 
                if (ht[c] == null)
                {
                    //string variable_list = "list" + Convert.ToString(c);
                    //Lss this.Controls[variable_list] = new Lss();
                    lss = new Lss();
                    lss.AddToHead(coutn_x, coutn_y);
                    ht.Add(c, lss);


                }
                else
                {
                    lss = (Lss)ht[c];
                    lss.AddToHead(coutn_x, coutn_y);
                    ht[c] = lss;

                }
                coutn_x++;
            }

            this.tajenka = Console.ReadLine();

        }

        public void pomoc()
        {
            char[] charArr = tajenka.ToCharArray();

            int parameter = 0;
            int posledni = 0;

            int[,] vzdalenosti = new int[sirka, vyska];
            int[,] vzdalenosti_zkratka = new int[sirka, vyska];

            int i = 0;
            int j = 0;

            int count_i;
            int count_j;

            int pocet_kroku_x;
            int pocet_kroku_y;
            int pocet_kroku_celkem;
            int pocet_kroku_minule;

            Lss driv_lss;
            Node driv;
            Lss aktualni_lss;
            Node aktualni;

            for (int c = 0; c < charArr.Length; c++)
            {
                if (ht[charArr[c]] != null)
                {
                    aktualni_lss = (Lss)ht[charArr[c]];
                    aktualni = aktualni_lss.Returnhead();

                    //koukni na vzdalesti ve ktery se od naseho nachazi
                    while (aktualni != null)
                    {
                        count_i = aktualni.Val_i;
                        count_j = aktualni.Val_j;

                        if (c == 0 || parameter == 0)
                        {
                            pocet_kroku_x = count_i;
                            pocet_kroku_y = count_j;
                            pocet_kroku_celkem = pocet_kroku_x + pocet_kroku_y + 1;
                            vzdalenosti[count_i, count_j] = pocet_kroku_celkem;

                            aktualni = aktualni.Next;

                        }
                        else
                        {
                            driv_lss = (Lss)ht[charArr[posledni]];
                            driv = driv_lss.Returnhead();
                            while (driv != null)
                            {
                                i = driv.Val_i;
                                j = driv.Val_j;

                                pocet_kroku_minule = vzdalenosti[i, j];
                                pocet_kroku_x = count_i - i;
                                pocet_kroku_y = count_j - j;
                                if (pocet_kroku_x < 0) pocet_kroku_x *= -1;
                                if (pocet_kroku_y < 0) pocet_kroku_y *= -1;
                                pocet_kroku_celkem = pocet_kroku_x + pocet_kroku_y + pocet_kroku_minule + 1;

                                if (vzdalenosti_zkratka[count_i, count_j] != 0 && vzdalenosti_zkratka[count_i, count_j] < pocet_kroku_celkem)
                                { }
                                else
                                {
                                    vzdalenosti_zkratka[count_i, count_j] = pocet_kroku_celkem;
                                }

                                if (aktualni.Next == null)
                                {
                                    if (charArr[c] != charArr[posledni])
                                    {
                                        vzdalenosti[i, j] = 0;
                                    }
                                }

                                driv = driv.Next;
                            }
                            vzdalenosti[count_i, count_j] = vzdalenosti_zkratka[count_i, count_j];


                            vzdalenosti_zkratka[count_i, count_j] = 0;

                            aktualni = aktualni.Next;
                            if (charArr[c] == 'c')
                            {
                                //Tiskni_tabulku(vzdalenosti);
                                //Tiskni_tabulku(vzdalenosti_zkratka);
                            }
                        }


                    }
                    parameter = 1;
                    //Tiskni_tabulku(vzdalenosti);
                    posledni = c;
                }
                else
                {

                }
            }

            int min;
            int porovnej;
            int vhe = charArr.Length - 1;
            while (ht[charArr[vhe]] == null)
            {
                vhe--;
                if (vhe == -1)
                {
                    return;
                }
            }
            aktualni_lss = (Lss)ht[charArr[vhe]];
            aktualni = aktualni_lss.Returnhead();

            min = vzdalenosti[aktualni.Val_i, aktualni.Val_j];
            aktualni = aktualni.Next;
            while (aktualni != null)
            {
                i = aktualni.Val_i;
                j = aktualni.Val_j;
                porovnej = vzdalenosti[i, j];
                if (porovnej < min)
                {
                    min = porovnej;
                }
                aktualni = aktualni.Next;
            }
            Console.WriteLine(min);
            return;

            /*
            char[] charArr = tajenka.ToCharArray();
            
            int[,] vzdalenosti = new int[sirka, vyska];
            int[,] vzdalenosti_zkratka = new int[sirka, vyska];

            int i;
            int j;

            int count_i;
            int count_j;

            int pocet_kroku_x;
            int pocet_kroku_y;
            int pocet_kroku_celkem;
            int pocet_kroku_minule;

            Lss ojednodriv_lss;
            Node ojednodriv;
            Lss aktualni_lss;
            Node aktualni;
            int predchod = 0;

            if (ht[charArr[0]] != null)
            {
                aktualni_lss = (Lss)ht[charArr[0]];
                aktualni = aktualni_lss.Returnhead();

                //koukni na vzdalesti ve ktery se od naseho nachazi
                while (aktualni != null)
                {

                    count_i = aktualni.Val_i;
                    count_j = aktualni.Val_j;

                    pocet_kroku_x = count_i;
                    //if (pocet_kroku_x < 0) pocet_kroku_x *= -1;
                    pocet_kroku_y = count_j;

                    pocet_kroku_celkem = pocet_kroku_x + pocet_kroku_y + 1;

                    vzdalenosti[count_i, count_j] = pocet_kroku_celkem;



                    aktualni = aktualni.Next;
                }
                
            }

            


            for (int c = 1; c < charArr.Length; c++)
            {
                if (ht[charArr[c]] == null)
                {
                    continue;
                }
                aktualni_lss = (Lss)ht[charArr[c]];
                aktualni = aktualni_lss.Returnhead();

                while (aktualni != null)
                {
                    count_i = aktualni.Val_i;
                    count_j = aktualni.Val_j;

                    ojednodriv_lss = (Lss)ht[charArr[predchod]];
                    ojednodriv = ojednodriv_lss.Returnhead();

                    while (ojednodriv != null)
                    {

                        i = ojednodriv.Val_i;
                        j = ojednodriv.Val_j;

                        pocet_kroku_minule = vzdalenosti[i, j];

                        pocet_kroku_x = count_i - i;
                        pocet_kroku_y = count_j - j;

                        if (pocet_kroku_x < 0) pocet_kroku_x *= -1;
                        if (pocet_kroku_y < 0) pocet_kroku_y *= -1;

                        pocet_kroku_celkem = pocet_kroku_x + pocet_kroku_y + pocet_kroku_minule +1;

                        if (vzdalenosti_zkratka[count_i, count_j] != 0 && vzdalenosti_zkratka[count_i, count_j] < pocet_kroku_celkem)
                        {

                        }
                        else
                        {
                            vzdalenosti_zkratka[count_i, count_j] = pocet_kroku_celkem;
                        }

                        if (aktualni.Next == null)
                        {
                            vzdalenosti[i, j] = 0;
                        }

                        ojednodriv = ojednodriv.Next;

                    }
                    vzdalenosti[count_i, count_j] = vzdalenosti_zkratka[count_i, count_j];
                    vzdalenosti_zkratka[count_i, count_j] = 0;
                    aktualni = aktualni.Next;
                }

                //Tiskni_tabulku(vzdalenosti);
                predchod = c;

            }
            int min;
            int porovnej;
            int vhe = charArr.Length - 1;
            while (ht[charArr[vhe]] == null)
            {
                vhe--;
                if (vhe == -1)
                {
                    return;
                }
            }
            aktualni_lss = (Lss)ht[charArr[vhe]];
            
            aktualni = aktualni_lss.Returnhead();
            min = vzdalenosti[aktualni.Val_i, aktualni.Val_j];
            aktualni = aktualni.Next;
            while (aktualni != null)
            {
                i = aktualni.Val_i;
                j = aktualni.Val_j;
                porovnej = vzdalenosti[i, j];
                if (porovnej < min)
                {
                    min = porovnej;
                }
                aktualni = aktualni.Next;
            }
            Console.WriteLine(min);
            return;

*/

        }
        public void heeelp()
        {
            char[] charArr = tajenka.ToCharArray();
            Lss cesta = (Lss)ht[charArr[1]];
            // lss.head.val_li


        }

        public void tiskni_vstupy()
        {
            Console.WriteLine(sirka);
            Console.WriteLine(vyska);
            Console.WriteLine(obsah_tabulky);
            for (int i = 0; i < vyska; i++)
            {
                for (int j = 0; j < sirka; j++)
                {
                    Console.Write(tabulka[j, i]);

                }
                Console.WriteLine();
            }

        }
        public void Tiskni_tabulku(int[,] vydalenosti)
        {
            for (int i = 0; i < vyska; i++)
            {
                for (int j = 0; j < sirka; j++)
                {
                    Console.Write(vydalenosti[j, i]);

                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
    public class Node
    {
        //pozice -> i j tedy dve hodnoty
        public int Val_i { get; set; }
        public int Val_j { get; set; }
        public Node Next { get; set; }
        public Node(int data_i, int data_j)
        {
            this.Val_i = data_i;
            this.Val_j = data_j;
            this.Next = null;
        }
    }
    public class Lss
    {
        public Node head; //protoye ho y jinud nbudu menit 
        private int pocet = 0;


        public Lss()
        {

        }

        public void AddToHead(int i, int j)
        {
            Node newNode = new Node(i, j);
            if (head == null)
            {
                head = newNode;
                return;
            }
            newNode.Next = head;
            head = newNode;
        }
        public void Print()
        {
            while (head != null)
            {
                Console.Write(head.Val_i);
                Console.Write(head.Val_j);
                Console.WriteLine();
                head = head.Next;
            }
        }
        public Node Returnhead()
        {
            return head;
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            /* vstup tabulky a ulozeni do 2d prostoru
             * vytvoreni hash tabulky a lin spojovyho seznamu
             * zjistovani delky
             */

            Vstup_a_tabulka zadani = new Vstup_a_tabulka();
            zadani.nacti_vstupy();
            //zadani.tiskni_vstupy();
            //zadani.pomoc();
            //zadani.heeelp();
            zadani.pomoc();
        }
    }
}
//ctrl k d 
/*
3  
3  
ABCBFECDF  
ABCDEFA  
*/