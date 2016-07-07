using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMN.Model
{
    public class Holiday
    {
        public Holiday(DateTime holiday, string reason, DateTime createddate)
        {
            this._holiDate = holiday;
            this._reason = reason;
            this._day = holiday.DayOfWeek;
            this._createdDate = createddate;
            

        }
        private DateTime _holiDate;
        private string _reason;
        private DayOfWeek _day;
        private DateTime _createdDate;

        public DateTime CreatedDate
        {
            get { return _createdDate; }
            set { _createdDate = value; }
        }
        

        public DayOfWeek Day
        {
            get { return _day; }
            set { _day = value; }
        }
             

        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }
        

        public DateTime Holidate
        {
            get { return _holiDate; }
            set { _holiDate = value; }
        }
        
    }
}
