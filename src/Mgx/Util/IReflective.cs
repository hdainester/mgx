namespace Chaotx.Mgx {
    interface IReflective {
        bool WasPropertySet(string propertyName);
        void RawSet(string propertyName, object value);
        // object RawGet(string fieldName);
    }
}