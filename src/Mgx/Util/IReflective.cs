namespace Chaotx.Mgx {
    // deprecated
    interface IReflective {
        // deprecated
        bool WasPropertySet(string propertyName);

        bool IsDeclared(string propertyName);
        
        // deprecated (?)
        void RawSet(string propertyName, object value);
        // object RawGet(string fieldName);
    }
}