using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Xml;

namespace naumlab2
{
    interface IStrategy
    {
        List<Resourse> search(Resourse query, string file);
    }



    class Linq2Xml : IStrategy
    {
        public List<Resourse> search(Resourse query, string file)
        {
            var doc = XDocument.Load(file);
            var results = from obj in doc.Descendants("resourse")
                          where
                        (
                            Resourse.comp(obj.Attribute("name").Value, query.name) &&
                            Resourse.comp(obj.Attribute("info").Value, query.info) &&
                            Resourse.comp(obj.Attribute("annotation").Value, query.annotation) &&
                            Resourse.comp(obj.Attribute("type").Value, query.type) &&
                            Resourse.comp(obj.Attribute("address").Value, query.address) &&
                            Resourse.comp(obj.Attribute("conditions").Value, query.conditions) &&
                            Resourse.comp(obj.Elements().First().Attribute("name").Value, query.author.name) &&
                            Resourse.comp(obj.Elements().First().Attribute("faculty").Value, query.author.faculty) &&
                            Resourse.comp(obj.Elements().First().Attribute("cathedra").Value, query.author.cathedra)
                        )
                          select new Resourse(
                              obj.Attribute("name").Value,
                              obj.Attribute("info").Value,
                              obj.Attribute("annotation").Value,
                              obj.Attribute("type").Value,
                              obj.Attribute("address").Value,
                              obj.Attribute("conditions").Value,
                              new Author(
                                  obj.Elements().First().Attribute("name").Value,
                                  obj.Elements().First().Attribute("faculty").Value,
                                  obj.Elements().First().Attribute("cathedra").Value
                                  )
                              );

            return results.ToList();
        }
    }

    class SAX : IStrategy
    {
        public List<Resourse> search(Resourse query, string file)
        {
            List<Resourse> all = new List<Resourse>();
            var reader = new XmlTextReader(file);
            while (reader.Read())
            {
                if (reader.HasAttributes)
                {
                    Resourse res = new Resourse("", "", "", "", "", "", new Author("", "", ""));
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.Name.Equals("name") && Resourse.comp(reader.Value, query.name))
                        {
                            res.name = reader.Value;
                            reader.MoveToNextAttribute();
                            if (reader.Name.Equals("info") && Resourse.comp(reader.Value, query.info))
                            {
                                res.info = reader.Value;
                                reader.MoveToNextAttribute();
                                if (reader.Name.Equals("annotation") && Resourse.comp(reader.Value, query.annotation))
                                {
                                    res.annotation = reader.Value;
                                    reader.MoveToNextAttribute();
                                    if (reader.Name.Equals("type") && Resourse.comp(reader.Value, query.type))
                                    {
                                        res.type = reader.Value;
                                        reader.MoveToNextAttribute();
                                        if (reader.Name.Equals("address") && Resourse.comp(reader.Value, query.address))
                                        {
                                            res.address = reader.Value;
                                            reader.MoveToNextAttribute();
                                            if (reader.Name.Equals("conditions") && Resourse.comp(reader.Value, query.conditions))
                                            {
                                                res.conditions = reader.Value;
                                                reader.Read();
                                                reader.Read();
                                                reader.MoveToNextAttribute();
                                                if (reader.Name.Equals("name") && Resourse.comp(reader.Value, query.author.name))
                                                {
                                                    res.author.name = reader.Value;
                                                    reader.MoveToNextAttribute();
                                                    if (reader.Name.Equals("faculty") && Resourse.comp(reader.Value, query.author.faculty))
                                                    {
                                                        res.author.faculty =reader.Value;
                                                        reader.MoveToNextAttribute();
                                                        if (reader.Name.Equals("cathedra") && Resourse.comp(reader.Value, query.author.cathedra))
                                                        {
                                                            res.author.cathedra = reader.Value;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (res.name != "" && res.info != "" && res.annotation != "" &&
                        res.type != "" && res.address != "" && res.conditions != "" &&
                        res.author.name != "" && res.author.faculty != "" && res.author.cathedra != "") all.Add(res);
                }
            }

            return all;
        }
    }

    class DOM : IStrategy
    {
        public List<Resourse> search(Resourse query, string file)
        {
            List<Resourse> all = new List<Resourse>();
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode node = doc.DocumentElement;
            foreach(XmlNode nod in node.ChildNodes)
            {
                Resourse res = new Resourse("", "", "", "", "", "", new Author("", "", ""));
                foreach(XmlAttribute attr in nod.Attributes)
                {
                    if (attr.Name.Equals("name") && Resourse.comp(attr.Value, query.name)) res.name = attr.Value;
                    if (attr.Name.Equals("info") && Resourse.comp(attr.Value, query.info)) res.info = attr.Value;
                    if (attr.Name.Equals("annotation") && Resourse.comp(attr.Value, query.annotation)) res.annotation = attr.Value;
                    if (attr.Name.Equals("type") && Resourse.comp(attr.Value, query.type)) res.type = attr.Value;
                    if (attr.Name.Equals("address") && Resourse.comp(attr.Value, query.address)) res.address = attr.Value;
                    if (attr.Name.Equals("conditions") && Resourse.comp(attr.Value, query.conditions)) res.conditions = attr.Value;
                }
                foreach(XmlAttribute attr in nod.ChildNodes[0].Attributes)
                {
                    if (attr.Name.Equals("name") && Resourse.comp(attr.Value, query.author.name)) res.author.name = attr.Value;
                    if (attr.Name.Equals("faculty") && Resourse.comp(attr.Value, query.author.faculty)) res.author.faculty = attr.Value;
                    if (attr.Name.Equals("cathedra") && Resourse.comp(attr.Value, query.author.cathedra)) res.author.cathedra = attr.Value;
                }
                if (res.name != "" && res.info != "" && res.annotation != "" &&
                    res.type != "" && res.address != "" && res.conditions != "" &&
                    res.author.name != "" && res.author.faculty != "" && res.author.cathedra != "") all.Add(res);
            }
            return all;
        }

    }
}
