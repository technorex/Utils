using System;
using System.Collections.Generic;
using System.Reflection;

namespace TechnoRex.Utils.Reflection
{
    //T => typ Atrybutu
    //M => typ obiektówy znajdujących się w liscie przekazywanej do konstruktora
    public class AttrManager<T, M> where T: class
        where M : class
    {
        //obiekt, w którym będą ustawiane pola oznaczone atrybutem
        private List<M> mainObjects { get; }


        //klucz => typ jaki jest oznaczony atrybutem
        //value => obiekt jaki ma się znaleźć w polu docelowym
        private Dictionary<Type, object> dicObj { get; }


        public AttrManager(List<M> mainObject)
        {
            this.mainObjects = mainObject;
            this.dicObj = new Dictionary<Type, object>();
        }
        public AttrManager(M mainObject)
        {
            List<M> list = new List<M>();
            list.Add(mainObject);
            this.mainObjects = list;
            this.dicObj = new Dictionary<Type, object>();
        }


        //Typ jaki będzie rzutowany z objektu attrValue na obiekt docelowy oznaczony atrybutem. Typ I musi być taki sam jak obiekt docelowy oznaczony atrybutem
        public void AddAttrObj<I>(object attrValue)
        {
            if (dicObj.ContainsKey(typeof(I))) return;
            dicObj.Add(typeof(I), attrValue);
        }


        public void Compose()
        {
            foreach (var obj in mainObjects)
            {
                if (obj == null) continue;
                PropertyInfo[] properties = obj.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    T attribute = Attribute.GetCustomAttribute(property, typeof (T)) as T;
                    if (attribute != null)
                    {
                        foreach (var pair in dicObj)
                        {
                            if (property.PropertyType == pair.Key)
                            {
                                property.SetValue(obj, pair.Value, null);
                            }
                        }
                    }

                }
            }

        }




    }
}
