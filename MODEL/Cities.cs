using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using DAL.FIRESTORE;
using Plugin.CloudFirestore;

namespace MODEL
{
    public class Cities : BaseList_FS<City>
    {
        public Cities() { }

        public async Task<Cities> SelectAll()
        {
            Cities cities = await FireStoreDbTable<City, Cities>.SelectAll("Name", Order_By_Direction.ACSCENDING);
            return cities;
        }

        public async Task<bool> Save()
        {
            GenereteUpdateLists();

            if (InsertList.Count > 0)
                foreach (City c in InsertList)
                    await FireStoreDbTable<City, Cities>.Insert(c);

            if (UpdateList.Count > 0)
                foreach (City c in UpdateList)
                    await FireStoreDbTable<City, Cities>.Update(c);

            if (DeleteList.Count > 0)
                foreach (City c in DeleteList)
                    await FireStoreDbTable<City, Cities>.Delete(c);

            return base.Save();
        }

        public override bool Exist(City entity, out City existEntity)
        {
            existEntity = Find(item => item.Name.Equals(entity.Name));
            return existEntity != null;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.Name.CompareTo(item2.Name));
        }

        public event EventHandler<ProductEventArgs> OnCitiesRetrieved;
        public class ProductEventArgs : EventArgs
        {
            public Cities cities { get; set; }
        }

        public void FetchAndListen()
        {
            Cities citiesList = new Cities();

        FireStoreDB.Connection.Collection("Cities")
                                   .AddSnapshotListener((snapshot, error) =>
                                   {
                                       if (snapshot != null)
                                       {
                                           foreach (var documentChange in snapshot.DocumentChanges)
                                           {
                                               switch (documentChange.Type)
                                               {
                                                   case DocumentChangeType.Added:
                                                       // Document Added
                                                       citiesList.Add(documentChange.Document.ToObject<City>());
                                                       break;
                                                   case DocumentChangeType.Modified:
                                                       // Document Modified
                                                       break;
                                                   case DocumentChangeType.Removed:
                                                       // Document Removed
                                                       break;
                                               }
                                           }
                                       }
                                   });

            if (this.OnCitiesRetrieved != null)
            {
                OnCitiesRetrieved.Invoke(this, new ProductEventArgs { cities = citiesList });
            }

        }
    }
}
