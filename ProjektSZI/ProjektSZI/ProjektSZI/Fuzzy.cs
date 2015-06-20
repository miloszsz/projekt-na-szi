using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzyFramework;
using FuzzyFramework.Sets;
using FuzzyFramework.Dimensions;
using FuzzyFramework.Members;
using FuzzyFramework.Defuzzification;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ProjektSZI
{
    class Fuzzy
    {

        #region Parametry
        public static String[] nazwa = new String[24] 
        {
            "Chipsy",
            "Pizza",
            "Krowki",
            "Musli",
            "Zozole",
            "Fasolka",
            "Bulki",
            "Pomidory",
            "Jablka",
            "Banany",
            "Hot-dog",
            "Kebab",
            "Burger",
            "Kiwi",
            "Wafle",
            "Jogurt",
            "Precle",
            "Rogale",
            "Pierogi",
            "Bigos",
            "Kabanos",
            "Chleb",
            "Babka",
            "Frytki"
        };

        double[] wynik = new double[6];

        decimal[,] TABdopasowanie = new decimal[24, 6]
        {
            {90, 65, 15, 10, 35, 0},
            {45, 60, 0, 0, 98, 0},
            {65, 5, 100, 0, 0, 0},
            {65, 23, 47, 0, 12, 7},
            {72, 0, 100, 0, 0, 0},
            {22, 85, 0, 0, 7, 0},
            {33, 12, 0, 100, 0, 0},
            {23, 2, 0, 0, 0, 100},
            {37, 3, 8, 0, 0, 100},
            {38, 2, 4, 0, 0, 100},
            {48, 60, 0, 38, 100, 0},
            {12, 32, 0, 0, 100, 0},
            {42, 49, 0, 35, 100, 4},
            {13, 0, 5, 0, 0, 100},
            {61, 1, 34, 0, 0, 0},
            {37, 26, 24, 0, 0, 19},
            {65, 13, 89, 0, 0, 0},
            {71, 13, 72, 5, 0, 0},
            {15, 72, 0, 0, 0, 0},
            {7, 68, 0, 0, 0, 0},
            {47, 56, 0, 0, 32, 0},
            {22, 35, 0, 100, 12, 0},
            {10, 35, 45, 15, 0, 0},
            {65, 53, 0, 0, 70, 12},
        };

        public decimal[] TABodleglosc = new decimal[6] { 0, 0, 0, 0, 0, 0 };
        int max = 0;
        #endregion;

        #region Definicje
        //Wymiary wartosci wejsciowych
        static ContinuousDimension dopasowanie = new ContinuousDimension("Dopasowanie", "Dopasowanie do polki danego typu", "unit", 0, 100);
        static ContinuousDimension odleglosc = new ContinuousDimension("Odleglosc", "Odleglosc do polki danego typu", "unit", 0, 100);

        //Wymiary warotsci wyjsciowych
        static ContinuousDimension consequent = new ContinuousDimension("Przynaleznosc do poszczegolnych stopni", "0 = zly, 5 = bardzo dobry", "grade", 0, 5);

        //Zbiory na wymiarach wartosci wejsciowych
        static FuzzySet dobreDopasowanie = new LeftLinearSet(dopasowanie, "Dobrze dopasowany", 35, 70);
        static FuzzySet dalekoPolozony = new LeftLinearSet(odleglosc, "Daleko polozony", 20, 50);
        //Zbiory na wymiarach wartosci wyjsciowych
        static FuzzySet dobraPrzynaleznosc = new LeftLinearSet(consequent, "Wynik", 0, 5);

        //Relacja
        static FuzzyRelation dobryWynik = dobreDopasowanie & !dalekoPolozony;

        //Implikacja
        FuzzyRelation term = (dobryWynik & dobraPrzynaleznosc & !dalekoPolozony) | (!dobryWynik & !dobraPrzynaleznosc & dalekoPolozony);
        #endregion

        #region Metoda ustawiania odleglosci
        public void setTABodleglosc(decimal[] tablica)
        {
            for (int i = 0; i < 6; i++)
            {
                TABodleglosc[i] = tablica[i];
            }
        }
        #endregion

        #region Metoda wyboru, gdzie x to numer produktu -1, czyli 0-23 produkty
        public int wybierz(int x)
        {
            for (int j = 0; j <= 5; j++)
            {
                double jestDobry = dobryWynik.IsMember(
                    new Dictionary<IDimension, decimal>{
                                { dopasowanie, TABdopasowanie[x,j] },
                                { odleglosc, TABodleglosc[j] }
                        }
                );

                wynik[j] = jestDobry;

                for (decimal i = 0; i <= 5; i++)
                {
                    double membership = term.IsMember(
                        new Dictionary<IDimension, decimal>{
                            { dopasowanie, TABdopasowanie[x,j] },
                            { odleglosc, TABodleglosc[j] },
                            { consequent, i}
                        }
                    );
                }

                #region Deffuzification of the output set
                Defuzzification result = new CenterOfGravity(
                    term,
                    new Dictionary<IDimension, decimal>{
                        { dopasowanie, TABdopasowanie[x,j] },
                        { odleglosc, TABodleglosc[j] }
                    }
                );
                #endregion
            }

            max = 0;

            for (int i = 0; i < 6; i++)
            {
                if (wynik[i] > max)
                    max = i;
            }
            return max;
        }
        #endregion
    }
}