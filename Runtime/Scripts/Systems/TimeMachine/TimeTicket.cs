using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.Systems
{
    public interface ITimeTicket : IDisposable
    {
        int? id { get; }
        TimeTicket.TicketType type { get; }

        void Unregister();
    }
    public struct TimeTicket : ITimeTicket
    {
        public enum TicketType
        {
            Update          = 1,
            LateUpdate      = 2,
            FixedUpdate     = 3,
        }
        public int? id { get; internal set; }
        public TicketType type { get; internal set; }
        
        public TimeTicket(int _id, TicketType _type)
        {
            this.id = _id;
            this.type = _type;
        }
         

        public void Unregister()
        {
            if (id == null) return;
            switch (this.type)
            {
                case TicketType.Update:
                    TimeMachine.RemoveUpdate(id.Value);
                    break;
                case TicketType.LateUpdate:
                    TimeMachine.RemoveLateUpdate(id.Value);
                    break;
                case TicketType.FixedUpdate:
                    TimeMachine.RemoveFixedUpdate(id.Value);
                    break;
            }
        }

        public void Dispose()
        {
            this.Unregister();
        }

        public override bool Equals(object obj)
        {
            return obj is TimeTicket ticket &&
                   id == ticket.id &&
                   type == ticket.type;
        }

        public override int GetHashCode()
        {
            int hashCode = -1056084179;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + type.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(TimeTicket left, TimeTicket right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TimeTicket left, TimeTicket right)
        {
            return !(left == right);
        }
    }
}
