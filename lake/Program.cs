// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Nacti
    {
        public string[] nacti;
        public int pocet_jezer;
        public int pocet_dni;
        private string[] timelapse;
        public bool[] is_full;

        private int pomoc_1 = 0;
        public int[] pomoc_2;
        /*
        public void vestba()
        {
            this.nacti = (Console.ReadLine().Split(' '));
            this.pocet_jezer = Convert.ToInt32( nacti[0]);
            this.pocet_dni = Convert.ToInt32(nacti[1]);

            this.predpoved = Console.ReadLine().Split(' ');
        }*/

        public Nacti()
        {
            nacti = (Console.ReadLine().Split(' '));
            this.pocet_jezer = Convert.ToInt32(nacti[0]);
            this.pocet_dni = Convert.ToInt32(nacti[1]);

            is_full = new bool[pocet_jezer +1];

            timelapse = Console.ReadLine().Split(' ');
            pomoc_2 = new int[pocet_dni];
            foreach (string s in timelapse)
            {
                pomoc_2[pomoc_1] = Convert.ToInt32(s);
                this.pomoc_1++;

            }

        }

    }

    public class Node
    {
        public int Data { get; set; }
        public Node Next { get; set; }

        public Node Prev { get; set; }
        public Node(int data)
        {
            this.Data = data;
            this.Next = null;
            this.Prev = null;
        }
    }
    public class Fifo
    {
        private Node head;
        private Node tail;

        private int pocet = 0;
        public Fifo()
        {

        }
        public void AddToTail(int data)
        {
            var newNode = new Node(data);
            if (head == null)
            {
                head = newNode;
                tail = newNode;
                return;
            }
            tail.Next = newNode;
            newNode.Prev = tail;
            tail = newNode;
        }
        public int GetFromHead()
        {
            Node curr = head;
            if (head == null)
            {
                return 0;
            }
            else if (tail == head)
            {
                head = null;
                return curr.Data;

            }
            Node curr_next = head.Next;
            head = curr_next;
            curr.Next = null;
            curr_next.Prev = null;
            return curr.Data;

        }
        public int delka()
        {
            Node curr = head;

            while (curr != null)
            {
                pocet++;
                curr = curr.Next;

            }
            return pocet;
        }
    }
    public class Drak
    {
        public int pocet_jezer;
        public int pocet_dni;
        public bool[] is_full;
        public int[] pomoc_2;

        public bool odpoved = true;
        public int[] reseni;

        private int pochybny_parametr = 0;
        private int help;
        private int i;
        public int kolik_nul = 0;

        public Fifo list = new Fifo();
        public Drak(Nacti predpoved)
        {
            this.pocet_dni = predpoved.pocet_dni;
            this.pocet_jezer = predpoved.pocet_jezer;
            is_full = predpoved.is_full;
            pomoc_2 = predpoved.pomoc_2;

        }


        // cpu dny do fifo nez ynajdu nulu 
        public void Den()
        {
            this.i = pocet_dni - 1;
            while (true)
            {
                if( i < 0)
                {
                    if (list.delka() != 0)
                    {
                        this.odpoved = false;
                        
                    }
                    return;
                }
                if (pomoc_2[i] == 0 && pochybny_parametr == 0)
                {
                    i--;
                    kolik_nul++;
                    continue;
                    
                }
                if (pomoc_2[i] == 0 && pochybny_parametr == 1)
                {
                    reseni = new int[i + 1];
                    break;
                }
                else
                {
                    if (is_full[pomoc_2[i]] == true)
                    {
                        this.odpoved = false;
                        return;
                    }
                    is_full[pomoc_2[i]] = true;
                    list.AddToTail(pomoc_2[i]);
                    pochybny_parametr = 1;
                }
                i--;
            }
            for (int z = i; z >= 0; z--)
            {
                if (pomoc_2[z] == 0)
                {
                    // narazila si na nulu a ne na konci

                    help = list.GetFromHead();
                    reseni[z] = help;
                    is_full[help] = false;

                }

                else
                {
                    if (is_full[pomoc_2[z]] == true)
                    {
                        this.odpoved = false;
                        return;
                    }
                    is_full[pomoc_2[z]] = true;
                    list.AddToTail(pomoc_2[z]);
                    reseni[z] = -1;
                }
            }
            if (list.delka() != 0)
            {
                this.odpoved = false;
                return;
            }
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Nacti predpoved = new Nacti();

            Drak nas_drak = new Drak(predpoved);
            nas_drak.Den();

            if (nas_drak.odpoved == false)
            {
                Console.WriteLine("NE");
            }
            else
            {
                Console.WriteLine("ANO");

                foreach (int s in nas_drak.reseni)
                {
                    if (s != -1)
                    {
                        Console.Write(s + " ");
                    }

                }
                for (int p = nas_drak.kolik_nul; p > 0; p--)
                {
                    Console.Write(0 + " ");
                }
                
            }


            

            //Console.WriteLine(predpoved.pocet_jezer);
            //Console.WriteLine(predpoved.pocet_dni);
            //Console.WriteLine(predpoved.predpoved);
        }
    }
}
