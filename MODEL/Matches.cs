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
   public class Matches:BaseList_FS<Match>
    {
        public async Task<Matches> SelectAll()
        {
            Matches matches = await FireStoreDbTable<Match, Matches>.SelectAll("p_name", Order_By_Direction.ACSCENDING);
            return matches;
        }

        public async Task<bool> Save()
        {
            GenereteUpdateLists();

            if (InsertList.Count > 0)
                foreach (Match m in InsertList)
                    await FireStoreDbTable<Match, Matches>.Insert(m);

            if (UpdateList.Count > 0)
                foreach (Match m in UpdateList)
                    await FireStoreDbTable<Match, Matches>.Update(m);

            if (DeleteList.Count > 0)
                foreach (Match m in DeleteList)
                    await FireStoreDbTable<Match, Matches>.Delete(m);

            return base.Save();
        }

        public override bool Exist(Match entity, out Match existEntity)
        {
            existEntity = Find(item => item.GameId.Equals(entity.GameId));
            return existEntity != null;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.RivalTeamName.CompareTo(item2.RivalTeamName));
        }

        public event EventHandler<ProductEventArgs> OnMatchesRetrieved;
        public class ProductEventArgs : EventArgs
        {
            public Matches Matches { get; set; }
        }

    }

}
