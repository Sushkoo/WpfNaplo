using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfOsztalyzas
{
    public class Osztalyzat
    {
        private bool felcserel = false;
        String nev;
        String datum;
        String tantargy;
        int jegy;

        public Osztalyzat(string nev, string datum, string tantargy, int jegy)
        {
            this.nev = nev;
            this.datum = datum;
            this.tantargy = tantargy;
            this.jegy = jegy;
        }

        public string Nev { get => nev; }
        public string Datum { get => datum; }
        public string Tantargy { get => tantargy; }
        public int Jegy { get => jegy; }

        //todo Bővítse az osztályt! Készítsen CsaladiNev néven property-t, ami a névből a családi nevet adja vissza. Feltételezve, hogy a névnek csak az első tagja az.
        public string CsaladiNev

        {
            get
            {
                if (nev.Split(" ").Count() > 1 && felcserel == false)
                {
                    return nev.Split(" ")[0];
                }
                else if (nev.Split(" ").Count() > 1)
                {
                    return nev.Split(" ")[1];
                }
                else
                {
                    return Nev;
                }
            }
        }


        //todo Készítsen metódust ForditottNev néven, ami a két tagból álló nevek esetén megfordítja a névtagokat. Pld. Kiss Ádám => Ádám Kiss
        public void ForditottNev()
        {
            if (CsaladiNev == string.Empty)
            {
                return;
            }
            string[] temp = Nev.Split(" ");
            string nameTemp = temp[0];
            temp[0] = temp[1];
            temp[1] = nameTemp;
            this.nev = string.Join(" ", temp);
            felcserel = !felcserel;
        }
    }
}