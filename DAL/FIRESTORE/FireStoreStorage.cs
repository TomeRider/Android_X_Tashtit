using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Plugin.FirebaseStorage;
using System.Threading.Tasks;
using Android.Gms.Extensions;
using System.IO;

namespace DAL.FIRESTORE
{
    public abstract class FireStoreStorage
    {
        public static async Task SaveToStorage(string id, byte[] bytes, string path = null)
        {
            try
            {
                var x = FireStoreDB.Instance;
                var reference = CrossFirebaseStorage.Current.Instance.RootReference;

                reference = MakePath(reference, path);
                reference = reference.Child(id);

                var y = /*await*/ reference.PutBytesAsync(bytes);
            }
            catch (Exception ex)
            {

            }
        }

        public static async Task<byte[]> LoadFromStorage(string id, string path = null)
        {
            try
            {
                var x = FireStoreDB.Instance;
                var reference = CrossFirebaseStorage.Current.Instance.RootReference;

                reference = MakePath(reference, path);
                reference = reference.Child(id);

                var maxDownloadSizeBytes = 1024 * 1024;

                var bytes = await reference.GetBytesAsync(maxDownloadSizeBytes);

                return bytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task DeleteFromStorage(string id, string path = null)
        {
            try
            {
                var x = FireStoreDB.Instance;
                var reference = CrossFirebaseStorage.Current.Instance.RootReference;

                reference = MakePath(reference, path);
                reference = reference.Child(id);

                await reference.DeleteAsync();
            }
            catch (Exception e)
            {

            }
        }

        private static IStorageReference MakePath (IStorageReference reference, string path)
        {
            if (path != null)
            {
                string[] vs = path.Split(new char[] { '/', '\\' });

                if (vs.Length > 0)
                    foreach (string v in vs)
                        reference = reference.Child(v);
            }

            return reference;
        }
    }
}