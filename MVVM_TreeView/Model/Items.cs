using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace MVVM_TreeView.Model
{
    public static class Names
    {
        private static List<string> Name = new List<string>()
        {
            //http://listofrandomnames.com/
           "Jaleesa Lawyer","Julian Mantooth","Chester Guyette","Marie Rowser","Marti Rodrique","Melodi Shell","Christi Corter","Jenni Mast","Gabriele Iraheta","Patti Highsmith","Bart Gobel","Zulma Geddie","Ashlea Gunnells","Cornell Bochenek","Rosana Cumberland","Enoch Lauzon","Minerva Baltz","Andrea Clabaugh","Merlin Thakkar","Rosalba Dejulio","Emerita Brugger","Sally Fairchild","Eugene Albert","Temika Crean","Shelley Mansir","Nona Foor","Luella Faw","Glen Every","Rosalia Wiegand","Antonio Lofland","Seth Eggleston","Racheal Stiles","Gilberto Aziz","Kelsey Danis","Henry Harth","Bula Arguello","Felton Cicero","Lyn Mealy","Angelia Ugalde","Jovita Haglund","Romelia Bellard","Lauren Lemieux","Barbar Lovern","Leesa Gaw","Grayce Sink","Nathan Becerril","Cherise Kromer","Madalyn Pinkney","Tien Lobue","Magda Quayle" 
        };
        private static Random _random = new Random();

        public static string GetRandomName()
        {
            return Name[_random.Next(0, 49)];
        }
    }

    public class Item
    {
        public string Name { get; private set; }
        private List<Item> _children;

        public Item()
        {
            this.Name = Names.GetRandomName();
        }

        public List<Item> Children
        {
            get
            {
                //Seed
                if (_children == null)
                {
                    _children = new List<Item>();
                    for(int i = 0; i < 10; i++)
                    {
                        _children.Add(new Item());
                    }
                }
                return _children;
            }
        }
    }
}