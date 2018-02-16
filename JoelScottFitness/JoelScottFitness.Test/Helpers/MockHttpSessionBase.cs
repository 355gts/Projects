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
    }
}
