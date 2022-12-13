using System;
using System.Collections.Generic;
using System.Text;

namespace naumlab2
{
    public class Resourse
    {

        public Resourse(string _name, string _info, string _annotation, string _type, string _address, string _conditions, Author _author)
        {
            author = new Author(_author.name, _author.faculty, _author.cathedra);
            name = _name;
            info = _info;
            annotation = _annotation;
            type = _type;
            address = _address;
            conditions = _conditions;
        }

        public string name;
        public string info;
        public string annotation;
        public string type;
        public string address;
        public string conditions;
        public Author author;

        public static bool comp(string my, string check)
        {
            if (my == check || my == null || my == "" || check == null || check == "") return true;
            else return false;
        }

        public string converter()
        {
            string res = $"Resourse name = {name}\n" +
                  $"Resourse name = {name}\n" +
                  $"info = {info}\n" +
                  $"Annotattion = {annotation}\n" +
                  $"Type = {type}\n" +
                  $"Address = {address}\n" +
                  $"Conditions = {conditions}\n" +
                  $"Author name = {author.name}\n" +
                  $"Faculty = {author.faculty}\n" + 
                  $"Cathedra = {author.cathedra}\n";
            return res;
        }
    }
    public class Author
    {
        public Author (string _name, string _faculty, string _cathedra)
        {
            name = _name;
            faculty = _faculty;
            cathedra = _cathedra;
        }

        public string name;
        public string faculty;
        public string cathedra;
    }
}
