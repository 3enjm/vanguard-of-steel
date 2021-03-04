using System;

namespace VanguardOfSteel.Weapons
{
    [Serializable]
    public class EmptyMagazineException : Exception
    {
        public EmptyMagazineException():base() { }
        public EmptyMagazineException(string message):base(message) { }
        public EmptyMagazineException(string message, Exception inner)
            :base(message, inner) { }
        
        protected EmptyMagazineException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            :base (info, context) { }
    }
}