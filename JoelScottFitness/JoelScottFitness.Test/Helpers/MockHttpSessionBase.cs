using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace JoelScottFitness.Test.Helpers
{
    public class MockHttpSessionBase : HttpSessionStateBase
    {
        NameValueCollection keyCollection = new NameValueCollection();
        Dictionary<string, object> sessionStateDictionary = new Dictionary<string, object>();
        public override object this[string name]
        {
            get
            {
                if (sessionStateDictionary.ContainsKey(name))
                    return sessionStateDictionary[name];
                return null;
            }
            set
            {
                sessionStateDictionary[name] = value;
                keyCollection[name] = null;
            }
        }

        public override int Count
        {
            get { return sessionStateDictionary.Count; }
        }

        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get { return keyCollection.Keys; }
        }

        public override void Remove(string name)
        {
            if (sessionStateDictionary.ContainsKey(name))
                sessionStateDictionary.Remove(name);

            foreach (var key in keyCollection.AllKeys)
            {
                if (key == name)
                    keyCollection.Remove(name);
            }
        }

        public override void Add(string name, object value)
        {
            sessionStateDictionary[name] = value;
            keyCollection[name] = null;
        }

        public override void Clear()
        {
            keyCollection = new NameValueCollection();
            sessionStateDictionary = new Dictionary<string, object>();
        }
    }
}
