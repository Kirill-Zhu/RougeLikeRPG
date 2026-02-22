using UnityEngine;
using System.Collections.Generic;

namespace MyMethods {

    public static class StaticMethods 
    {
        public static J[] GetTypesWith<T, J>(J[] inputTypes) {
    
            List<J> types = new List<J>();
            foreach (var type in inputTypes) {
                if (type.GetType() is T) {
                    types.Add(type);
                }
            }
                return types.ToArray();
        }
    }
}
