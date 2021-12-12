using System;
using System.Collections.Generic;
using System.Text;

using SQLite;
using Plugin.CloudFirestore.Attributes;

namespace MODEL
{
    public enum EntityStatus { ADDED, MODIFIED, DELETED, UNCHANGED }

    [Serializable]
    public abstract class BaseEntity
    {
        private int          id;
        private string       idfs;
        private EntityStatus entityStatus;

        protected BaseEntity() : this(EntityStatus.UNCHANGED) { }

        protected BaseEntity(EntityStatus entityStatus)
        {
            this.entityStatus = entityStatus;
        }

        public abstract bool Validate();

        [PrimaryKey, AutoIncrement]     // SQlite
        [Ignored]
        public int Id { get => id; set => id = value; }

        [Id]                            // FireStore
        [Ignore]
        public string IdFs { get => idfs; set => idfs = value; }

        [Ignore]                        // SQLite
        [Ignored]                       // FireStore
        public EntityStatus EntityStatus { get => entityStatus; set => entityStatus = value; }

        public override bool Equals(object obj)
        {
            return obj is BaseEntity entity &&
                   id           == entity.id &&
                   entityStatus == entity.entityStatus;
        }

        public static bool operator ==(BaseEntity left, BaseEntity right)
        {
            return EqualityComparer<BaseEntity>.Default.Equals(left, right);
        }

        public static bool operator !=(BaseEntity left, BaseEntity right)
        {
            return !(left == right);
        }
    }
}
