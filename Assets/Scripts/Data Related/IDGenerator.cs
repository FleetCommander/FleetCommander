using UnityEngine;

namespace Data_Related {
    [CreateAssetMenu(fileName = "IDGenerator", menuName = "IDGenerator")]
    public class IDGenerator : ScriptableObject {
    
        private int id;

        public int GetNextId() {
            id++;
            return id;
        }
    }
}
