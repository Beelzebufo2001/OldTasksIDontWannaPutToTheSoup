using System;
using System.Text;

namespace bludiste
{
    class Program
    {
        private static readonly char[] SMER_ZNAK = { '^', '>', 'v', '<' }; //private-> moje, static -> všechny objekty společný -> readonly-> jen číst, seznam charaterů

        public enum Smer // enum je 0,1,2,3
        { // vlastnost která se nebude měnit a budu ji znát pořád 
            NAHORU,
            DOPRAVA,
            DOLU,
            DOLEVA
        }

        class Bludiste
        {
            public int sirka;
            public int vyska;
            public bool[,] mapa; // true = volny
            public int priseraX;
            public int priseraY;
            public Smer priseraSmer; //objekt typu směr 

            public Bludiste(string jmeno)
            {
                // TODO nacti sirka
                this.sirka = Convert.ToInt32(Console.ReadLine());
                // TODO nacti vyska
                this.vyska = Convert.ToInt32(Console.ReadLine());
                mapa = new bool[vyska, sirka]; //řádky a sloupci
                NactiMapu();
            }

            /// <summary>
            /// Metoda nacte mapu ze vstupu a ulozi ji do promenne this.mapa
            /// </summary>
            private void NactiMapu()
            {
                for (int i = 0; i < vyska; i++)
                {
                    string cely_radek = Console.ReadLine();

                    for (int j = 0; j < sirka; j++)
                    {
                        char pozice = cely_radek[j];

                        if (pozice == '.') // uvozovky jsou string a apostrofy jsou char
                        {
                            mapa[i, j] = true;
                        }
                        else if (pozice == 'X' || pozice == 'x')
                        {
                            mapa[i, j] = false;
                        }
                        else
                        {
                            mapa[i, j] = true;
                            this.priseraX = j;
                            this.priseraY = i;

                            for (int k = 0; k <= 3; k++)
                            {
                                if (SMER_ZNAK[k] == pozice)
                                {
                                    this.priseraSmer = (Smer)k; // pretypovani


                                }

                            }

                        }

                    }

                }

            }

