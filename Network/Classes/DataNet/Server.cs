using Android.App;
using Android.Content;
using System;

namespace Network.Classes.DataNet
{
    class Server
    {
        private static ISharedPreferences _preferences = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
        private static ISharedPreferencesEditor _preferencesEdit = _preferences.Edit();
        public static void SaveData ()
        {
            string[] properties = { "QuantityParts", "LengthConnect", "SizeParts", "SpeedParts", "IdPartColor", "IdBackgroundColor" };

            foreach (string property in properties)
            {
                try
                { string s = typeof(NetState).GetProperty(property).Name; }
                catch (Exception)
                { throw new InvalidOperationException(); } // Свойство не найдено, ДОДЕЛАТЬ

                _preferencesEdit.PutInt(property, (int) typeof(NetState).GetProperty(property).GetValue(typeof(NetState)));
            } 
            _preferencesEdit.Commit();
        }

        public static void LoadData ()
        {
            string[] properties = { "QuantityParts", "LengthConnect", "SizeParts", "SpeedParts", "IdPartColor", "IdBackgroundColor" };
            foreach (string property in properties)
            {
                try
                {
                    typeof(NetState).GetProperty(property).SetValue(typeof(NetState), _preferences.GetInt(property, 0));
                }
                catch (Exception)
                {
                    NetState.QuantityParts = 150;
                    NetState.LengthConnect = 100;
                    NetState.SizeParts = 4;
                    NetState.SpeedParts = 3;

                    NetState.IdPartColor = Resource.String.Aquamarine;
                    NetState.IdBackgroundColor = Resource.String.Black;
                }
            }
        }
    }
}