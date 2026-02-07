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
            public bool[,] mapa;
            private int pocitadlo = 0;
            public int[,] prisera;
            public Smer[] priseraSmer;
            public int pocet_sester;

            public Bludiste(string jmeno, int pocet_sester)
            {
                // TODO nacti sirka
                this.sirka = Convert.ToInt32(Console.ReadLine());
                // TODO nacti vyska
                this.vyska = Convert.ToInt32(Console.ReadLine());
                mapa = new bool[vyska, sirka]; //řádky a sloupci
                NactiMapu();
                prisera = new int[pocet_sester, pocet_sester];
                priseraSmer = new Smer[pocet_sester];
                this.pocet_sester = pocet_sester;
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
                            if (pocitadlo < pocet_sester)
                            {

                                mapa[i, j] = true;
                                this.prisera[pocitadlo, 1] = j;
                                this.prisera[pocitadlo, 0] = i;

                                for (int k = 0; k <= 3; k++)
                                {
                                    if (SMER_ZNAK[k] == pozice)
                                    {
                                        this.priseraSmer[pocitadlo] = (Smer)k; // pretypovani


                                    }

                                }
                                pocitadlo++;
                            }


                        }

                    }

                }
                VypisMapu();
            }


            public void VypisMapu()
            {
                for (int y = 0; y < vyska; y++)
                {
                    for (int x = 0; x < sirka; x++)
                    {
                        for (int z = 0; z < pocet_sester; z++)
                        {
                            if ((y == prisera[z, 1]) && (x == prisera[z, 0]))
                            {
                                Console.Write(SMER_ZNAK[(int)priseraSmer[z]]);
                            }
                        }

                        if (mapa[y, x]) Console.Write('.');
                        else Console.Write('X');

                        //kdyz je v if/else/for atdd jen jeden prikaz, netreba psat {}    
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Predstavuje tridu priser. Ne vsechny prisery se pohybuji
        /// podel prave zdi, ale vsechny maji polohu, smer atd...
        /// </summary>
        abstract class ObecnaPrisera // abstrakt -> nevztvarej instance 
        {



            protected int x;
            protected int y;
            protected int parametr;
            protected Smer smer;
            protected int pocet_sester;

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
                bludiste.prisera[parametr, 0] = x;
                bludiste.prisera[parametr, 1] = y;
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
                if (k > 3)
                {
                    k = 0;
                }
                bludiste.priseraSmer[parametr] = (Smer)k;// menim smer v nasom bludisti ale to neymeni muj smer a smer pravackz priserkz
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
                bludiste.priseraSmer[parametr] = (Smer)k;
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
            public PravackaPrisera(Bludiste bludiste, int parametr)//obsahuje cele bludiste
            {
                this.bludiste = bludiste;
                this.parametr = parametr;
                this.pocet_sester = bludiste.pocet_sester;
                x = bludiste.prisera[parametr, 0];
                y = bludiste.prisera[parametr, 1];
                smer = bludiste.priseraSmer[parametr];
            }


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
                int pochybny_parameter = 0;
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
                    maybe_wall_y = y - 1;


                }
                else if (k == 1)//doprava // 
                {
                    wall_x = x;
                    wall_y = y + 1;

                    maybe_wall_x = x + 1;
                    maybe_wall_y = y;

                }
                else if (k == 2)//dolu // 
                {
                    wall_x = x - 1;
                    wall_y = y;

                    maybe_wall_x = x;
                    maybe_wall_y = y + 1;

                }
                else
                {
                    wall_x = x;
                    wall_y = y - 1;

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
                    if (bludiste.mapa[maybe_wall_y, maybe_wall_x] == false)
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
            string moje_blbe_bludiste = "MOJE BLBÉ BLUDISTE";
            int pocet_priser = 2;
            Bludiste bludiste = new Bludiste(moje_blbe_bludiste, pocet_priser);
            int parametr_1 = 0;
            int parametr_2 = 1;

            PravackaPrisera priserka_1 = new PravackaPrisera(bludiste, parametr_1);
            PravackaPrisera priserka_2 = new PravackaPrisera(bludiste, parametr_2);

            for (int i = 1; i <= 20; i++)
            {
                priserka_1.Krok();
                // prvni prisera kouka na tu druhou jako na plochu - nikdy neceka ye ji uhne !! NIKDZ
                priserka_2.Krok();
                bludiste.VypisMapu();
            }
        }
    }
}

//když chcou jit na stejný políčko 
//když koukají na sebe 
