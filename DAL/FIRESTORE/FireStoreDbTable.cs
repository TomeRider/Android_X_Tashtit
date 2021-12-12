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

using System.Threading.Tasks;
using Plugin.CloudFirestore;
using Firebase.Firestore;
using Java.Interop;
using Xamarin.Essentials;

namespace DAL.FIRESTORE
{
    public enum Order_By_Direction { ACSCENDING, DESCENDING };

    public abstract class FireStoreDbTable<TEntity, TCollection> where TEntity : new() where TCollection : List<TEntity>, new()
    {
        public static async Task<TCollection> SelectAll(string orderBy = "", Order_By_Direction order_By_Direction = Order_By_Direction.ACSCENDING)
        {
            IQuerySnapshot query;
            TCollection    collection = new TCollection();
            List<TEntity>  entityList = new List<TEntity>();

            try
            {
                if (orderBy == "")
                {
                    query = await FireStoreDB.Connection
                                  .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                                  .GetAsync();
                }
                else
                {
                    if (order_By_Direction == Order_By_Direction.ACSCENDING)
                        query = await FireStoreDB.Connection
                                      .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                                      .OrderBy(orderBy, false)
                                      .GetAsync();
                    else
                        query = await FireStoreDB.Connection
                                      .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                                      .OrderBy(orderBy, true)
                                      .GetAsync();
                }

                entityList = query.ToObjects<TEntity>().ToList();

                if (entityList != null)
                    collection.AddRange(entityList);
            }
            catch (Exception e)
            {

            }

            return collection;
        }

        private void FetchAndListen()
        {
            //FireStoreDB.Connection.Collection("Cities").AddSnapshotListener(this);
        }

        public static async Task<bool> Insert(TEntity entity)
        {
            try
            {
                DocumentReferenceWrapper docWrapper = (DocumentReferenceWrapper)await FireStoreDB.Connection
                      .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                      .AddAsync(entity);

                entity.GetType().GetProperty("IdFs").SetValue(entity, docWrapper.Id);

                      //.AddDocumentAsync(entity);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<bool> Update(TEntity entity)
        {
            try
            {
                //string v = entity.GetType().GetProperty("Id").GetValue(entity).ToString();

                //await FireStoreDB.Connection
                //      .GetCollection(COLLECTION_NAME)
                //      .GetDocumentsAsync(v)
                //      .UpdateDataAsync(entity);

                await FireStoreDB.Connection
                      .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                      .GetDocument(entity.GetType().GetProperty("IdFs").GetValue(entity).ToString())
                      .UpdateDataAsync(entity);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<bool> Delete(TEntity entity)
        {
            try
            {
                await FireStoreDB.Connection
                      .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                      .GetDocument(entity.GetType().GetProperty("IdFs").GetValue(entity).ToString())
                      .DeleteDocumentAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static async Task<TEntity> Select(string id)
        {
            try
            {
                IQuerySnapshot query = await FireStoreDB.Connection
                                             .GetCollection(typeof(TCollection).Name /*COLLECTION_NAME*/)
                                             .WhereEqualsTo("Id", id)
                                             .GetDocumentsAsync();

                if (query.Count >= 1)
                    return query.ToObjects<TEntity>().ToList()[0];
                else
                    return default(TEntity);
            }
            catch (Exception e)
            {
                return default(TEntity);
            }
        }

        public void OnEvent(Java.Lang.Object obj, FirebaseFirestoreException error)
        {
            throw new NotImplementedException();
        }

        public void SetJniIdentityHashCode(int value)
        {
            throw new NotImplementedException();
        }

        public void SetPeerReference(JniObjectReference reference)
        {
            throw new NotImplementedException();
        }

        public void SetJniManagedPeerState(JniManagedPeerStates value)
        {
            throw new NotImplementedException();
        }

        public void UnregisterFromRuntime()
        {
            throw new NotImplementedException();
        }

        public void DisposeUnlessReferenced()
        {
            throw new NotImplementedException();
        }

        public void Disposed()
        {
            throw new NotImplementedException();
        }

        public void Finalized()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}