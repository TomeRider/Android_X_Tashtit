using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DAL.FIRESTORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
 public class Players:BaseList_FS<Player>
    {
        public async Task<Players> SelectAll()
        {
            Players players = await FireStoreDbTable<Player, Players>.SelectAll("p_name", Order_By_Direction.ACSCENDING);
            return players;
        }

        public async Task<bool> Save()
        {
            GenereteUpdateLists();

            if (InsertList.Count > 0)
                foreach (Player p in InsertList)
                    await FireStoreDbTable<Player, Players>.Insert(p);

            if (UpdateList.Count > 0)
                foreach (Player p in UpdateList)
                    await FireStoreDbTable<Player, Players>.Update(p);

            if (DeleteList.Count > 0)
                foreach (Player p in DeleteList)
                    await FireStoreDbTable<Player, Players>.Delete(p);

            return base.Save();
        }

        public override bool Exist(Player entity, out Player existEntity)
        {
            existEntity = Find(item => item.P_identifyNumber.Equals(entity.P_identifyNumber));
            return existEntity != null;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.P_name.CompareTo(item2.P_name));
        }

        public event EventHandler<ProductEventArgs> OnPlayersRetrieved;
        public class ProductEventArgs : EventArgs
        {
            public Players players { get; set; }
        }

        }
    }

