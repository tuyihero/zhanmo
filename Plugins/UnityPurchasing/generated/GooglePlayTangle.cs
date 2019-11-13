#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("v/QcNs0kOjyP6Xs8Y47OrAL3pOF1hE763bIbawHsunpLbvQNTfe/e7MuLw9FUTGNMpMs08/8aJhGuJ8GjEn/OTBQ7DdVzFIP/cd+dDpz/BaSj3uE2riYw6RI2QdA2Lxk6P1gZLx7FAuL99fFIvaY0qzvCb0DyOs9ji/Awc89DrnbLcjfwz+WQNtU/v2d26xplp4pRl/ZBgUiX+2Nqo+tOVoi7lMvZWkNqi824U/Y0y7eppvXL3KwAPWmdOarSlChOzSVLb0GRTcdLECcJs5RtIBQdeeVguDzFXuACi7ZTF4KciwKqTkzBQvcrSrdo1YRevn3+Mh6+fL6evn5+GpzOlsmZVDIevnayPX+8dJ+sH4P9fn5+f34++Zd2YUaXSwsK/r7+fj5");
        private static int[] order = new int[] { 11,10,13,12,4,5,9,13,13,13,12,13,12,13,14 };
        private static int key = 248;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