            public void VypisMapu()
            {
                for (int y = 0; y < vyska; y++)
                {
                    for (int x = 0; x < sirka; x++)
                        if ((y == priseraY) && (x == priseraX))
                            Console.Write(SMER_ZNAK[(int)priseraSmer]);
                        else
                        //kdyz je v if/else/for atdd jen jeden prikaz, netreba psat {}
                            if (mapa[y, x]) Console.Write('.');
                        else Console.Write('X');
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        /// <summary>        /// Predstavuje tridu priser. Ne vsechny prisery se pohybuji
        /// podel prave zdi, ale vsechny maji polohu, smer atd...
        /// </summary>
        abstract class ObecnaPrisera // abstrakt -> nevztvarej instance 
        {
            protected int x;
            protected int y;
            protected Smer smer;
            protected Bludiste bludiste;


            /// <summary>
            /// Posune priseru o jednu pozici v jejim smeru
            /// </summary>
            protected void Kupredu()
            {
                int k = (int)smer;// zepta se nejdrive parapetru smer ktery odkaze na Smer a chci int
                if (k == 0)//nahoru //y se zmensi o jedna a x je stejne
                {
                    this.y = y - 1;
                }
                else if (k == 1)//doprava // x se o jedna zvetsi a y je stejne
                {
                    this.x = x + 1;
                }
                else if (k == 2)//dolu // y se zvetsi
                {
                    this.y = y + 1;
                }
                else if (k == 3)//doleva
                {
                    this.x = x - 1;
                }
                // ted potrebuju uloyit polohu do obou trid 
                bludiste.priseraX = x;
                bludiste.priseraY = y;
                // smer zustava a myslim ze poloha se dedi
                //nemela bych ale prepsat mapu na prazdne pole kde jsem stala? -> myslim ze nemusim


            }

            /// <summary>
            /// Otoci priseru doprava
            /// napr je-li vlevo, otoci se nahoru
            /// </summary>
            protected void OtocDoprava()
            {
                int k = (int)smer;
                k += 1;
                if (k>3)
                {
                    k = 0;
                }
                bludiste.priseraSmer = (Smer)k;// menim smer v nasom bludisti ale to neymeni muj smer a smer pravackz priserkz
                smer = (Smer)k; // takd zmenim informace o tom jak se ma menit smer v obecne proseyce a to vzvola dobrovolnz rust v pravacke priserce

            }

            /// <summary>
            /// Otoci priseru doleva
            /// </summary>
            protected void OtocDoleva()
            {
                int k = (int)smer;
                k -= 1;
                if (k < 0)
                {
                    k = 3;
                }
                bludiste.priseraSmer = (Smer)k;
                smer = (Smer)k;
            }

            /// <summary>
            /// Jeden krok prisery, misto kde se rozhoduje otoceni atd.
            /// </summary>
            abstract public void Krok();
        }

        /// <summary>
        /// Tato prisera chodi podle prave zdi
        /// </summary>
        class PravackaPrisera : ObecnaPrisera
        {
            public PravackaPrisera(Bludiste bludiste)//obsahuje cele bludiste
            {
                this.bludiste = bludiste;
                x = bludiste.priseraX;
                y = bludiste.priseraY;
                smer = bludiste.priseraSmer;
            }
            int pochybny_parameter = 0;

            /// <summary>
            /// Tato metoda resi, co udela prisera v kazdem kroku
            /// prisera se muze otocit doleva, otocit doprava nebo postoupit o
            /// jedno pole dopredu ve smeru.
            /// </summary>
            public override void Krok() // potrebuju koukat ze na pravo je stena -> predpoklad je ze je ale musim v tom pokracovat
            {
                // if poprave strane stena a predemnou volno 
                /* the wall... i can use x and y by looking at my direction what the right side is 
                 */
                int k = (int)smer;
                int wall_x;
                int wall_y;

                int maybe_wall_x;
                int maybe_wall_y;



                if (k == 0)//nahoru // right side is by x = x +1 and y =y
                {
                    wall_x = x + 1;
                    wall_y = y;

                    maybe_wall_x = x;
                    maybe_wall_y = y-1;

                    
                }
                else if (k == 1)//doprava // 
                {
                    wall_x = x;
                    wall_y = y+1;

                    maybe_wall_x = x+1;
                    maybe_wall_y = y;

                }
                else if (k == 2)//dolu // 
                {
                    wall_x = x - 1;
                    wall_y = y;

                    maybe_wall_x = x;
                    maybe_wall_y = y+1;

                }
                else
                {
                    wall_x = x;
                    wall_y = y -1;

                    maybe_wall_x = x - 1;
                    maybe_wall_y = y;

                }
                //when i dont feel the wall by my right side i must turn to right!
                if (bludiste.mapa[wall_y, wall_x] == true)
                {
                    //koukni se je mozne ze chces jit rovne -> je neco za tebu ? if yes then asi chces rovne
                    if (pochybny_parameter == 1)
                    {
                        pochybny_parameter = 0;
                        Kupredu();
                    }
                    else
                    {
                        pochybny_parameter = 1;
                        OtocDoprava();
                        
                    }
                    
                    
                }
                else
                {
                    //koukni se jestli je pred tebou stena
                    if (bludiste.mapa[maybe_wall_y,maybe_wall_x] == false)
                    {
                        pochybny_parameter = 0;
                        OtocDoleva();
                    }
                    // neni prede mnou stena 
                    else
                    {
    
                        pochybny_parameter = 0;
                        Kupredu();
                    }
                }
            }
        }



        static void Main(string[] args)
        {
            Bludiste bludiste = new Bludiste(@"C:\Users\robli\Desktop\VSC 22\game_board.txt");
            PravackaPrisera priserka = new PravackaPrisera(bludiste);

            for (int i = 1; i <= 20; i++)
            {
                priserka.Krok();
                bludiste.VypisMapu();
            }
        }
    }
}